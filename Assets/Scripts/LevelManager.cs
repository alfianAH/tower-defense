using System.Collections.Generic;
using UnityEngine;

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

    private List<Tower> spawnedTowers = new List<Tower>();

    private void Start()
    {
        InstantiateAllTowerUi();
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
}
