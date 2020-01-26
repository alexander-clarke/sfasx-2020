using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Canvas Menu;
    [SerializeField] private Canvas Hud;
    [SerializeField] private Transform CharacterStart;
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private MoneyManager money;

    private RaycastHit[] mRaycastHits;
    //private Character mCharacter;
    private Environment mMap;

    private CameraController cameraController;

    private readonly int NumberOfRaycastHits = 1;

    public int health { get; private set; }

    void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
        mMap = GetComponentInChildren<Environment>();
        //mCharacter = Instantiate(Character, transform);
        ShowMenu(true);
        cameraController = MainCamera.GetComponent<CameraController>();
    }

    private void Update()
    {
        // Check to see if the player has clicked a tile and if they have, try to find a path to that 
        // tile. If we find a path then the character will move along it to the clicked tile. 
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
        //    int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
        //    if( hits > 0)
        //    {
        //        EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
        //        if (tile != null)
        //        {
        //            if (tile.IsAccessible)
        //            {
        //                List<EnvironmentTile> route = mMap.Solve(mCharacter.CurrentPosition, tile);
        //                mCharacter.GoTo(route);
        //            }
        //        }
        //    }
        //}

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

            if( show )
            {
                //mCharacter.transform.position = CharacterStart.position;
                //mCharacter.transform.rotation = CharacterStart.rotation;
                mMap.CleanUpWorld();
                enemyController.CleanUpEnemies();
            }
            else
            {
                
                //mCharacter.transform.position = mMap.Start.Position;
                //mCharacter.transform.rotation = Quaternion.identity;
                //mCharacter.CurrentPosition = mMap.Start;
            }
        }
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
            ShowMenu(true);
        }
    }

    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
