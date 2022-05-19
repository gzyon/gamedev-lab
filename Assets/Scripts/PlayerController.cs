using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 10;
    private Rigidbody2D mario;
 
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        mario = GetComponent<Rigidbody2D>();   
    }

    void  FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        // dynamic rigidbody
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (mario.velocity.magnitude < maxSpeed)    
                mario.AddForce(movement * speed);
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
                // stop
                mario.velocity = Vector2.zero;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
