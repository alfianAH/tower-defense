using UnityEngine;
using UnityEngine.UI;

public class TowerUi : MonoBehaviour
{
    [SerializeField] private Image towerIcon;

    private Tower towerPrefab;
    
    /// <summary>
    /// Set tower prefab
    /// </summary>
    /// <param name="tower"></param>
    public void SetTowerPrefab(Tower tower)
    {
        towerPrefab = tower;
        towerIcon.sprite = tower.GetTowerHeadIcon();
    }
}
