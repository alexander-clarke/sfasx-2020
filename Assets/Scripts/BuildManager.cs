using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] List<GameObject> towersToSell;
    [SerializeField] Game game;
    [SerializeField] State state;

    private RaycastHit[] mRaycastHits;
    private readonly int NumberOfRaycastHits = 2;

    private GameObject towerDragging;

    [SerializeField] private enum State
    {
        Open,
        Dragging,
        Closed,
    }

    // Start is called before the first frame update
    void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Open:
                OpenShop();
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
        throw new NotImplementedException();
    }

    private void Dragging()
    {
        if (towerDragging != null)
        {
            if (Input.GetMouseButton(0))
            {
                Ray screenClick = game.MainCamera.ScreenPointToRay(Input.mousePosition);
                int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
                if (hits > 0)
                {
                    if (hits > 1)
                    {
                        EnvironmentTile tile = mRaycastHits[1].transform.GetComponent<EnvironmentTile>();
                        EnvironmentTile tile1 = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();



                        // Check if we're over a tile or over the build menu
                        if (tile != null)
                        {
                            towerDragging.transform.position = tile.transform.position;
                        }
                        if (tile1 != null && tile1.gameObject == towerDragging)
                        {
                            towerDragging.transform.position = tile1.transform.position;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();

                        // Check if we're over a tile or over the build menu
                        if (tile != null)
                        {
                            towerDragging.transform.position = tile.transform.position;
                        }
                    }
                }


            }
            else
            {
                state = State.Open;
                // Put tower into gameworld
            }
        }
        else
        {
            Debug.Log("Shouldn't be here");
        }
    }

    private void OpenShop()
    {
        // If player is buying an item
        if (Input.GetMouseButtonDown(0))
        {
            Ray screenClick = game.MainCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
            if (hits > 0)
            {
                GameObject hitObject = mRaycastHits[0].transform.gameObject;

                if (towersToSell.Contains(hitObject)) {
                    towerDragging = hitObject;
                    state = State.Dragging;
                }
            }
        }
    }
}
