using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float directionX = 1.0f;
    public float directionY = 1.0f;
    public GameObject KillObject;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(directionX * Time.deltaTime, directionY  * Time.deltaTime, 0);
    }

    public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject == KillObject)
           Destroy(KillObject);
        else
        {
            directionX *= -1;
            directionY *= -1;
        }
    }
}
