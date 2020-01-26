using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoCleanUp());
    }

    private IEnumerator DoCleanUp()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Rigidbody>().AddForce((transform.rotation * Vector3.forward).normalized * 10000);
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
