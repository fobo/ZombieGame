using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float maxRange = 10f;
    public Transform gunPivot;
    public LayerMask detectionMask;
    public string enemyTag = "Enemy";

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


    private void Update()
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
        currentTarget = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxRange, detectionMask);

        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            Vector2 direction = hit.transform.position - transform.position;
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction.normalized, maxRange, detectionMask);

            if (rayHit && rayHit.collider.CompareTag(enemyTag))
            {
                float dist = Vector2.Distance(transform.position, rayHit.point);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    currentTarget = rayHit.transform;
                }
            }
        }
    }

    private void RotateToFace(Vector2 targetPos)
    {
        Vector2 direction = targetPos - (Vector2)gunPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }


}
