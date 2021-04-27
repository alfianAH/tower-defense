using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUi : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image towerIcon;
    [SerializeField] private Tower towerPrefab;
    
    private Tower currentSpawnedTower;
    
    /// <summary>
    /// Set tower prefab
    /// </summary>
    /// <param name="tower"></param>
    public void SetTowerPrefab(Tower tower)
    {
        towerPrefab = tower;
        towerIcon.sprite = tower.GetTowerHeadIcon();
    }

    /// <summary>
    /// Called on begin drag tower UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject newTowerObj = Instantiate(towerPrefab.gameObject);

        currentSpawnedTower = newTowerObj.GetComponent<Tower>();
        currentSpawnedTower.ToggleOrderInLayer(true);
    }
    
    /// <summary>
    /// Called on drag tower UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Camera mainCamera = Camera.main;
        Vector3 mousePosition = Input.mousePosition;
        
        if (mainCamera != null)
        {
            mousePosition.z = -mainCamera.transform.position.z;

            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            currentSpawnedTower.transform.position = targetPosition;
        }
    }
    
    /// <summary>
    /// Called on end drag tower UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // If current spawned tower's place position is null, ...
        if (currentSpawnedTower.PlacePosition == null)
        {
            // Destroy the instantiated one
            Destroy(currentSpawnedTower.gameObject);
        }
        else // Else, ...
        {
            // Lock the placement
            currentSpawnedTower.LockPlacement();
            // Set the current spawned tower's order in layer to 0
            currentSpawnedTower.ToggleOrderInLayer(false);
            
            // Register the spawned towers to level manager
            LevelManager.Instance.RegisterSpawnedTowers(currentSpawnedTower);
            // Set current spawned tower to null 
            currentSpawnedTower = null;
        }
    }
}
