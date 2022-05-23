using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D mario;
    private int score = 0;
    private bool countScoreState = false;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool onGroundState = true;

    public float speed;
    public float maxSpeed = 10;
    public float upSpeed = 10;
    public Transform enemyLocation;
    public Text scoreText;
    public Text gameOverText;
    
 
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        mario = GetComponent<Rigidbody2D>();   
        marioSprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        // mario move
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (mario.velocity.magnitude < maxSpeed)    
                mario.AddForce(movement * speed);
        }

        // mario stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            mario.velocity = Vector2.zero;
        }

        // mario jump
        if (Input.GetKeyDown("space") && onGroundState) {
            // mario dont flip while jumping
            mario.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            countScoreState = true; //check if Goomba is underneath
        }

        // mario flip
        if (Input.GetKeyDown("a") && faceRightState) {
            faceRightState = false;
            marioSprite.flipX = true;
        }
        if (Input.GetKeyDown("d") && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("mario collided");
        if (col.gameObject.CompareTag("Ground")) {
            Debug.Log("Mario is on the floor");
            onGroundState = true;
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with Goomba!");
            gameOverText.gameObject.SetActive(true);
        }
    }
}
