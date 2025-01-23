using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Die()
    {
        Debug.Log($"Enemy {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
