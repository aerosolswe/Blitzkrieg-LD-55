using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSpawner : MonoBehaviour
{
    [SerializeField] private FactionSpawnerInfo info;
    [SerializeField] private SpriteRenderer spawnArea;

    private float startTime = 0.0f;
    private float spawnTime = 0.0f;
    private bool spawning = false;

    public float TotalElapsedTime
    {
        get
        {
            return Time.realtimeSinceStartup - startTime;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.Running)
        {
            StopSpawning();
            return;
        }

        if (spawning)
        {
            if (Time.realtimeSinceStartup - spawnTime > info.GetCurrentSpawnRate(TotalElapsedTime))
            {
                spawnTime = Time.realtimeSinceStartup;

                for (int i = 0; i < info.GetCurrentSpawnAmount(TotalElapsedTime); i++)
                {
                    var unit = UnitPool.GetUnit(info.id);

                    if (unit != null)
                    {
                        Vector3 spawnPosition = GetRandomPoint(spawnArea);
                        unit.Init(spawnPosition);
                        unit.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void StartSpawning()
    {
        startTime = Time.realtimeSinceStartup;
        spawnTime = Time.realtimeSinceStartup;
        spawning = true;
    }

    public void StopSpawning()
    {
        spawning = false;
    }

    public Vector3 GetRandomPoint(SpriteRenderer sr)
    {
        float minX = sr.bounds.center.x - sr.bounds.extents.x;
        float maxX = sr.bounds.center.x + sr.bounds.extents.x;
        float minY = sr.bounds.center.y - sr.bounds.extents.y;
        float maxY = sr.bounds.center.y + sr.bounds.extents.y;

        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0.0f);
    }
}
