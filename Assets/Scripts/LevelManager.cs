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
}
