using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    // Singleton
    private static LevelManager instance;

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }

            return instance;
        }
    }

    [SerializeField] private Transform towerUiParent;
    [SerializeField] private GameObject towerUiPrefab;

    [SerializeField] private Tower[] towerPrefabs;
    [SerializeField] private Enemy[] enemyPrefabs;

    [SerializeField] private Transform[] enemyPaths;
    [SerializeField] private float spawnDelay = 5f;

    private List<Tower> spawnedTowers = new List<Tower>();
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    private float runningSpawnDelay;
    
    private void Start()
    {
        InstantiateAllTowerUi();
    }

    private void Update()
    {
        // Counter is to spawn the enemy in certain delay
        // Time.unscaledDeltaTime is independent delta time, unaffected by anything but itself,
        // so it can be used as a timer
        runningSpawnDelay -= Time.unscaledDeltaTime;

        if (runningSpawnDelay <= 0f)
        {
            SpawnEnemy();
            runningSpawnDelay = spawnDelay;
        }

        foreach (Enemy enemy in spawnedEnemies)
        {
            if(!enemy.gameObject.activeSelf) continue;
            
            // If the difference between enemy's position and target position is less than 0.1
            if (Vector2.Distance(enemy.transform.position, enemy.TargetPosition) < 0.1f)
            {
                enemy.SetCurrentPathIndex(enemy.CurrentPathIndex + 1);
                
                // If enemy's current path index less than total of enemy paths, ...
                if (enemy.CurrentPathIndex < enemyPaths.Length)
                {
                    // Set new target position
                    enemy.SetTargetPosition(enemyPaths[enemy.CurrentPathIndex].position);
                }
                else // If enemy reaches the last path, ...
                {
                    // Deactivate the object
                    enemy.gameObject.SetActive(false);
                }
            }
            else
            {
                enemy.MoveToTarget();
            }
        }
    }
    
    /// <summary>
    /// Show all towers in Tower UI
    /// </summary>
    private void InstantiateAllTowerUi()
    {
        foreach (Tower tower in towerPrefabs)
        {
            GameObject newTowerUiObj = Instantiate(towerUiPrefab.gameObject, towerUiParent);

            TowerUi newTowerUi = newTowerUiObj.GetComponent<TowerUi>();
            
            newTowerUi.SetTowerPrefab(tower);
            newTowerUi.transform.name = tower.name;
        }
    }
    
    /// <summary>
    /// Set tower to spawned towers
    /// </summary>
    /// <param name="tower"></param>
    public void RegisterSpawnedTowers(Tower tower)
    {
        spawnedTowers.Add(tower);
    }
    
    /// <summary>
    /// Spawn the enemy
    /// </summary>
    public void SpawnEnemy()
    {
        // Choose random enemy
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        string enemyIndexString = (randomIndex + 1).ToString();

        // Check enemy on hierarchy
        // Return object enemy in game object is not active and contains index in its name
        // Else null
        GameObject newEnemyObj = spawnedEnemies.Find(
                e => !e.gameObject.activeSelf && e.name.Contains(enemyIndexString))
            ?.gameObject;
        
        // If newEnemyObj is null, then make new one
        if (newEnemyObj == null)
        {
            newEnemyObj = Instantiate(enemyPrefabs[randomIndex].gameObject);
        }
        
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
        
        if (!spawnedEnemies.Contains(newEnemy))
        {
            spawnedEnemies.Add(newEnemy);
        }
        
        newEnemy.transform.position = enemyPaths[0].position;
        newEnemy.SetTargetPosition(enemyPaths[1].position);
        newEnemy.SetCurrentPathIndex(1);
        newEnemy.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < enemyPaths.Length - 1; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(enemyPaths[i].position, enemyPaths[i + 1].position);
        }
    }
}
