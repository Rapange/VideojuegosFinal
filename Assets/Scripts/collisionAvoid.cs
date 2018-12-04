using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionAvoid : MonoBehaviour {

	// Use this for initialization
	GameObject parentEnemy;
	int dir;
	void Start () {
		parentEnemy = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerStay(Collider col){
		if(col.tag != "Player" && !parentEnemy.GetComponent<enemy>().attackMode)
			parentEnemy.GetComponent<enemy>().Rotate(dir * 100);
		/*Vector3 velocity = parentEnemy.GetComponent<enemy>().velocity * 100.0f;
		parentEnemy.GetComponent<enemy>().velocity += new Vector3(-velocity.z,0,velocity.x);*/
		//print(col.gameObject.tag);
	}
	
	void OnTriggerEnter(Collider col){
		if(col.tag != "Player" && !parentEnemy.GetComponent<enemy>().attackMode){
			dir = Random.Range(0,1);
			if(dir == 0) dir = -1;
			parentEnemy.GetComponent<enemy>().Rotate(dir * 100);
		}
	}
}
