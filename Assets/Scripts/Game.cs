using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [Header("UI")]
    [SerializeField] private Canvas Menu;
    [SerializeField] private Canvas Hud;
    [SerializeField] private Canvas GameOver;
    [Header("Controllers")]
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private MoneyManager money;

    private Environment mMap;

    private CameraController cameraController;

    public int health { get; private set; }

    void Start()
    {
        mMap = GetComponentInChildren<Environment>();
        ShowMenu(true);
        cameraController = MainCamera.GetComponent<CameraController>();
    }

    private void Update()
    {

        if (Input.mouseScrollDelta.y > 0)
        {
            cameraController.ZoomIn();
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            cameraController.ZoomOut();
        }

        if (Input.GetKey(KeyCode.W))
        {
            cameraController.MoveUp();
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraController.MoveLeft();
        }

        if (Input.GetKey(KeyCode.S))
        {
            cameraController.MoveDown();
        }

        if (Input.GetKey(KeyCode.D))
        {
            cameraController.MoveRight();
        }
    }

    public void ShowMenu(bool show)
    {
        if (Menu != null && Hud != null)
        {
            Menu.enabled = show;
            Hud.enabled = !show;
            GameOver.enabled = false;

            if ( show )
            {
                CleanUp();
            }
            else
            {
                
            }
        }
    }

    public void CleanUp()
    {
        mMap.CleanUpWorld();
        enemyController.CleanUpEnemies();
    }


    public void Generate()
    {
        mMap.GenerateWorld();
        health = 100;
        enemyController.StartSpawning(new List<EnvironmentTile> { mMap.GetTile(new Vector2Int(0, 0)), mMap.GetTile(new Vector2Int(1, 0)) }, mMap.GetTile(new Vector2Int(10, 10)));
        money.Init(30);
    }

    public void TakeDamage()
    {
        health -= 5;
        if (health <= 0)
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        Menu.enabled = false;
        Hud.enabled = false;
        GameOver.enabled = true;
    }

    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
