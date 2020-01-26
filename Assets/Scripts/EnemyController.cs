using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Environment environment;
    [SerializeField] Character enemyPrefab;

    List<EnvironmentTile> spawningTiles;

    EnvironmentTile target;

    List<Character> enemies;

    int howOften = 5;
    int howMany = 4;
    
    [SerializeField] private enum State
    {
        Spawning,
        NotSpawning,
    }

    [SerializeField] private State state;

    // Start is called before the first frame update
    void Start()
    {
        spawningTiles = new List<EnvironmentTile>();
        enemies = new List<Character>();
        environment.MapUpdated += UpdatePaths;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (state) {
            case State.NotSpawning:
                break;
            case State.Spawning:
                break;
        }
    }

    internal void remove(Character character)
    {
        enemies.Remove(character);
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            List<EnvironmentTile> tilesToSpawnOn;

            if (howMany < spawningTiles.Count)
            {
                tilesToSpawnOn = spawningTiles.OrderBy(x => Random.value).Take(howMany).ToList();
            }
            else
            {
                tilesToSpawnOn = spawningTiles;
            }

            foreach (EnvironmentTile tile in tilesToSpawnOn)
            {
                Character character = Instantiate(enemyPrefab, tile.Position, Quaternion.identity, transform);
                enemies.Add(character);
                character.GoTo(environment.Solve(tile, target));
            }

            yield return new WaitForSeconds(howOften);

        }
    }

    public void StartSpawning(List<EnvironmentTile> tiles, EnvironmentTile target)
    {
        StopAllCoroutines();
        spawningTiles = tiles;
        this.target = target;
        StartCoroutine(Spawner());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    public void UpdatePaths(object sender, System.EventArgs e)
    {
        foreach (Character enemy in enemies)
        {
            EnvironmentTile currTile = environment.GetTileBelow(enemy.transform.position);
            if (currTile == null)
            {
                Debug.LogError("No tile below character, can't update paths");
                continue;
            }
            enemy.GoTo(environment.Solve(currTile, target));
        }
    }

    internal void CleanUpEnemies()
    {
        // Destroy all enemies and ragdolls
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject); 
        }
        enemies.Clear();
    }
}
