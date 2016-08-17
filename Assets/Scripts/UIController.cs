using System;
using UnityEngine;
using System.Collections;

public class UIController : Singleton<UIController>
{
    private UIScreen screenUI;
 
    void Awake()
    {
        EventManager.GenericEnemyDeathEvent += EventManager_GenericEnemyDeathEvent;
        EventManager.GenericGameEvent += EventManager_GenericGameEvent;
        EventManager.GenericBonusEvent += EventManager_GenericBonusEvent;
    }

    void OnDestroy()
    {
        EventManager.GenericEnemyDeathEvent -= EventManager_GenericEnemyDeathEvent;
        EventManager.GenericGameEvent -= EventManager_GenericGameEvent;
        EventManager.GenericBonusEvent -= EventManager_GenericBonusEvent;
    }

    public void Init(GameUIForms uiforms, RectTransform UIRoot, Action onDone)
	{
        foreach (var uiForm in uiforms.Forms)
        {
            GameObject newForm = Instantiate(uiForm.FormPrefab) as GameObject;
            newForm.GetComponent<RectTransform>().SetParent(UIRoot);
            newForm.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            var form = newForm.GetComponent<UIForm>();
            switch (form.FormID)
            {
                    case UIFormIDs.GameStart:
                    ((UIFormStartGame)form).Init(onStartButtonClick:() => EventManager.CallGameEvent(EventManager.GameEvents.GameStart));
                    break;
                    case UIFormIDs.GameEnd:
                    ((UIFormEndGame)form).Init(onRestartButtonClick:() => EventManager.CallGameEvent(EventManager.GameEvents.RestartGame));
                    break;
            }
        }
        GameObject playerUI = Instantiate(uiforms.UIPrefab) as GameObject;
        playerUI.GetComponent<RectTransform>().SetParent(UIRoot);
        playerUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        playerUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        screenUI = playerUI.GetComponent<UIScreen>();
        onDone();
	}

    public void UpdateAll()
    {
        UpdateUIWeapon();
        UpdateUIHealth();
        UpdateUIArmor();
        UpdateUIScore();
    }

    private void EventManager_GenericBonusEvent(BonusInfo bonus)
    {
        switch (bonus.BonusID)
        {
            case BonusIDs.Health:
                UpdateUIHealth();
                break;
            case BonusIDs.Defence:
                UpdateUIArmor();
                break;
        }
    }

    private void UpdateUIArmor()
    {
        screenUI.TextArmor.text = ((int)(GameplayController.Instance.Player.Armor*100f)).ToString();
    }

    private void UpdateUIHealth()
    {
        screenUI.TextHealth.text = ((int)GameplayController.Instance.Player.Health).ToString();
    }

    private void UpdateUIWeapon()
    {
        screenUI.TextWeapon.text = GameplayController.Instance.Player.CurrentWeapon.LocalizedName;
    }

    private void UpdateUIScore()
    {
        screenUI.TextScore.text = GameplayController.Instance.SessionScore.ToString();
    }

    private void EventManager_GenericGameEvent(EventManager.GameEvents eventID)
    {
        switch (eventID)
        {
            case EventManager.GameEvents.WeaponChanged:
                UpdateUIWeapon();
                break;
            case EventManager.GameEvents.PlayerHit:
                UpdateUIArmor();
                UpdateUIHealth();
                break;
        }
    }

    private void EventManager_GenericEnemyDeathEvent(EnemyInfo enemy)
    {
        UpdateUIScore();
    }

}
