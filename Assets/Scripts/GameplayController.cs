using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameplayController : Singleton<GameplayController>
{
    [HideInInspector]
    public GameSettings Settings;
    public PlayerController Player;
    public int SessionScore;
    public int MaxScore;

    void Awake()
    {
        EventManager.GenericGameEvent += GameEventHandler;
    }

    public void Init(GameSettings settings)
    {
        Settings = settings;
        SessionScore = 0;
    }

    public void GameEventHandler(EventManager.GameEvents gameEvent)
    {
        switch (gameEvent)
        {
            case EventManager.GameEvents.PlayerDied:
                break;
            case EventManager.GameEvents.RestartGame:
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
                break;
        }
    }
}
