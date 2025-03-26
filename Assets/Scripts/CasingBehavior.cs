using UnityEngine;
using System.Collections;

public class CasingBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    public void Eject(float gunAngle)
    {
        StartCoroutine(AnimateEjection(gunAngle));
    }

    private IEnumerator AnimateEjection(float gunAngle)
    {
        float ejectDuration = 0.5f;
        float elapsedTime = 0f;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        Vector2 baseEjectDirection = Vector2.right;
        Vector2 ejectDirection = Quaternion.Euler(0, 0, gunAngle - 90f + Random.Range(-15f, 15f)) * baseEjectDirection;
        float ejectSpeed = Random.Range(2f, 4f);
        float spinSpeed = Random.Range(-300f, 300f);

        LayerMask wallLayer = LayerMask.GetMask("Walls");

        while (elapsedTime < ejectDuration)
        {
            elapsedTime += Time.deltaTime;

            Vector2 currentPos = transform.position;
            float checkDistance = ejectSpeed * Time.deltaTime * 1.1f;
            Vector2 movement = ejectDirection * ejectSpeed * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(currentPos, ejectDirection, checkDistance, wallLayer);
            if (hit.collider != null)
            {
                transform.position = hit.point - ejectDirection * 0.01f;
                break;
            }

            transform.position = currentPos + movement;
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

            yield return null;
        }

        rb.isKinematic = false;
        rb.velocity = ejectDirection * 1.5f;
        rb.angularVelocity = spinSpeed / 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            // Immediately stop casing on wall hit
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true; // optional: freeze it permanently
        }
    }
}
