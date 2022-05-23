using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public float speed;
    public float upSpeed;
    private Rigidbody2D marioBody;
    public float maxSpeed = 10;
    public float pushForce = 10;

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool onGroundState = true;

    public Transform enemyLocation;
    public TMPro.TextMeshProUGUI scoreText;

    private int score = 0;
    private bool countScoreState = false;

    public Sprite[] spriteArray;

    public AudioClip impact;
    public AudioClip omona;
    AudioSource die;
    AudioSource jump;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        jump = GetComponent<AudioSource>();
        
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
            countScoreState = true; //check if gomb is underneath
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Ground")) {
            onGroundState = true;
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
        }
        if(col.gameObject.CompareTag("RightWall")){
            Debug.Log("pushLeft");
            marioBody.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
        }
        else if(col.gameObject.CompareTag("LeftWall")){
            Debug.Log("pushRight");
            marioBody.AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);
        }
    }


    // Update is called once per frame
    // Here you change the animation and state, not the physics
    void Update()
    {
        if(Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if(Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }

        // todo: implement jump animation
        if(Input.GetKeyDown("space") && !onGroundState){
            jump.PlayOneShot(impact, 0.7F);
            marioSprite.sprite = spriteArray[0];
        }
        if(onGroundState){
            marioSprite.sprite = spriteArray[1];
        }

        if (Input.GetKeyDown("a") || Input.GetKeyDown("d")) {
            if (onGroundState) {
                for (int i = 2; i < 5; i++)
                {
                    marioSprite.sprite = spriteArray[i];
                    
                }

            }
        }
        
        //

        if(!onGroundState && countScoreState){
            if(Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f){
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
  {
      if (other.gameObject.CompareTag("Enemy"))
      {
          Debug.Log("Collided with Gomba!");
          jump.PlayOneShot(omona, 0.7F);
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          
      }
  }
}
