using System;
using System.Collections;
using System.Collections.Generic;
using RPGM.Gameplay;
using RPGM.Core;
using UnityEngine;
using UnityEngine.U2D;

namespace RPGM.Gameplay
{
    /// <summary>
    /// A simple controller for animating a 4 directional sprite using Physics.
    /// </summary>
    public class CharacterController2D : MonoBehaviour
    {
        public float speed = 1;
        public float acceleration = 2;
        public Vector3 nextMoveCommand;
        public Animator animator;
        public bool flipX = false;

        new Rigidbody2D rigidbody2D;
        SpriteRenderer spriteRenderer;
        PixelPerfectCamera pixelPerfectCamera;

        enum State
        {
            Idle, Moving
        }

        State state = State.Idle;
        Vector3 start, end;
        Vector2 currentVelocity;
        float startTime;
        float distance;
        float velocity;

        public Collider2D coll;
        Collider2D touchgingObject = null;
        GameModel model = Schedule.GetModel<GameModel>();

        [System.Serializable]
        public class UsableItems{
            public InventoryItem item;
        }

        public UsableItems[] usableItems;

        void IdleState()
        {
            if (nextMoveCommand != Vector3.zero)
            {
                start = transform.position;
                end = start + nextMoveCommand;
                distance = (end - start).magnitude;
                velocity = 0;
                UpdateAnimator(nextMoveCommand);
                nextMoveCommand = Vector3.zero;
                state = State.Moving;
            }
        }

        void MoveState()
        {
            velocity = Mathf.Clamp01(velocity + Time.deltaTime * acceleration);
            UpdateAnimator(nextMoveCommand);
            rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, nextMoveCommand * speed, ref currentVelocity, acceleration, speed);
            //spriteRenderer.flipX = rigidbody2D.velocity.x >= 0 ? true : false;
        }

        void UpdateAnimator(Vector3 direction)
        {
            if (animator)
            {
                animator.SetInteger("WalkX", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
                animator.SetInteger("WalkY", direction.y < 0 ? 1 : direction.y > 0 ? -1 : 0);
            }
        }

        void Update()
        {
            switch (state)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Moving:
                    MoveState();
                    break;
            }

            if(Input.GetKeyDown(KeyCode.Space)){
                animator.SetBool("isChoping", true);
                Debug.Log(animator.GetBool("isChoping"));
                animator.SetTrigger("ChopingTrigger");
                usingItem();
                animator.SetBool("isChoping", false);
                Debug.Log(animator.GetBool("isChoping"));
            }
            if(Input.GetKeyDown("r")){
                emptyInventory();
            }
        }
        

        void LateUpdate()
        {
            if (pixelPerfectCamera != null)
            {
                transform.position = pixelPerfectCamera.RoundToPixel(transform.position);
            }
        }

        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            pixelPerfectCamera = GameObject.FindObjectOfType<PixelPerfectCamera>();
        }

        void OnCollisionEnter2D(Collision2D other){
            if(other.gameObject.tag == "Enemy"){
                FindObjectOfType<GameManager>().GameOver();
            }
        }
        
        void OnTriggerEnter2D(Collider2D coll){
            if(coll.gameObject.tag == "Enemy"){
                touchgingObject = coll;
            }
        }

        void OnTriggerExit2D(Collider2D coll){
            if(coll.gameObject.tag == "Enemy" && coll == touchgingObject){
                touchgingObject = null;
            }
        }

        Vector3 CalculateThrow(Collider2D enemy){
            float x = transform.position.x - enemy.transform.position.x;
            float y = transform.position.y - enemy.transform.position.y;
            if(x < 0.5 && x > -0.5){
                if(y < 0)
                    return new Vector3(enemy.transform.position.x, transform.position.y - 1, 0);
                else
                    return new Vector3(enemy.transform.position.x, transform.position.y + 1, 0);

            }
            else if (y < 0.5 && y > -0.5) {
                if(x < 0)
                    return new Vector3(transform.position.x - 1, enemy.transform.position.y, 0);
                else
                    return new Vector3(transform.position.x + 1, enemy.transform.position.y, 0);
                    
            }
            return enemy.transform.position;
        }

        void usingItem(){
            var inv = new HashSet<string>(model.InventoryItems);
            if(touchgingObject!=null && model.GetInventoryCount("Schaufel") >= 1){
                foreach (var i in usableItems){
                    if (i.item.name == "Schaufel"){}
                        model.RemoveInventoryItem(i.item, 1);
                }
                
                touchgingObject.transform.position = CalculateThrow(touchgingObject);
            }
        }

        public void emptyInventory(){
            foreach (var i in usableItems){
                model.RemoveInventoryItem(i.item, model.GetInventoryCount(i.item.name));
            }
        }
    }
}