using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private Tower placedTower;

    private void OnTriggerStay2D(Collider2D other)
    {
        // If the placement is already reserved, then return
        if (placedTower != null) return;

        Tower tower = other.GetComponent<Tower>();

        if (tower)
        {
            tower.SetPlacePosition(transform.position);
            placedTower = tower;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (placedTower == null) return;

        placedTower.SetPlacePosition(null);
        placedTower = null;
    }
}
