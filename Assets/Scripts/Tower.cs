using System.Collections.Generic;
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
        bulletSplashRadius;
    [SerializeField] private Bullet bulletPrefab;

    private float runningShootDelay;
    private Enemy targetEnemy;
    private Quaternion targetRotation;
    
    public Vector2? PlacePosition { get; private set; }
    
    /// <summary>
    /// Get tower head's sprite
    /// </summary>
    /// <returns>Tower head's sprite</returns>
    public Sprite GetTowerHeadIcon()
    {
        return towerHead.sprite;
    }
    
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
    /// Check nearest enemy
    /// </summary>
    /// <param name="enemies"></param>
    public void CheckNearestEnemy(List<Enemy> enemies)
    {
        // If there is target, ...
        if (targetEnemy != null)
        {
            // If target is not active or target is out of shooting distance/range, ...
            if (!targetEnemy.gameObject.activeSelf ||
                Vector3.Distance(transform.position, targetEnemy.transform.position) > shootDistance)
            {
                // Set targetEnemy to null
                targetEnemy = null;
            }
            else return;
        }

        float nearestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;
        
        // Check nearest distance and enemy in enemies
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if(distance > shootDistance) continue;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        targetEnemy = nearestEnemy;
    }
    
    /// <summary>
    /// Shoot target enemy
    /// </summary>
    public void ShootTarget()
    {
        if(targetEnemy == null) return;

        runningShootDelay -= Time.unscaledDeltaTime;
        
        if (runningShootDelay <= 0f)
        {
            bool headHasAimed = Mathf.Abs(
                towerHead.transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z) < 10f;
            
            if(!headHasAimed) return;
            
            // Prepare the bullet
            Bullet bullet = LevelManager.Instance.GetBulletFromPool(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.SetProperties(shootPower, bulletSpeed, bulletSplashRadius);
            bullet.SetTargetEnemy(targetEnemy);
            bullet.gameObject.SetActive(true);

            runningShootDelay = shootDelay;
        }
    }
    
    /// <summary>
    /// Rotate tower head to the target enemy
    /// </summary>
    public void SeekTarget()
    {
        if(targetEnemy == null) return;

        Vector3 direction = targetEnemy.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, targetAngle - 90f));
        
        towerHead.transform.rotation = Quaternion.RotateTowards(
            towerHead.transform.rotation,
            targetRotation,
            Time.deltaTime * 180f);
    }
}
