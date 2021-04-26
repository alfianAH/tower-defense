using System;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private SpriteRenderer healthBar,
        healthFill;
    
    public Vector3 TargetPosition { get; private set; }
    public int CurrentPathIndex { get; private set; }

    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        healthFill.size = healthBar.size;
    }
    
    /// <summary>
    /// Move to target position
    /// </summary>
    public void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 
            moveSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// Set enemy's target position
    /// </summary>
    /// <param name="targetPosition">Target Position</param>
    public void SetTargetPosition(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        healthBar.transform.parent = null;
        
        // Change enemy's rotation
        Vector3 distance = TargetPosition - transform.position;
        if (Mathf.Abs(distance.y) > Mathf.Abs(distance.x))
        {
            // If y axis of distance, ...
            transform.rotation = Quaternion.Euler(distance.y > 0 
                ? new Vector3(0f, 0f, 90f) // Rotate upward
                : new Vector3(0f, 0f, -90f)); // Rotate downward
        }
        else
        {
            transform.rotation = quaternion.Euler(distance.x > 0 
                ? new Vector3(0f, 0f, 0f) // Rotate to the right (default)
                : new Vector3(0f, 0f, 180f)); // Rotate to the left
        }

        healthBar.transform.parent = transform;
    }
    
    /// <summary>
    /// Set current path index
    /// </summary>
    /// <param name="currentIndex"></param>
    public void SetCurrentPathIndex(int currentIndex)
    {
        CurrentPathIndex = currentIndex;
    }
}
