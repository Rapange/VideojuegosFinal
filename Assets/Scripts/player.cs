using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour {

	// Use this for initialization
	float x,y,z;
	Vector3 mousePos, lastMousePos, lastEulerAngles;
	Transform mainCamera;
	bool isJumping;
	public float speed, rSpeed, jSpeed, health;
	public Transform bulletPrefab, bulletSpawn, playerSpawn;
	public Text textHealth;
	GameObject master;
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		lastMousePos  = mousePos = Input.mousePosition;
		isJumping = false;
		master = GameObject.Find("Master");
		
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject == null) return;
		if(health <= 0)
		{
			master.GetComponent<master>().enemyKills++;
			health = 100;
			transform.position = playerSpawn.position;
		}
		
		textHealth.text = "Health: " + health.ToString();
		
		x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
		mousePos = Input.mousePosition;
		
		lastEulerAngles = mainCamera.eulerAngles;
		mainCamera.eulerAngles = new Vector3(0,mainCamera.eulerAngles.y,0);

		
		transform.Rotate(0, (mousePos.x-lastMousePos.x)*rSpeed *Time.deltaTime ,0);
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,0);
		
		transform.Translate(x,0,z);
		
		mainCamera.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		
		mainCamera.eulerAngles = lastEulerAngles;
		
		mainCamera.Rotate( (lastMousePos.y-mousePos.y) * rSpeed *Time.deltaTime, (mousePos.x-lastMousePos.x)*rSpeed *Time.deltaTime, 0);
		mainCamera.eulerAngles = new Vector3(mainCamera.eulerAngles.x, /*mainCamera.eulerAngles.y*/ transform.eulerAngles.y,0);
		
		
		lastMousePos = mousePos;
		
		if(GetComponent<Rigidbody>().velocity.y == 0 && isJumping)
			isJumping = false;
		
		if(Input.GetKeyDown("space") && !isJumping){
			GetComponent<Rigidbody>().velocity = (Vector3.up*jSpeed);
			isJumping = true;
			//Debug.Log("print");
		}
		
		if(Input.GetMouseButtonDown(0)){
			Fire();
		}
	}
	
	void Fire(){
		var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		//bullet.transform.eulerAngles = transform.eulerAngles;
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10f ;
		
		//print(transform.forward);
	}
}
