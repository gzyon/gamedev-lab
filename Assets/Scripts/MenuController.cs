using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Awake()
    {
      Time.timeScale = 0.0f;
    }

    public void StartButtonClicked() {
        foreach (Transform eachChild in transform) {

            if (eachChild.name != "Score") {
                Debug.Log("Child found. Name: " + eachChild.name );
                eachChild.gameObject.SetActive(false);
                if (eachChild.name == "PowerUps"){
                  eachChild.gameObject.SetActive(true);
                }
                Time.timeScale = 1.0f;
            }
        }
    }

}