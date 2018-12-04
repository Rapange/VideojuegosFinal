using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	// Use this for initialization
	public float speed;
	Vector3 iniPosition;
	void Start () {
		iniPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(iniPosition,transform.position) > 7){
			Destroy(gameObject);
		}
	}
	
	void OnCollisionEnter(Collision col){
		//print("enter2");
		if(col.gameObject.tag == "Enemy"){
			col.gameObject.GetComponent<AudioSource>().Play();
			col.gameObject.GetComponent<enemy>().health -= 25;
		}
		if(col.gameObject.tag == "Player"){
			col.gameObject.GetComponent<AudioSource>().Play();
			col.gameObject.GetComponent<player>().health -= 25;
		}
		Destroy(gameObject);
	}
}
