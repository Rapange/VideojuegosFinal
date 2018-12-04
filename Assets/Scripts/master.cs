using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class master : MonoBehaviour {

	// Use this for initialization
	public Text score;
	public int playerKills, enemyKills;
	void Start () {
		playerKills = enemyKills = 0;
	}
	
	// Update is called once per frame
	void Update () {
		score.text = "Score: " + playerKills.ToString() + " - " + enemyKills.ToString();
	}
}
