using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreCameraManager : MonoBehaviour
{
    [SerializeField] private Camera _environmentCamera;
    [SerializeField] private GameObject _cameraView;

    private ExploreLogManager _logManager;
    private EnvironmentsManager _environmentsManager;
    private DroneManager _droneManager;
    private Transform _currentEnvironment;
    private Transform _cameraTransform;
    private List<Transform> _currentSpawnedDrones;

    private const float CAMERA_OFFSET = -15f;
    private const float CAMERA_Y_POS = 9.5f;
    private const float CAMERA_COOLDOWN = 1f;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate += OnCreate;
        //menuBehaviour.OnShow += OnShow;
        menuBehaviour.OnHide += OnHide;
    }

    private void OnCreate()
    {
        _environmentsManager = EnvironmentsManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _logManager = GetComponent<ExploreLogManager>();
        _logManager.OnExploreLogShown += OnExploreLogShown;
        _currentSpawnedDrones = new List<Transform>();
        _cameraTransform = _environmentCamera.transform;
        _currentEnvironment = null;
    }

    private void OnHide()
    {
        _cameraView.SetActive(false);
        HidePreviousEvent();
    }

    private void HidePreviousEvent()
    {
        _environmentCamera.gameObject.SetActive(false);
        if (_currentEnvironment != null)
        {
            _currentEnvironment.gameObject.SetActive(false);
        }
        foreach (Transform drone in _currentSpawnedDrones)
        {
            Destroy(drone.gameObject);
        }
        _currentSpawnedDrones.Clear();
    }

    private List<Vector2> GenerateDronePositions(int droneCount)
    {
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < droneCount; i++)
        {
            Vector2 pos = new Vector2(GeneratePositionNumber(), GeneratePositionNumber());
            while (positions.Contains(pos) || (pos.x > 0 && pos.y < 0))
            {
                pos = new Vector2(GeneratePositionNumber(), GeneratePositionNumber());
            }
            positions.Add(pos);
        }
        return positions;
    }

    private int GeneratePositionNumber()
    {
        //Possible values: -8, -4, 0, 4, 8
        return (Random.Range(0, 3) * 4) * (Random.Range(0, 2) == 0 ? 1 : -1);
    }

    private void OnExploreLogShown(object sender, SquadExplorationEvent squadEvent)
    {
        HidePreviousEvent();
        
        Vector2 cameraPosition = squadEvent.Area.GetBiome().Coordinates;
        _cameraTransform.position = new Vector3(cameraPosition.x + CAMERA_OFFSET, CAMERA_Y_POS, cameraPosition.y - CAMERA_OFFSET);
        
        _currentEnvironment = _environmentsManager.GetEnvironment(squadEvent.Area.GetBiome());
        _currentEnvironment.gameObject.SetActive(true);

        List<Vector2> positions = GenerateDronePositions(squadEvent.Squad.GetDroneCount() - 1);
        int i = 0;
        foreach (Drone drone in squadEvent.Squad.GetDrones())
        {
            if (!drone.IsSquadLeader())
            {
                Transform droneModel = Instantiate(_droneManager.GetDronePrefab(drone.GetDroneData()), _currentEnvironment);
                droneModel.localPosition = new Vector3(positions[i].x, droneModel.localPosition.y, positions[i].y);
                droneModel.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                _currentSpawnedDrones.Add(droneModel);
                i++;
            }
        }

        StartCoroutine(TurnOnCamera());
    }

    private IEnumerator TurnOnCamera()
    {
        yield return new WaitForSeconds(CAMERA_COOLDOWN);
        _environmentCamera.gameObject.SetActive(true);
        _cameraView.SetActive(true);
    }

    private void OnDestroy()
    {
        _logManager.OnExploreLogShown -= OnExploreLogShown;
    }
}
