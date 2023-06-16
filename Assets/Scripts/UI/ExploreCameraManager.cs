using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreCameraManager : MonoBehaviour
{
    [SerializeField] private Camera _environmentCamera;
    
    private ExploreLogManager _logManager;
    private EnvironmentsManager _environmentsManager;
    private DroneManager _droneManager;
    private Transform _currentEnvironment;
    private Transform _cameraTransform;
    private List<Transform> _currentSpawnedDrones;

    private const float CAMERA_OFFSET = -15f;
    private const float CAMERA_Y_POS = 9.5f;

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
        return (Random.Range(0, 3) * 4) * (Random.Range(0, 2) == 0 ? 1 : -1);
    }

    private void OnExploreLogShown(object sender, SquadExplorationEvent squadEvent)
    {
        //TODO spawn squad units, spawn additional event props
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
                //TODO set drone rotation
                droneModel.localPosition = new Vector3(positions[i].x, droneModel.localPosition.y, positions[i].y);
                _currentSpawnedDrones.Add(droneModel);
                i++;
            }
        }

        _environmentCamera.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _logManager.OnExploreLogShown -= OnExploreLogShown;
    }
}
