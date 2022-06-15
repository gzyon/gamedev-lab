using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 1.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private Animator animator;
    private bool moveEnemy = true;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log("enemy moving");
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity(){
        velocity = new Vector2((moveRight)*maxOffset / enemyPatroltime, 0);
    }
    void MoveGomba(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Goomba collided into mario");
            moveEnemy = false;
            animator.SetBool("marioDead", true);
            // enemyBody.GetComponent<EnemyController>().enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PipeBottom")) {
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
    }

    void Update()
    {
        if (moveEnemy) {
            if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset) {
                // move gomba
                MoveGomba();
            }
            else {
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                MoveGomba();
            }
        } else {
            enemyBody.velocity.Set(0, 0);
            Debug.Log("enemy stop moving " + enemyBody.velocity);
        }
    }
    
}