using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UIFormStartGame : UIForm {

    public Button StartButton;

    public void Init(Action onStartButtonClick)
    {
        StartButton.onClick.AddListener(new UnityAction(onStartButtonClick));
        EventManager.GenericGameEvent += HandleGameEvent;
        gameObject.SetActive(false);
    }
	
    void OnDestroy()
    {
        EventManager.GenericGameEvent -= HandleGameEvent;
    }

    public override void HandleGameEvent(EventManager.GameEvents gameEvent)
    {
        switch (gameEvent)
        {
                case EventManager.GameEvents.GameLoaded:
                gameObject.SetActive(true);
                break;
                case EventManager.GameEvents.GameStart:
                gameObject.SetActive(false);
                break;
        }
    }
}
