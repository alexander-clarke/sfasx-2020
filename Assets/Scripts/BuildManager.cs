using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] List<Tower> towersToSell;
    [SerializeField] List<GameObject> sellingPositions;
    [SerializeField] Camera gameCamera;
    [SerializeField] State state;
    [SerializeField] GameObject buildMenu;

    [SerializeField] MoneyManager moneyManager;

    [SerializeField] Environment environment;

    private RaycastHit[] mRaycastHits;
    private readonly int NumberOfRaycastHits = 3;

    private Tower towerDragging;

    [SerializeField] Transform closedPosition;
    [SerializeField] Transform openPosition;

    [SerializeField] private enum State
    {
        // Shop is open, no items being dragged
        Open,
        // An item is being dragged
        Dragging,
        // Shop is closed
        Closed,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Closed;
        transform.position = closedPosition.position;
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (state == State.Closed)
            {
                ShowShop(true);
            }
            else
            {
                ShowShop(false);
            }
        }
        switch (state)
        {
            case State.Open:
                WaitingToBuy();
                break;
            case State.Dragging:
                Dragging();
                break;
            case State.Closed:
                Closed();
                break;
        }

    }

    private void Closed()
    {
        
    }

    private void Dragging()
    {
        if (towerDragging != null)
        {
            //Debug.Log("Dragging tower: " + towerDragging);
            Ray screenClick = gameCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
            for (int i = 0; i < hits; i++)
            {
                GameObject obj = mRaycastHits[i].transform.gameObject;
                if (obj == buildMenu)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        ResetTowerDrag();
                    }
                    else
                    {
                        OverMenu(mRaycastHits[i]);
                    }
                    break;
                }
                else if (obj != towerDragging.gameObject && obj.transform.parent.gameObject != towerDragging.gameObject)
                {
                    EnvironmentTile tile = obj.GetComponent<EnvironmentTile>();
                    if (tile != null)
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            PlaceTile(tile);
                        }
                        else
                        {
                            OverTile(tile);
                        }
                        break;
                    }
                }
            }

        }
        else
        {
            Debug.Log("In dragging state with no tower to drag");
        }
    }

    private void ResetTowerDrag()
    {
        state = State.Open;
        int i = towersToSell.FindIndex(t => t == towerDragging);
        towerDragging.transform.position = sellingPositions[i].transform.position;
        towerDragging.transform.parent = transform;
        towerDragging = null;
    }

    private void PlaceTile(EnvironmentTile tile)
    {
        state = State.Open;
        
        if (moneyManager.TrySpend(towerDragging.cost))
        {
            EnvironmentTile towerPlaced = environment.SwapTile(towerDragging.tile, tile.position2D, false);

            towerPlaced.GetComponent<Tower>().Activate();
        }
        ResetTowerDrag();

        
    }

    private void OverMenu(RaycastHit hit)
    {
        if (towerDragging == null)
        {
            Debug.LogError("OverMenu called when no object is being dragged");
            return;
        }

        towerDragging.transform.position = hit.point;

        towerDragging.PlacingMode(false);

        towerDragging.transform.parent = transform; // Add offset
    }

    private void OverTile(EnvironmentTile tile)
    {
        if (tile == null)
        {
            Debug.LogError("OverTile called with null tile");
            return;
        }

        if (towerDragging == null)
        {
            Debug.LogError("OverTile called when no object is being dragged");
            return;
        }

        towerDragging.transform.parent = environment.transform;

        towerDragging.transform.position = tile.transform.position;
        towerDragging.transform.rotation = tile.transform.rotation;


        towerDragging.PlacingMode(true);
    }



    private void WaitingToBuy()
    {
        // If player is buying an item
        if (Input.GetMouseButtonDown(0))
        {
            Ray screenClick = gameCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
            for (int i = 0; i < hits; i++)
            {
                GameObject hitObject = mRaycastHits[i].transform.gameObject;

                Tower tower = hitObject.GetComponent<Tower>();
                if (tower == null)
                {
                    continue;
                }

                if (towersToSell.Contains(tower))
                {
                    Debug.Log("Picked up tower");
                    towerDragging = tower;
                    state = State.Dragging;
                    break;
                }
            }
        }
    }

    private IEnumerator OpenShop()
    {
        for (int i = 0; i < 100; i++)
        {
            float smoothUpdateMove = 10 * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, openPosition.position, smoothUpdateMove);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CloseShop()
    {
        for (int i = 0; i < 100; i++)
        {
            float smoothUpdateMove = 10 * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, closedPosition.position, smoothUpdateMove);
            yield return new WaitForEndOfFrame();
        }
    }

    // Smoothly shows/hides shop
    public void ShowShop(bool show)
    {
        StopAllCoroutines();
        if (show)
        {
            state = State.Open;
            StartCoroutine(OpenShop());
        }
        else
        {
            if (state == State.Dragging)
            {
                ResetTowerDrag();
            }
            state = State.Closed;
            StartCoroutine(CloseShop());
        }
    }
}
