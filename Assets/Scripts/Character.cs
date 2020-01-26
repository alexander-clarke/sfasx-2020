using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float SingleNodeMoveTime = 0.5f;
    [SerializeField] private int health = 100;

    [SerializeField] private Material hitMaterial;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material invisibleMaterial;

    [SerializeField] SkinnedMeshRenderer[] meshRenderer;

    [SerializeField] EnemyController enemyController;

    [SerializeField] GameObject ragdoll;
    [SerializeField] Rigidbody rb;

    public void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public EnvironmentTile CurrentPosition { get; set; }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DoGetHit());
        if (health <= 0)
        {
            StopAllCoroutines();
            
            enemyController.remove(this);

            Instantiate(ragdoll, transform.position, transform.rotation, transform.parent);
            rb.AddForce((transform.rotation * Vector3.forward) * 100);
            ragdoll.GetComponent<Rigidbody>().velocity = rb.velocity;
            ragdoll.GetComponent<Rigidbody>().angularVelocity = rb.angularVelocity;
            Destroy(gameObject);
        }
        
    }

    private IEnumerator DoGetHit()
    {
        foreach (SkinnedMeshRenderer mesh in meshRenderer)
        {
            mesh.material = hitMaterial;
        }
        yield return new WaitForSeconds(1);
        foreach (SkinnedMeshRenderer mesh in meshRenderer)
        {
            mesh.material = normalMaterial;
        }
    }

    private IEnumerator DoMove(Vector3 position, Vector3 destination)
    {
        // Move between the two specified positions over the specified amount of time
        if (position != destination)
        {
            transform.rotation = Quaternion.LookRotation(destination - position, Vector3.up);

            Vector3 p = transform.position;
            float t = 0.0f;

            while (t < SingleNodeMoveTime)
            {
                t += Time.deltaTime;
                p = Vector3.Lerp(position, destination, t / SingleNodeMoveTime);
                transform.position = p;
                yield return null;
            }
        }
    }

    private IEnumerator DoGoTo(List<EnvironmentTile> route)
    {
        // Move through each tile in the given route
        if (route != null)
        {
            
            Vector3 position = transform.position;
            for (int count = 1; count < route.Count; ++count)
            {
                Vector3 next = route[count].Position;
                yield return DoMove(position, next);
                CurrentPosition = route[count];
                position = next;
            }
        }
    }

    public void GoTo(List<EnvironmentTile> route)
    {
        // Clear all coroutines before starting the new route so 
        // that clicks can interupt any current route animation
        StopAllCoroutines();
        StartCoroutine(DoGoTo(route));
    }
}
