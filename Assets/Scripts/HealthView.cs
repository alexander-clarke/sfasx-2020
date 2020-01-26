using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI meshPro;
    [SerializeField] Game game;

    // Start is called before the first frame update
    void Start()
    {
        meshPro = GetComponent<TextMeshProUGUI>();
        game = GetComponentInParent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        int health = game.health;
        meshPro.text = "Health: " + health.ToString();
    }
}
