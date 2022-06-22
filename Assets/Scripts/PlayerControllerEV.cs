using System.Collections;
using UnityEngine;


public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    private float fallMultiplier = 6f;
    private float gravity = 1f;
    private float linearDrag = 4f;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;
    private bool isDead = false;
    private bool isADKeyUp = true;
    private bool faceRightState = true; 
    private bool isSpacebarUp = true; 
    private bool onGroundState = true; 
    private bool countScoreState = false;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;
	  
	// other components and interal state
    void Start() {
        marioBody = GetComponent<Rigidbody2D>();
        marioAnimator = GetComponent<Animator>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerStartingMaxSpeed);
        force = gameConstants.playerDefaultForce;
    }

    void Update()
    {
        // Debug.Log(marioBody.velocity.magnitude);
        marioAnimator.SetFloat("xSpeed", marioBody.velocity.magnitude);
        if (Mathf.Abs(marioBody.velocity.x) > 1) {
        	marioAnimator.SetTrigger("onSkid");
        }

        if(Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            isADKeyUp = true;
        }

        else if (Input.GetKeyDown("a")) {
            isADKeyUp = false;
            faceRightState = false;
            marioSprite.flipX = true;
        }

        else if (Input.GetKeyDown("d")) {
            isADKeyUp = false;
            faceRightState = true;
            marioSprite.flipX = false;
        }

        if (Input.GetKeyUp("space")) {
            Debug.Log("not jumping");
            isSpacebarUp = true; 
        } else if (Input.GetKeyDown("space")) {
            Debug.Log("jumping");
            isSpacebarUp = false;
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            //check if a or d is pressed currently
            if (!isADKeyUp)
            {
                float direction = faceRightState ? 1.0f : -1.0f;
                Vector2 movement = new Vector2(force * direction, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
                    marioBody.AddForce(movement);
            }

            if (!isSpacebarUp && onGroundState)
            {
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // part 2
                marioAnimator.SetBool("onGround", onGroundState);
                countScoreState = true; //check if goomba is underneath
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("colliding with " + other.gameObject);
        if (other.gameObject.CompareTag("Ground")) {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) isDead = true;   
    }
}