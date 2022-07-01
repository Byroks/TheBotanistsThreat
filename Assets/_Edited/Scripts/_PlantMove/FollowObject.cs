using System.Collections;
using UnityEngine;
using Pathfinding;

public class FollowObject : MonoBehaviour
{
    public GameObject DestroyObject;
    public AIPath aiPath;
    public float chaseDistance = 5f;

    void awake(){
        aiPath.isStopped = true;
    }
    
    // Update is called once per frame
    void Update()
    {        
        //player.transform.position.x = 
        /*if(FollowObject!=null){
            // get size of plant --> calculate center point for player collision
            float halfPlantWidthX = transform.localScale.x / 2;
            float halfPlantWidthY = transform.localScale.y / 2;

            Vector2 followPos = FollowObject.transform.position;
            Vector2 targetLocation = new Vector2(followPos.x - halfPlantWidthX, followPos.y + halfPlantWidthY);

            transform.position = Vector2.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
        }*/

        
        if(aiPath.desiredVelocity.x >= 0.01f) {
            this.transform.localScale = new Vector3 (-1f, 1f, 1f);
        }
        else if(aiPath.desiredVelocity.x <= -0.01f) {
            transform.localScale = new Vector3 (1f, 1f, 1f);
        }

        if(aiPath.remainingDistance > chaseDistance*2 && !aiPath.isStopped){
            aiPath.isStopped = true;
        }
        else if(aiPath.remainingDistance < chaseDistance && aiPath.isStopped){
            aiPath.isStopped = false;
        }
        // else if(aiPath.isStopped){
        //     aiPath.isStopped = false;
        // }
    }

    public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.SetActive(false);
        }

    }
}
