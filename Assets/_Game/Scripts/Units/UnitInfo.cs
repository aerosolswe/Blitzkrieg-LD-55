using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnityInfo", menuName = "Scriptables/UnitInfo", order = 1)]
public class UnitInfo : ScriptableObject
{
    public Faction faction = Faction.Allied;
    public CursorUnit cursorPrefab;
    public UnitSpawn spawnPrefab;

    public string id = "unit_id";
    public int cost = 0;
    public int creditsReward = 0;
    public float cooldown = 1.0f;

    public int health = 60;
    public int damage = 20;
    public float firerate = 1.0f;
    public float range = 5.0f;
    public float aoe = 0.0f;

    public float maxX = 0.0f;
    public float speed = 1.0f;
    public float turnSpeed = 5.0f;
    public float attackDelay = 0.5f;

    public string corpseID = "corpse_id";

    [Header("Building")]
    public float buildTime = 5.0f;
    public UnitInfo buildUnit;

}
