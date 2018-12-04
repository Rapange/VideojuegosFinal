using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

	// Use this for initialization
	GameObject parentEnemy;
	void Start () {
		parentEnemy = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			parentEnemy.GetComponent<enemy>().attackMode = true;
		}
	}
	
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			parentEnemy.GetComponent<enemy>().attackMode = false;
		}
	}
}
