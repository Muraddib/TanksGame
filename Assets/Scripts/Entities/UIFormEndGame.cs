using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UIFormEndGame : UIForm
{
    public Button RestartButton;
    public Text ScoreResult;

    public void Init(Action onRestartButtonClick)
    {
        RestartButton.onClick.AddListener(new UnityAction(onRestartButtonClick));
        EventManager.GenericGameEvent += HandleGameEvent;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.GenericGameEvent -= HandleGameEvent;
    }

    void UpdateScoreResult()
    {
        ScoreResult.text = GameplayController.Instance.SessionScore.ToString();
    }

    public override void HandleGameEvent(EventManager.GameEvents gameEvent)
    {
        switch (gameEvent)
        {
                case EventManager.GameEvents.PlayerDied:
                gameObject.SetActive(true);
                UpdateScoreResult();
                break;
                case EventManager.GameEvents.RestartGame:
                gameObject.SetActive(false);
                break;
        }
    }
}
