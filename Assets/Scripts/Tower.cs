using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] public EnvironmentTile tile;

    //[SerializeField] State state;

    //[SerializeField] private enum State
    //{
    //    inactive,
    //    placing,
    //    active,
    //}


    [SerializeField] LineRenderer lr;
    [SerializeField] GetClosestEnemy getClosestEnemy;
    [SerializeField] Transform shootPoint;
    

    [Header("Tower stats")]
    [SerializeField] float range;
    [SerializeField] float timeBetweenShots;
    [SerializeField] public int cost { get; private set; } = 10;
    [SerializeField] int damage;


    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<EnvironmentTile>();
        getClosestEnemy = GetComponentInChildren<GetClosestEnemy>();
        lr = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator DoShooting()
    {
        while(true)
        {
            yield return new WaitUntil(EnemyInRange);
            FireWeapon(getClosestEnemy.closestCharacter);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void FireWeapon(Character target)
    {
        if (target == null)
        {
            Debug.LogError("Fire Weapon called with no target");
            return;
        }
        lr.SetPositions(new Vector3[2] { shootPoint.position, target.transform.position });
        StartCoroutine(TurnLaserOnAndOff());
        target.TakeDamage(damage);
    }

    private IEnumerator TurnLaserOnAndOff()
    {
        lr.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lr.enabled = false;
    }

    private bool EnemyInRange()
    {
        return getClosestEnemy.closestCharacter != null;
    }

    void StartShooting()
    {
        
    }

    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(DoShooting());
    }

    public void DeActivate()
    {
        StopAllCoroutines();
    }

    public void PlacingMode(bool place)
    {
        if (place)
        {

        }
        else
        {

        }
    }
}
