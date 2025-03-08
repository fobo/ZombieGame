using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{

    //[SerializeField] float moveSpeed = 1f;
    private GameObject target;
    SpriteRenderer sr;
    Rigidbody2D myRigidBody;


    NavMeshAgent agent;

    private float originalSpeed;
    private Coroutine restoreSpeedRoutine;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        // Find the player object in the scene and assign it as the target
        target = GameObject.FindGameObjectWithTag("Player");

        if (agent != null)
        {
            originalSpeed = agent.speed;
        }


    }



    public void ApplyStoppingPower(float stoppingPower)
    {
        if (agent == null) return;

        float minSpeed = originalSpeed * (1 - stoppingPower);
        Debug.Log("originalSpeed: " + originalSpeed + " stopping power:" + stoppingPower);
        agent.speed = Mathf.Max(minSpeed, agent.speed - (originalSpeed * stoppingPower));

        // If a speed restore coroutine is already running, stop it
        if (restoreSpeedRoutine != null)
        {
            StopCoroutine(restoreSpeedRoutine);
        }

        // Start a coroutine to restore speed over 1 second
        restoreSpeedRoutine = StartCoroutine(RestoreSpeedOverTime(1f));
    }

    private IEnumerator RestoreSpeedOverTime(float duration)
    {
        float elapsedTime = 0f;
        float startSpeed = agent.speed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            agent.speed = Mathf.Lerp(startSpeed, originalSpeed, elapsedTime / duration);
            yield return null;
        }

        agent.speed = originalSpeed; // Ensure speed is fully restored
    }

    private void FixedUpdate()
    {
        if (target == null) { return; }
        agent.SetDestination(target.transform.position);

        if (gameObject.CompareTag("Enemy"))
        {
            FlipEnemySprite();
        }
    }

    private void FlipEnemySprite()
    {
        if (agent.velocity.x > 0)
        {
            sr.flipX = false; // Facing right
        }
        else if (agent.velocity.x < 0)
        {
            sr.flipX = true; // Facing left
        }
    }


    void FlipPlayer()
    {
        // Get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Check if the mouse is left or right of the player

    }



}
