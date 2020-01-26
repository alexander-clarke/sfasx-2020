using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] public EnvironmentTile tile;
    [SerializeField] LineRenderer lr;
    [SerializeField] GetClosestEnemy getClosestEnemy;
    [SerializeField] Transform shootPoint;
    

    [Header("Tower stats")]
    [SerializeField] float range;
    [SerializeField] float timeBetweenShots;
    [SerializeField] public int cost { get; private set; } = 10;
    [SerializeField] int damage;


    void Start()
    {
        tile = GetComponent<EnvironmentTile>();
        getClosestEnemy = GetComponentInChildren<GetClosestEnemy>();
        lr = GetComponentInChildren<LineRenderer>();
    }

    private IEnumerator DoShooting()
    {
        while(true)
        {
            yield return new WaitUntil(() => getClosestEnemy.closestCharacter != null);
            if (FireWeapon(getClosestEnemy.closestCharacter))
            {
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }

    private bool FireWeapon(Character target)
    {
        if (target == null)
        {
            Debug.LogError("Fire Weapon called with no target");
            return false;
        }
        lr.SetPositions(new Vector3[2] { shootPoint.position, target.transform.position });
        StartCoroutine(TurnLaserOnAndOff());
        target.TakeDamage(damage);
        return true;
    }

    private IEnumerator TurnLaserOnAndOff()
    {
        lr.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lr.enabled = false;
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
            // Planned shader change to indicate the placing mode
        }
        else
        {

        }
    }
}
