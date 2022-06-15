using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    static float groundDistance = -1.0f;
    public static int maxEnemies = 5;

    private static int enemiesSpawned;
    void Start()
    {
        for (int j = 0; j < 2; j++) {
            spawnFromPooler(ObjectType.greenEnemy);
            enemiesSpawned++;
        }
        // for (int j = 0; j < 2; j++) {
        //     spawnFromPooler(ObjectType.gombaEnemy);
        // }
    }

    void Awake() {
        // spawn two gombaEnemy
        Debug.Log("spawner awake");
        // for (int j =  0; j  <  2; j++)
        //     spawnFromPooler(ObjectType.greenEnemy);
        // for (int i = 0; i< 0; i++) 
        //     spawnFromPooler(ObjectType.greenEnemy);    
    }

    public static int getNumEnemies() {
        return enemiesSpawned;
    }


    public static void spawnFromPooler(ObjectType i)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null)
        {
            //set position
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.position = new Vector3(Random.Range(-4.5f, 4.5f), groundDistance + item.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("not enough items in the pool!");
        }
    }

    public static void spawnNewEnemy()
    {

        ObjectType i = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
        spawnFromPooler(i);

    }

}
