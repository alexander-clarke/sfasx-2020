using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBaseController : MonoBehaviour
{
    [SerializeField] Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponentInParent<Game>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character == null)
        {
            return;
        }

        Debug.Log("Enemy reached base");
        character.StopAllCoroutines();

        collision.gameObject.transform.position -= collision.impulse.normalized*1;

        character.TakeDamage(100);

        game.TakeDamage();
    }
}
