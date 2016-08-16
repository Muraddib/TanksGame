using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FlowController : MonoBehaviour
{
    public GameSettingsKeeper SettingsKeeper;
    public GameUIFormsKeeper UIFormsKeeper;
    public AudioBank AudioKeeper;
    public RectTransform UIRoot;
    public Transform PlayerSpawn;

    private void Start()
    {
        Init();
    }
    
    private void Init()
    {
        GameplayController.Instance.Init(SettingsKeeper.GameSettings);
        EnemySpawnController.Instance.Init(SettingsKeeper.GameSettings);
        ObjectPool.Instance.Init(SettingsKeeper.GameSettings);
        AudioController.Instance.Init(AudioKeeper);
        UIController.Instance.Init(UIFormsKeeper.UIForms, UIRoot, onDone: OnInitialized);
    }

    private void OnInitialized()
    {
        PlayerController player = SetPlayer();
        GameplayController.Instance.Player = player;
        SetSceneCamera(player);
        UIController.Instance.UpdateAll();
        EventManager.CallGameEvent(EventManager.GameEvents.GameLoaded);
    }

    private PlayerController SetPlayer()
    {
        GameObject player = Instantiate(SettingsKeeper.GameSettings.PlayerPrefab, PlayerSpawn.position, Quaternion.identity) as GameObject;
        PlayerController pController = player.AddComponent<PlayerController>();
        pController.Init(SettingsKeeper.GameSettings);
        return pController;
    }

    private void SetSceneCamera(PlayerController player)
    {
        GameObject sceneCamera = Instantiate(SettingsKeeper.GameSettings.CameraPrefab);
        SmoothFollow followCam = sceneCamera.GetComponent<SmoothFollow>();
        followCam.FollowTarget = player.gameObject.transform;
    }
}
