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

    /// <summary>
    /// Get tower head's sprite
    /// </summary>
    /// <returns>Tower head's sprite</returns>
    public Sprite GetTowerHeadIcon()
    {
        return towerHead.sprite;
    }
}
