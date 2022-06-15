using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour
{
    private float originalX;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private Animator animator;
    private bool moveEnemy = true;
    public  GameConstants gameConstants;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log("enemy moving");
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
        GameManager.OnPlayerDeath  +=  EnemyRejoice;
    }

    void  ComputeVelocity()
	{
			velocity  =  new  Vector2((moveRight) *  gameConstants.maxOffset  /  gameConstants.enemyPatroltime, 0);
	}
  
	void  MoveGomba()
	{
		enemyBody.MovePosition(enemyBody.position  +  velocity  *  Time.fixedDeltaTime);
	}


    void OnTriggerEnter2D(Collider2D other) {
        // if (other.gameObject.CompareTag("Player")) {
        //     Debug.Log("Goomba collided into mario");
        //     moveEnemy = false;
        //     animator.SetBool("marioDead", true);
        //     // enemyBody.GetComponent<EnemyController>().enabled = false;
        // }
        // check if it collides with Mario
		if (other.gameObject.tag  ==  "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  0.75f){
				KillSelf();
			}
			else{
				// hurt player, implement later
                // animator.SetBool("marioDead", true);
                CentralManager.centralManagerInstance.damagePlayer();
                // EnemyRejoice();
			}
		}
    }

    void  KillSelf(){
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield  break;
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
        if (moveEnemy){
            if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset) {
                // move gomba
                MoveGomba();
            }
            else {
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                MoveGomba();
            }
        }
        else{
            enemyBody.velocity = Vector3.zero;
        }
        
    }

    // animation when player is dead
    void  EnemyRejoice(){
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        // ...
        moveEnemy = false;
        Debug.Log("enemy stop moving " + enemyBody.velocity);
    }
    
}