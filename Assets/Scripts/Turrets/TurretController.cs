using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float maxRange = 15f;
    public Transform gunPivot;
    public LayerMask detectionMask;
    public string enemyTag = "Enemy";
    public LayerMask wall;

    private Transform currentTarget;
    private TurretGun turretGun;

    private void Awake()
    {
        if (gunPivot != null)
        {
            turretGun = gunPivot.GetComponent<TurretGun>();
            if (turretGun == null)
                Debug.LogWarning("TurretGun not found on gunPivot (TurretEye).");
        }

    }



    private List<Transform> enemiesInRange = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Remove(other.transform);
            if (currentTarget == other.transform)
                currentTarget = null;
        }
    }
    private void FixedUpdate()
    {
        ScanForTarget();
        if (currentTarget != null)
        {
            RotateToFace(currentTarget.position);

            turretGun?.Shoot(); // Fire if possible
        }
    }

    private void ScanForTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform closest = null;
        foreach (Transform enemy in enemiesInRange)
        {
            
            Vector2 direction = (Vector2)(enemy.position - transform.position); // find direction of the enemy
            float distanceToEnemy = direction.magnitude; // get distance
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distanceToEnemy, wall);
            if (hit.collider != null && hit.transform == enemy)
            { // if the ray hits something and is an enemy
                if (distanceToEnemy < closestDistance)
                { 
                    closestDistance = distanceToEnemy;
                    closest = enemy;
                }
            }
        }
        currentTarget = closest;
    }
    private void RotateToFace(Vector2 targetPos)
    {
        Vector2 direction = targetPos - (Vector2)gunPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }


}
