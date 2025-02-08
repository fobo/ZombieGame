using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private int damage;
    public float lifetime = 3f;

    void Start()
    {

    }
    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void setBulletDamage(int setDamage){
        damage = setDamage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(damage, gameObject);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        GameController.Instance.ReturnToPool("Bullet", gameObject);
    }
}
