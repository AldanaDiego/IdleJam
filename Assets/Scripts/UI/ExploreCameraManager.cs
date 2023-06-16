using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreCameraManager : MonoBehaviour
{
    [SerializeField] private Camera _environmentCamera;
    private ExploreLogManager _logManager;
    private EnvironmentsManager _environmentsManager;
    private Transform _currentEnvironment;
    private Transform _cameraTransform;

    private const float CAMERA_OFFSET = -8f;
    private const float CAMERA_Y_POS = 6f;

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
        _logManager = GetComponent<ExploreLogManager>();
        _logManager.OnExploreLogShown += OnExploreLogShown;
        _currentEnvironment = null;
        _cameraTransform = _environmentCamera.transform;
    }

    private void OnHide()
    {
        if (_currentEnvironment != null)
        {
            _currentEnvironment.gameObject.SetActive(false);
            _currentEnvironment = null;
        }
        _environmentCamera.gameObject.SetActive(false);
    }

    private void HidePreviousEvent()
    {
        _environmentCamera.gameObject.SetActive(false);
        if (_currentEnvironment != null)
        {
            _currentEnvironment.gameObject.SetActive(false);
        }
        //TODO despawn squad
    }

    private void OnExploreLogShown(object sender, SquadExplorationEvent squadEvent)
    {
        //TODO despawn previous event,update camera position, get environment to show, show environment, spawn squad units, spawn additional event props
        HidePreviousEvent();
        
        Vector2 cameraPosition = squadEvent.Area.GetBiome().Coordinates;
        _cameraTransform.position = new Vector3(cameraPosition.x + CAMERA_OFFSET, CAMERA_Y_POS, cameraPosition.y - CAMERA_OFFSET);
        
        _currentEnvironment = _environmentsManager.GetEnvironment(squadEvent.Area.GetBiome());
        _currentEnvironment.gameObject.SetActive(true);

        _environmentCamera.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _logManager.OnExploreLogShown -= OnExploreLogShown;
    }
}
