using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Spawner : MonoBehaviour
{


    [SerializeField] GameObject spawnTarget; //for now, we can insert the specific type of object we want to spawn.
    [SerializeField] float intervalTime; //How often the spawner spawns the object.
    float currTime;

    // Start is called before the first frame update
    void Start()
    {
        currTime = 0f; 
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime >= intervalTime){
            Debug.Log("Making thing");
            spawnGameObject();
            currTime -= intervalTime;//reset the time back down
        }

    }

    void spawnGameObject(){
        Instantiate(spawnTarget, transform.position, transform.rotation);
    }
}
