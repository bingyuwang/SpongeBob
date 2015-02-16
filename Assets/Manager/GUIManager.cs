using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public GUIText gameTitle, gameOverText,GameInstruction;
	// Use this for initialization
	void Start () {
		gameOverText.enabled = false;
		GameManager.GameStart += GameStart;
		GameManager.GameOver += GameOver;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			GameManager.TriggerGameStart();			
		}
	}
	void GameStart(){
		gameTitle.enabled = false;
		GameInstruction.enabled = false;
		gameOverText.enabled = false;
	}
	void GameOver(){
		gameOverText.enabled = true;
		gameTitle.enabled = true;
		GameInstruction.enabled = true;
	}
}
