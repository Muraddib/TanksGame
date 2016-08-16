using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GameUIForms
{
    [Header("UIForms")]
    public UIFormInfo[] Forms;

    [Header("Screen UI Prefab")] 
    public GameObject UIPrefab;
}
