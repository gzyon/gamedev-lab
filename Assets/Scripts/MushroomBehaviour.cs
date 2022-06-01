using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBehaviour : MonoBehaviour
{

    private Vector2 currentPosition;
    private Vector2 nextPosition;
    private float speed  = 5.0f;
    private Vector2 currentDirection = new Vector2(1, 0);
    private Rigidbody2D mushroomBody;

    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        // get the starting position
        // currentPosition = mushroomBody.position;
        mushroomBody.AddForce(Vector2.up  *  10, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = mushroomBody.position;
        nextPosition = currentPosition + speed * currentDirection.normalized * Time.fixedDeltaTime;
        mushroomBody.MovePosition(nextPosition);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            // mushroomBody.velocity = Vector2.zero;
            // Debug.Log(mushroomBody.velocity);
            mushroomBody.gameObject.GetComponent<MushroomBehaviour>().enabled = false;
        }
        else if (other.gameObject.CompareTag("PipeBottom")) {
            // Debug.Log("change direction");
            currentDirection *= -1;
        }
    }
}
