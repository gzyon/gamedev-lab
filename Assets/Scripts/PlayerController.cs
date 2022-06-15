using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{

    private static int score = 0;
    private bool countScoreState = false;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool onGroundState = true;
    private Animator animator;

    [Header("Physics")]
    public float speed;
    public float maxSpeed = 10;
    public float upSpeed = 10;
    public float fallMultiplier = 5f;
    public float gravity = 1f;
    public float linearDrag = 4f;

    [Header("UI")]
    public Text scoreText;
    public Text gameOverText;
    public Button restartButton;

    [Header("Game Elements")]
    public AudioClip impact;
    public AudioClip omona;
    AudioSource jump;
    public Transform enemyLocation;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        jump = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    
    // Here is where you update the physics
    void  FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if(marioBody.velocity.magnitude < maxSpeed){
                marioBody.AddForce(movement * speed);
            }
        }
        if(Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            marioBody.velocity = Vector2.zero;
        }

        if(Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            animator.SetBool("onGround", onGroundState);
            countScoreState = true; //check if gomb is underneath
        }
        if (!onGroundState) {
            marioBody.drag = linearDrag * 0.15f;
            if(marioBody.velocity.y < 0){
                marioBody.gravityScale = gravity * fallMultiplier;
            }else if(marioBody.velocity.y > 0){
                marioBody.gravityScale = gravity * fallMultiplier/2;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        Debug.Log(col.gameObject);
        if(col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle") || col.gameObject.CompareTag("PipeTop") || col.gameObject.CompareTag("QuestionBox")) {
            onGroundState = true;
            animator.SetBool("onGround", onGroundState);
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
        }
        if(col.gameObject.CompareTag("RightWall")){
            Debug.Log("pushLeft");
            // marioBody.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
            marioBody.velocity = Vector2.zero;
        }
        else if(col.gameObject.CompareTag("LeftWall")){
            Debug.Log("pushRight");
            // marioBody.AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);
            marioBody.velocity = Vector2.zero;
        } 
    }


    // Update is called once per frame
    // Here you change the animation and state, not the physics
    void Update()
    {
        animator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        if (Input.GetKeyDown("a") && faceRightState) {                  
            faceRightState = false;
            marioSprite.flipX = true;
            Debug.Log(marioSprite.flipX);
        }

        if(Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }

        if (Mathf.Abs(marioBody.velocity.x) > 1) {
        	animator.SetTrigger("onSkid");
        }

    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("triggered");
        if (other.gameObject.CompareTag("Enemy")) {
            enemyLocation = other.transform;
            if(Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
                // Debug.Log("jump on enemy");
                other.gameObject.SetActive(false);
                Spawner.spawnNewEnemy();
            }
            else {
                gameObject.SetActive(false);
                jump.PlayOneShot(omona, 0.7F);
                marioBody.velocity = Vector2.zero;
                gameOverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                marioBody.GetComponent<PlayerController>().enabled = false;
                animator.enabled = false;
            }
        } 
    }

    void PlayJumpSound()
    {
        Debug.Log("Mario jump");
        jump.PlayOneShot(impact, 0.7F);
    }

    public static void addScore() {
        score++;
    }

    public static void setScore(int newScore) {
        score = newScore;
    }

 }
