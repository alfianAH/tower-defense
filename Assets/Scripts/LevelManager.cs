using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : SingletonBaseClass<LevelManager>
{
    [Header("Level Properties")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int totalEnemies = 15;

    [Header("User Interface")] 
    [SerializeField] private GameObject panel;
    [SerializeField] private Text statusInfo,
        livesInfo,
        totalEnemiesInfo;

    [Header("Tower Properties")]
    [SerializeField] private Transform towerUiParent;
    [SerializeField] private GameObject towerUiPrefab;
    [SerializeField] private Tower[] towerPrefabs;
    
    [Header("Enemy Properties")]
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private Transform[] enemyPaths;
    [SerializeField] private float spawnDelay = 5f;

    private readonly List<Tower> spawnedTowers = new List<Tower>();
    private readonly List<Enemy> spawnedEnemies = new List<Enemy>();
    private readonly List<Bullet> spawnedBullets = new List<Bullet>();

    private float runningSpawnDelay;
    private int enemyCounter, 
        currentLives;
    public bool IsOver { get; private set; }
    
    private void Start()
    {
        SetCurrentLives(maxLives);
        SetTotalEnemies(totalEnemies);
        InstantiateAllTowerUi();
    }

    private void Update()
    {
        if (IsOver) return;
        
        // Counter is to spawn the enemy in certain delay
        // Time.unscaledDeltaTime is independent delta time, unaffected by anything but itself,
        // so it can be used as a timer
        runningSpawnDelay -= Time.unscaledDeltaTime;

        if (runningSpawnDelay <= 0f)
        {
            SpawnEnemy();
            runningSpawnDelay = spawnDelay;
        }
        
        // Tower are looking for enemy...
        foreach (Tower tower in spawnedTowers)
        {
            tower.CheckNearestEnemy(spawnedEnemies);
            tower.SeekTarget();
            tower.ShootTarget();
        }
        
        // Enemy's movement
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
                    ReduceLives(1);
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
    private void SpawnEnemy()
    {
        SetTotalEnemies(--enemyCounter);
        
        // If enemy counter is below 0, don't spawn anymore
        if (enemyCounter < 0)
        {
            bool isAllEnemyDestroyed = spawnedEnemies.Find(
                e => e.gameObject.activeSelf) == null;

            // If all enemies are destroyed, ...
            if (isAllEnemyDestroyed)
            {
                // Player is win
                SetGameOver(true);
            }
            
            return;
        }
        
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
    
    /// <summary>
    /// Find bullet from the pool
    /// </summary>
    /// <param name="bulletPrefab">Bullet prefab</param>
    /// <returns></returns>
    public Bullet GetBulletFromPool(Bullet bulletPrefab)
    {
        // Find unused bullet in hierarchy
        GameObject newBulletObj = spawnedBullets.Find(
            b => !b.gameObject.activeSelf && b.name.Contains(bulletPrefab.name)
        )?.gameObject;
        
        // Make a new one if can't find it
        if (newBulletObj == null)
        {
            newBulletObj = Instantiate(bulletPrefab.gameObject);
        }

        Bullet newBullet = newBulletObj.GetComponent<Bullet>();
        
        // Add new bullet to the list if it isn't in the list
        if (!spawnedBullets.Contains(newBullet))
        {
            spawnedBullets.Add(newBullet);
        }

        return newBullet;
    }
    
    /// <summary>
    /// Exploosioonn!!!
    /// </summary>
    /// <param name="point">Bullet's position</param>
    /// <param name="radius">Bullet's splash radius</param>
    /// <param name="damage">Bullet's damage</param>
    public void ExplodeAt(Vector2 point, float radius, int damage)
    {
        foreach (Enemy enemy in spawnedEnemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                if (Vector2.Distance(enemy.transform.position, point) <= radius)
                {
                    enemy.ReduceEnemyHealth(damage);
                }
            }
        }
    }
    
    /// <summary>
    /// Reduce lives if enemy reach the last path
    /// </summary>
    /// <param name="lives"></param>
    private void ReduceLives(int lives)
    {
        SetCurrentLives(currentLives - lives);
        AudioPlayer.Instance.PlaySFX("error");

        if (currentLives <= 0)
        {
            SetGameOver(false);
        }
    }
    
    /// <summary>
    /// Set player's current lives
    /// </summary>
    /// <param name="currentLives">Player's current lives</param>
    private void SetCurrentLives(int currentLives)
    {
        this.currentLives = Mathf.Max(currentLives, 0);
        livesInfo.text = $"Lives: {this.currentLives}";
    }
    
    /// <summary>
    /// Set total current enemies
    /// </summary>
    /// <param name="totalEnemies"></param>
    private void SetTotalEnemies(int totalEnemies)
    {
        enemyCounter = totalEnemies;
        totalEnemiesInfo.text = $"Total Enemy: {Mathf.Max(enemyCounter, 0)}";
    }
    
    /// <summary>
    /// Set game over info
    /// </summary>
    /// <param name="isWin">Is player win the game?</param>
    private void SetGameOver(bool isWin)
    {
        IsOver = true;

        statusInfo.text = isWin ? "You Won!" : "You Lose!";
        panel.gameObject.SetActive(true);
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
