using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class GameManager : Singleton<GameManager>
{
	public  Text score;
	private  int playerScore =  0;
	public  delegate  void gameEvent();
    public  static  event  gameEvent OnPlayerDeath;

	// Singleton Pattern
	private  static  GameManager _instance;
	override  public  void  Awake(){
        base.Awake();
        Debug.Log("awake called");
        // other instructions...
    }
	// Getter
	public  static  GameManager Instance
	{
		get { return  _instance; }
	}

	public  void  increaseScore(){
		playerScore += 1;
		score.text =  "SCORE: " + playerScore.ToString();
	}

    public  void  damagePlayer(){
        OnPlayerDeath();
    }
}