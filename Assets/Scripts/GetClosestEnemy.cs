using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetClosestEnemy : MonoBehaviour
{
    [SerializeField] private List<Character> charactersInRange;

    public Character closestCharacter
    {
        get
        {
            charactersInRange.RemoveAll(character => character == null);
            return charactersInRange.OrderBy(character => (character.transform.position - transform.position).sqrMagnitude)
                                    .FirstOrDefault();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        charactersInRange = new List<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character != null)
        {
            Debug.Log("Character in range of turret");

            charactersInRange.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();
        if (character != null)
        {
            charactersInRange.Remove(character);
        }
    }
}
