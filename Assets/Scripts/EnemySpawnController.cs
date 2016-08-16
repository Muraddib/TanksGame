using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnController : Singleton<EnemySpawnController>
{
    public int CurrentEnemyCount;
    public int MaxEnemyCount;
    public float SpawnPointRadius;
    public EnemyInfo[] Enemies;
    public Transform[] SpawnPoints;
    public float SpawnDelay;

    private void Awake()
    {
        EventManager.GenericGameEvent += GameEventHandler;
        EventManager.GenericEnemyDeathEvent += EventManager_GenericEnemyDeathEvent;
    }

    private void EventManager_GenericEnemyDeathEvent(EnemyInfo enemy)
    {
        CurrentEnemyCount--;
        StartCoroutine(SpawnEnemy());
    }

    public void Init(GameSettings settings)
    {
        SpawnPoints = GetSpawnPoints();
        CurrentEnemyCount = 0;
        SpawnDelay = settings.SpawnDelay;
        SpawnPointRadius = settings.SpawnPointRadius;
    }

    public void GameEventHandler(EventManager.GameEvents gameEvent)
    {
        switch (gameEvent)
        {
                case EventManager.GameEvents.GameStart:
                StartCoroutine(SpawnEnemy());
                break;
                case EventManager.GameEvents.EnemyDied:
                CurrentEnemyCount--;
                StartCoroutine(SpawnEnemy());
                break;
                case EventManager.GameEvents.RestartGame:
                StopAllCoroutines();
                break;
        }
    }

    private void OnDestroy()
    {
        EventManager.GenericGameEvent -= GameEventHandler;
        EventManager.GenericEnemyDeathEvent -= EventManager_GenericEnemyDeathEvent;
    }

    private IEnumerator SpawnEnemy()
    {
        while (CurrentEnemyCount < GameplayController.Instance.Settings.MaxEnemyCount)
        {
            yield return new WaitForSeconds(SpawnDelay);
            int rndEnemy = Random.Range(0, GameplayController.Instance.Settings.Enemies.Length);
            int rndPoint = Random.Range(0, SpawnPoints.Length);
            
            GameObject enemy = ObjectPool.Instance.GetEnemy((EnemyIDs)rndEnemy);
            if (enemy)
            {
                enemy.transform.position = SpawnPoints[rndPoint].position +
                                           new Vector3(Random.insideUnitCircle.x*SpawnPointRadius, 0f,
                                                       Random.insideUnitCircle.y*SpawnPointRadius);
                enemy.transform.rotation = Quaternion.identity;

                CurrentEnemyCount++;
            }
        } 
    }

    private Transform[] GetSpawnPoints()
    {
        GameObject SpawnPointsContainer = GameObject.FindGameObjectWithTag("spawn");
        List<Transform> spawnPoints = new List<Transform>();
        if (SpawnPointsContainer != null)
        {
            foreach (Transform child in SpawnPointsContainer.transform)
            {
                if (child != SpawnPointsContainer.transform)
                {
                    spawnPoints.Add(child);
                }
            }
        }
        return spawnPoints.ToArray();
    }
}
