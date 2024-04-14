using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static class Numbers
    {
        public static int Credits = 0;
        public static int Score = 0;
        public static int TotalTime = 0;
        public static int TotalKills = 0;

        public static void Reset()
        {
            Credits = 0;
            Score = 0;
            TotalTime = 0;
            TotalKills = 0;
        }
    }

    public static GameManager Instance = null;

    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private MainMenuPopup mainMenuPopup;
    //[SerializeField] private GameObject debug;
    [SerializeField] private GameObject debug;
    [SerializeField] private SpawnController spawnController;

    private float startTime = 0.0f;

    public bool Running
    {
        get;
        set;
    }

    private void Awake()
    {
        Instance = this;

#if !UNITY_EDITOR
        debug.SetActive(false);
#endif
    }

    private void Start()
    {
        mainMenuPopup.Show();
    }

    public void StartGame()
    {
        UnitPool.ResetAllUnits();
        Numbers.Reset();
        startTime = Time.realtimeSinceStartup;
        Running = true;
        spawnController.Begin();
    }

    private void Update()
    {
        if (Running)
        {
            Numbers.TotalTime = (int)(Time.realtimeSinceStartup - startTime);
        }
    }

    public void ShowMainMenu()
    {
        mainMenuPopup.Show();
    }

    public void GameOver()
    {
        CursorSpawner.Cancel();
        UnitButton.CancelOthers(null);

        Running = false;
        gameOverPopup.Show();
        // Show game over menu
    }
}

public enum Faction
{
    Allied, Axis
}
