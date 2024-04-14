using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSpawner : MonoBehaviour
{
    public static CursorSpawner Instance = null;

    [Serializable]
    public class CursorSpawnerRule
    {
        public UnitInfo info;
        public SpriteRenderer bounds;
        public Action onSpawned;
        public Action onCancel;
    }

    [SerializeField] private GameObject container;
    [SerializeField] private CursorSpawnerRule[] rules;

    public CursorSpawnerRule ActiveRule
    {
        get; private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        container.transform.position = mousePosition;

        if (Input.GetMouseButtonDown(0) && ActiveRule != null)
        {
            var cursorUnit = container.GetComponentInChildren<CursorUnit>();

            if (!cursorUnit.CanBePlaced)
                return;

            if (ActiveRule.info.spawnPrefab != null)
            {
                var savedUnitInfo = ActiveRule.info;
                var spawnUnit = Instantiate(ActiveRule.info.spawnPrefab); 
                spawnUnit.transform.position = mousePosition;
                spawnUnit.Show(() =>
                {
                    foreach (var u in spawnUnit.UnitSpriteRenderers)
                    {
                        var unit = UnitPool.GetUnit(savedUnitInfo);
                        unit.Init(u.transform.position);
                        unit.gameObject.SetActive(true);
                    }
                });

                ActiveRule.onSpawned?.Invoke();
            } else
            {
                var unit = UnitPool.GetUnit(ActiveRule.info);
                unit.Init(cursorUnit.transform.position);
                unit.gameObject.SetActive(true);
                ActiveRule.onSpawned?.Invoke();
            }
        }

        if (Input.GetMouseButtonDown(1) && ActiveRule != null)
        {
            Cancel();
        }
    }

    private CursorSpawnerRule FindCursorRule(UnitInfo info)
    {
        foreach (var rule in rules)
        {
            if (rule.info.id == info.id)
            {
                return rule;
            }
        }

        return null;
    }

    public void SetActiveRule(UnitButton button, UnitInfo info, Action onSpawned, Action onCancel)
    {
        ActiveRule = Instance.FindCursorRule(info);
        ActiveRule.onSpawned = onSpawned;
        ActiveRule.onCancel = onCancel;

        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }

        var cursorUnit = Instantiate(info.cursorPrefab, container.transform);
        cursorUnit.Button = button;
        cursorUnit.BoundsArea = ActiveRule.bounds;
        cursorUnit.Cost = ActiveRule.info.cost;
        ActiveRule.bounds.gameObject.SetActive(true);
    }

    public static void SetCursorUnit(UnitButton button, UnitInfo info, Action onSpawned, Action onCancel)
    {
        Instance.SetActiveRule(button, info, onSpawned, onCancel);
    }

    public static void Cancel()
    {
        Instance.ActiveRule?.bounds?.gameObject.SetActive(false);
        Instance.ActiveRule?.onCancel?.Invoke();

        for (int i = 0; i < Instance.container.transform.childCount; i++)
        {
            Destroy(Instance.container.transform.GetChild(i).gameObject);
        }

        Instance.ActiveRule = null;
    }
}
