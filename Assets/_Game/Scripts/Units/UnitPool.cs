using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    public static UnitPool Instance = null;

    [Serializable]
    public class UnitPoolHolder
    {
        public string name;
        public Unit unit;
        public int count = 50;
        public string id = "unit_id";
        public List<Unit> list = new List<Unit>();
    }

    [SerializeField] private List<UnitPoolHolder> pools = new List<UnitPoolHolder>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < pools.Count; i++)
        {
            for (int j = 0; j < pools[i].count; j++)
            {
                Unit unit = Instantiate(pools[i].unit, transform);
                unit.gameObject.name = pools[i].name + j;
                unit.gameObject.SetActive(false);
                pools[i].list.Add(unit);
            }
        }
    }

    public static Unit GetUnit(string identifier)
    {
        foreach (var pool in Instance.pools)
        {
            if (pool.id == identifier)
            {
                foreach (var unit in pool.list)
                {
                    if (!unit.gameObject.activeInHierarchy)
                        return unit;
                }

                Unit newUnit = Instantiate(pool.unit, Instance.transform);
                newUnit.gameObject.name = pool.name + (pool.count + 1);
                newUnit.gameObject.SetActive(false);
                pool.list.Add(newUnit);
                return newUnit;
            }
        }

        return null;
    }

    public static Unit GetUnit(UnitInfo info)
    {
        return GetUnit(info.id);
    }

    public static void ResetAllUnits()
    {
        for (int i = 0; i < Instance.pools.Count; i++)
        {
            for (int j = 0; j < Instance.pools[i].count; j++)
            {
                Instance.pools[i].list[j].gameObject.SetActive(false);
            }
        }
    }
}
