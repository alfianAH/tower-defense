using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Component")] 
    [SerializeField] private SpriteRenderer towerPlace;
    [SerializeField] private SpriteRenderer towerHead;

    [Header("Tower Properties")] 
    [SerializeField] private int shootPower = 1;
    [SerializeField] private float shootDistance = 1f,
        shootDelay = 5f,
        bulletSpeed = 1f,
        bulletSplashRadius = 0f;
    
    public Vector2? PlacePosition { get; private set; }
    
    /// <summary>
    /// Set place position
    /// </summary>
    /// <param name="newPosition">New position. Can be null</param>
    public void SetPlacePosition(Vector2? newPosition)
    {
        PlacePosition = newPosition;
    }
    
    /// <summary>
    /// Lock tower placement
    /// </summary>
    public void LockPlacement()
    {
        if (PlacePosition != null) 
            transform.position = (Vector2) PlacePosition;
    }
    
    /// <summary>
    /// Set tower's order in layer
    /// </summary>
    /// <param name="toFront">Is tower to front</param>
    public void ToggleOrderInLayer(bool toFront)
    {
        // If front, order in layer is 2, else is 0
        int orderInLayer = toFront ? 2 : 0;
        towerPlace.sortingOrder = orderInLayer;
        towerHead.sortingOrder = orderInLayer;
    }
    
    /// <summary>
    /// Get tower head's sprite
    /// </summary>
    /// <returns>Tower head's sprite</returns>
    public Sprite GetTowerHeadIcon()
    {
        return towerHead.sprite;
    }
}
