using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] private GameObject target;
    
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = (target.transform.position - myRigidBody.transform.position).normalized;
        myRigidBody.velocity = targetDirection * moveSpeed * Time.deltaTime;
    }
}
