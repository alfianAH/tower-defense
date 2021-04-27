using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int bulletPower;
    private float bulletSpeed,
        bulletSplashRadius;

    private Enemy targetEnemy;

    private void FixedUpdate()
    {
        if (LevelManager.Instance.IsOver) return;
        
        if(targetEnemy != null)
        {
            if (!targetEnemy.gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                targetEnemy = null;
                return;
            }

            Vector3 targetPosition = targetEnemy.transform.position;
            Vector3 bulletPosition = transform.position;
            
            transform.position = Vector3.MoveTowards(bulletPosition, 
                targetPosition,
                bulletSpeed * Time.fixedDeltaTime);

            Vector3 direction = targetPosition - bulletPosition;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, targetAngle-90f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(targetEnemy == null) return;

        // If the bullet hits target enemy, ...
        if (other.gameObject.Equals(targetEnemy.gameObject))
        {
            gameObject.SetActive(false);

            // Bullet that has splash effect
            if (bulletSplashRadius > 0)
            {
                LevelManager.Instance.ExplodeAt(transform.position, bulletSplashRadius, bulletPower);
            }
            else // Bullet that is single-target
            {
                targetEnemy.ReduceEnemyHealth(bulletPower);
            }

            targetEnemy = null;
        }
    }
    
    /// <summary>
    /// Set bullet's properties
    /// </summary>
    /// <param name="bulletPower">Damage of bullet</param>
    /// <param name="bulletSpeed">Speed of bullet</param>
    /// <param name="bulletSplashRadius">Splash radius of bullet</param>
    public void SetProperties(int bulletPower, float bulletSpeed, float bulletSplashRadius)
    {
        this.bulletPower = bulletPower;
        this.bulletSpeed = bulletSpeed;
        this.bulletSplashRadius = bulletSplashRadius;
    }
    
    /// <summary>
    /// Set target enemy
    /// </summary>
    /// <param name="enemy">Target enemy</param>
    public void SetTargetEnemy(Enemy enemy)
    {
        targetEnemy = enemy;
    }
}
