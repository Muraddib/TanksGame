using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public enum GameEvents
    {
        GameLoaded = 0,
        GameStart = 1,
        PlayerDied = 2,
        EnemyDied = 3,
        WeaponChanged = 4,
        RestartGame = 5,
        PlayerHit = 6
    }

    public delegate void SoundEvent(AudioIDs sound, Vector3 pos);
    public static event SoundEvent GenericSoundEvent;

    public delegate void FireEvent(WeaponInfo weaponSource);
    public static event FireEvent GenericFireEvent;

    public delegate void GameEvent(GameEvents eventID);
    public static event GameEvent GenericGameEvent;

    public delegate void BonusEvent(BonusInfo bonus);
    public static event BonusEvent GenericBonusEvent;

    public delegate void EnemyDeath(EnemyInfo enemy);
    public static event EnemyDeath GenericEnemyDeathEvent;

    public static void CallBonusEvent(BonusInfo bonus)
    {
        if (GenericBonusEvent != null)
        {
            GenericBonusEvent(bonus);
            Debug.Log("EventManager:" + bonus);
        }
    }

    public static void CallGameEvent(GameEvents eventID)
    {
        if (GenericGameEvent != null)
        {
            GenericGameEvent(eventID);
            Debug.Log("EventManager:" + eventID);
        }
    }

    public static void CallSoundEvent(AudioIDs sound, Vector3 pos)
    {
        if (GenericSoundEvent != null)
            GenericSoundEvent(sound, pos);
    }

    public static void CallFireEvent(WeaponInfo weaponSource)
    {
        if (GenericFireEvent != null)
            GenericFireEvent(weaponSource);
    }

    public static void CallEnemyDeathEvent(EnemyInfo enemy)
    {
        if (GenericEnemyDeathEvent != null)
            GenericEnemyDeathEvent(enemy);
    }


}