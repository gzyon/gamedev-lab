using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakBrick : MonoBehaviour
{
    public GameObject prefab;

    public bool broken;
    public UnityEvent onBreakBrick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Player") &&  !broken){
            broken  =  true;
            // assume we have 5 debris per box
            for (int x =  0; x<5; x++){
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
            GetComponent<EdgeCollider2D>().enabled  =  false;
            onBreakBrick.Invoke();
            Debug.Log("Broke brick");
            // Spawner.spawnNewEnemy();
        }
}
}
