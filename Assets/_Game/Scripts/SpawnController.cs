using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [Serializable]
    public class Spawner
    {
        public FactionSpawner FactionSpawner;
        public float startDelay = 0.0f;
        public bool started = false;
    }

    [SerializeField] private List<Spawner> spawners;

    private bool started = false;
    private float startTime = 0.0f;

    public void Begin()
    {
        startTime = Time.realtimeSinceStartup;
        started = true;

        foreach (Spawner spawner in spawners)
        {
            spawner.started = false;
        }
    }

    private void FixedUpdate()
    {
        if (!started)
            return;

        foreach (Spawner spawner in spawners)
        {
            if (spawner.started)
                continue;

            if (Time.realtimeSinceStartup - startTime > spawner.startDelay)
            {
                spawner.started = true;
                spawner.FactionSpawner.StartSpawning();
            }
        }
    }

}
