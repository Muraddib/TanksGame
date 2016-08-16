using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour, IPoolable
{
    private Transform BonusRoot;
    [HideInInspector]
    public BonusInfo BonusParameters;
    public BonusIDs BonusID;
    protected bool active;

    public void Initialize()
    {
        if (BonusRoot == null) BonusRoot = transform.parent;
        BonusRoot.gameObject.SetActive(true);
        BonusParameters = GetBonusProperties();
    }

    private BonusInfo GetBonusProperties()
    {
        foreach (var info in GameplayController.Instance.Settings.Bonuses)
        {
            if (info.BonusID == BonusID)
            {
                return info;
            }
        }
        return GameplayController.Instance.Settings.Bonuses[0];
    }

    public bool IsActive
    {
        get { return active; }
        set
        {
            if (value)
            {
                Initialize();
            }
            else
            {
                Reset();
            }
            active = value;
        }
    }

    private void Reset()
    {
        if (BonusRoot == null) BonusRoot = transform.parent;
        transform.localPosition = Vector3.zero;
        BonusRoot.gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            Debug.Log(player.gameObject.name);
            player.ConsumeBonus(this);
        }
    }
}
