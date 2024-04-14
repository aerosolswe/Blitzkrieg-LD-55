using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionSpawnerInfo", menuName = "Scriptables/FactionSpawnerInfo", order = 2)]
public class FactionSpawnerInfo : ScriptableObject
{
    public string id = "unit_id";

    public float baseSpawnRate = 1.0f;
    public float baseSpawnAmount = 1.0f;

    public float spawnRateMax = 0.1f;
    public float spawnAmountMax = 10.0f;

    public AnimationCurve difficultyCurve;
    public float difficultyOverTime = 60;

    public float GetCurrentSpawnRate(float time)
    {
        return Mathf.Lerp(baseSpawnRate, spawnRateMax, GetCurrentDifficulty(time));
    }

    public int GetCurrentSpawnAmount(float time)
    {
        return (int)Mathf.Lerp(baseSpawnAmount, spawnAmountMax, GetCurrentDifficulty(time));
    }

    public float GetCurrentDifficulty(float time)
    {
        return difficultyCurve.Evaluate(time / difficultyOverTime);
    }
}
