using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool : Singleton<ObjectPool>
{
    public List<GameObject> EnemyPool;
    public List<GameObject> BonusPool;
    private int maxCountPerEnemyType;
    private int maxCountPerBonusType;

    private void SetEnemyPool(GameSettings settings)
    {
        for (int i = 0; i < settings.Enemies.Length; i++)
        {
            for (int j = 0; j < maxCountPerEnemyType; j++)
            {
                GameObject go = Instantiate(settings.Enemies[i].EnemyPrefab);
                var enemy = go.GetComponent<Enemy>();
                enemy.IsActive = false;
                switch (enemy.EnemyID)
                {
                    case EnemyIDs.OrcWarrior:
                        enemy.EnemyWeapon = settings.EnemyWeapons.First(a => a.WeaponID == WeaponIDs.Axe);
                        break;
                    case EnemyIDs.GoblinWizard:
                        enemy.EnemyWeapon = settings.EnemyWeapons.First(a => a.WeaponID == WeaponIDs.FireballSpell);
                        break;
                    case EnemyIDs.OrgeHitter:
                        enemy.EnemyWeapon = settings.EnemyWeapons.First(a => a.WeaponID == WeaponIDs.Blunt);
                        break;
                    case EnemyIDs.TrollCurer:
                        enemy.EnemyWeapon = settings.EnemyWeapons.First(a => a.WeaponID == WeaponIDs.Dagger);
                        break;
                }
                EnemyPool.Add(go);
            }
        }
    }

    private void SetBonusPool(GameSettings settings)
    {
        for (int i = 0; i < settings.Bonuses.Length; i++)
        {
            for (int j = 0; j < maxCountPerBonusType; j++)
            {
                GameObject go = Instantiate(settings.Bonuses[i].BonusPrefab);
                var bonus = go.GetComponentInChildren<Bonus>();
                bonus.IsActive = false;
                BonusPool.Add(go);
            }
        }
    }

    public void Init(GameSettings settings)
    {
        EnemyPool = new List<GameObject>();
        BonusPool = new List<GameObject>();
        maxCountPerEnemyType = GameplayController.Instance.Settings.MaxCountPerEnemyType;
        maxCountPerBonusType = GameplayController.Instance.Settings.MaxCountPerBonusType;
        SetEnemyPool(settings);
        SetBonusPool(settings);
    }

    public GameObject GetEnemy(EnemyIDs enemyRequestType)
    {
        if (EnemyPool.FindAll(a => a.GetComponent<Enemy>().EnemyID == enemyRequestType && a.GetComponent<Enemy>().IsActive).Count >= maxCountPerEnemyType)
        {
            return null;
        }

        GameObject enemy =
            EnemyPool.First(
                a => !a.GetComponent<Enemy>().IsActive && a.GetComponent<Enemy>().EnemyID == enemyRequestType);
        enemy.GetComponent<Enemy>().IsActive = true;
        return enemy;
    }

    public GameObject GetBonus(BonusIDs bonusRequestType)
    {
        if (BonusPool.FindAll(a => a.GetComponentInChildren<Bonus>().BonusID == bonusRequestType && a.GetComponentInChildren<Bonus>().IsActive).Count >= maxCountPerBonusType)
        {
            return null;
        }

        GameObject bonus =
            BonusPool.First(
                a => !a.GetComponentInChildren<Bonus>().IsActive && a.GetComponentInChildren<Bonus>().BonusID == bonusRequestType);
        bonus.GetComponentInChildren<Bonus>().IsActive = true;
        return bonus;
    }
}
