using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class enemy : MonoBehaviour {

	// Use this for initialization
	public float speed, wanderDist, wanderRad, wanderAngle, angleChange;
	public float fireSpeed, health;
	public Vector3 lastPos, velocity, steering, lastVelocity;
	public bool attackMode;
	public Transform bulletPrefab, bulletSpawn, enemySpawn;
	GameObject master;
	int dir, cases;
	GameObject player;
	NeuralNetwork nn, nnMove;
	float timer = 0;
	void Start () {
		
		lastPos = transform.position;
		lastVelocity = velocity = new Vector3(speed,0,0);
		player = GameObject.FindGameObjectWithTag("Player");
		master = GameObject.Find("Master");
		//Inputs
		//degrees
		//0.0 left - 1.0 right enemy
		
		
		// Outputs
		// 0.0 -> Turn Left
		// 0.5 -> Shoot
		// 1.0 -> Turn Right
		
		nn = new NeuralNetwork(3, new int[]{2,4,1},0.1);
		
		cases = 48+4;		
		
		double[][] inputs = new double[cases][];
		double[][] outputs = new double[cases][];
		int i = 0;
		
		using(TextReader reader = File.OpenText("Assets/Scripts/shootData.txt"))
		{
			string data;
			while( (data = reader.ReadLine()) != null && data != "" ){
				string[] iio = data.Split(' ');
				inputs[i] = new double[]{double.Parse(iio[0]), double.Parse(iio[1])}; 
				outputs[i] = new double[]{double.Parse(iio[2])};

				i++;
			}	
			
		}
		
		nn.Train(cases,inputs, outputs, new double[]{180.0,1.0});
		
		/*nn.neurons[0].weights[0] = 2.24099446534939;
		nn.neurons[0].weights[1] = 14.1352602510659;
		nn.neurons[0].weights[2] = -7.60894994677473;
		nn.neurons[0].weights[3] = 9.16004243467351;
		
		nn.neurons[1].weights[0] = 2.33910241390732;
		nn.neurons[1].weights[1] = -1.14793573280798;
		nn.neurons[1].weights[2] = -4.96602626979668;
		nn.neurons[1].weights[3] = -12.4437975255316;
		
		nn.neurons[2].weights[0] = -3.30698839553645;
		nn.neurons[3].weights[0] = 9.78709523581914;
		nn.neurons[4].weights[0] = 11.4841994939091;
		nn.neurons[5].weights[0] = -16.8014032000518;*/
		
		/*print(String.Format("0-0: {0}",nn.neurons[0].weights[0]));
		print(String.Format("0-1: {0}",nn.neurons[0].weights[1]));
		print(String.Format("0-2: {0}",nn.neurons[0].weights[2]));
		print(String.Format("0-3: {0}",nn.neurons[0].weights[3]));
		
		print(String.Format("1-0: {0}",nn.neurons[1].weights[0]));
		print(String.Format("1-1: {0}",nn.neurons[1].weights[1]));
		print(String.Format("1-2: {0}",nn.neurons[1].weights[2]));
		print(String.Format("1-3: {0}",nn.neurons[1].weights[3]));
		
		print(String.Format("2-0: {0}",nn.neurons[2].weights[0]));
		
		print(String.Format("3-0: {0}",nn.neurons[3].weights[0]));
		
		print(String.Format("4-0: {0}",nn.neurons[4].weights[0]));
		
		print(String.Format("5-0: {0}",nn.neurons[5].weights[0]));*/
		
		cases = 672;
		nnMove = new NeuralNetwork(3, new int[]{3,6,4},0.1);
		inputs = new double[cases][];
		outputs = new double[cases][];
		i = 0;
		
		/*using(TextReader reader = File.OpenText("Assets/Scripts/moveData.txt"))
		{
			string data;
			while( (data = reader.ReadLine()) != null && data != "" ){
				string[] iio = data.Split(' ');
				inputs[i] = new double[]{double.Parse(iio[0]), double.Parse(iio[1]), double.Parse(iio[2])}; 
				outputs[i] = new double[]{double.Parse(iio[3]),double.Parse(iio[4]),double.Parse(iio[5]),double.Parse(iio[6])};
				
				i++;
			}	
			
		}*/
		
		//nnMove.Train(cases,inputs,outputs,new double[]{180.0,1.0,7.0});
		
		
		nnMove.neurons[0].weights[0] = 0.114657283630446;
		nnMove.neurons[0].weights[1] = -0.107851242652959;
		nnMove.neurons[0].weights[2] = -12.4898618001156;
		nnMove.neurons[0].weights[3] = -36.1413132259214;
		nnMove.neurons[0].weights[4] = -0.137461142828297;
		nnMove.neurons[0].weights[5] = 18.8030015102722;
		
		nnMove.neurons[1].weights[0] = 0.608153048144759;
		nnMove.neurons[1].weights[1] = 1.24285973099438;
		nnMove.neurons[1].weights[2] = -8.69602656034807;
		nnMove.neurons[1].weights[3] = 3.46612021546023;
		nnMove.neurons[1].weights[4] = 0.867627871265702;
		nnMove.neurons[1].weights[5] = -25.2174822321421;
		
		nnMove.neurons[2].weights[0] = -2.36765978758248;
		nnMove.neurons[2].weights[1] = 2.22659378949603;
		nnMove.neurons[2].weights[2] = -1.02651608325125;
		nnMove.neurons[2].weights[3] = -1.11686902071726;
		nnMove.neurons[2].weights[4] = -2.37476119802058;
		nnMove.neurons[2].weights[5] = -1.03249450907426;
		
		nnMove.neurons[3].weights[0] = -74.79695198887;
		nnMove.neurons[3].weights[1] = 40.1383893909188;
		nnMove.neurons[3].weights[2] = -4.81962136752055;
		nnMove.neurons[3].weights[3] = 4.79292884034483;

		nnMove.neurons[4].weights[0] = 39.0184402250097;
		nnMove.neurons[4].weights[1] = -41.735153476062;
		nnMove.neurons[4].weights[2] = -10.2572367495746;
		nnMove.neurons[4].weights[3] = 10.2576968720378;
		
		nnMove.neurons[5].weights[0] = -28.0263129446159;
		nnMove.neurons[5].weights[1] = 9.65837869187608;
		nnMove.neurons[5].weights[2] = -37.4486705768241;
		nnMove.neurons[5].weights[3] = 37.4475186432657;
		
		nnMove.neurons[6].weights[0] = 1.15958316835611;
		nnMove.neurons[6].weights[1] = -0.0309156537317375;
		nnMove.neurons[6].weights[2] = 20.5588289635302;
		nnMove.neurons[6].weights[3] = -20.560965141323;
		
		nnMove.neurons[7].weights[0] = -69.6432822359652;
		nnMove.neurons[7].weights[1] = 42.3398094388351;
		nnMove.neurons[7].weights[2] = -5.33802292491268;
		nnMove.neurons[7].weights[3] = 5.36464537592003;
		
		nnMove.neurons[8].weights[0] = -11.5051572203787;
		nnMove.neurons[8].weights[1] = 5.68716446070077;
		nnMove.neurons[8].weights[2] = 25.6110008130518;
		nnMove.neurons[8].weights[3] = -25.6107764026332;
		
		/*print(String.Format("0-0: {0}",nnMove.neurons[0].weights[0]));
		print(String.Format("0-1: {0}",nnMove.neurons[0].weights[1]));
		print(String.Format("0-2: {0}",nnMove.neurons[0].weights[2]));
		print(String.Format("0-3: {0}",nnMove.neurons[0].weights[3]));
		print(String.Format("0-4: {0}",nnMove.neurons[0].weights[4]));
		print(String.Format("0-5: {0}",nnMove.neurons[0].weights[5]));
		
		print(String.Format("1-0: {0}",nnMove.neurons[1].weights[0]));
		print(String.Format("1-1: {0}",nnMove.neurons[1].weights[1]));
		print(String.Format("1-2: {0}",nnMove.neurons[1].weights[2]));
		print(String.Format("1-3: {0}",nnMove.neurons[1].weights[3]));
		print(String.Format("1-4: {0}",nnMove.neurons[1].weights[4]));
		print(String.Format("1-5: {0}",nnMove.neurons[1].weights[5]));
		
		print(String.Format("2-0: {0}",nnMove.neurons[2].weights[0]));
		print(String.Format("2-1: {0}",nnMove.neurons[2].weights[1]));
		print(String.Format("2-2: {0}",nnMove.neurons[2].weights[2]));
		print(String.Format("2-3: {0}",nnMove.neurons[2].weights[3]));
		print(String.Format("2-4: {0}",nnMove.neurons[2].weights[4]));
		print(String.Format("2-5: {0}",nnMove.neurons[2].weights[5]));
		
		print(String.Format("3-0: {0}",nnMove.neurons[3].weights[0]));
		print(String.Format("3-1: {0}",nnMove.neurons[3].weights[1]));
		print(String.Format("3-2: {0}",nnMove.neurons[3].weights[2]));
		print(String.Format("3-3: {0}",nnMove.neurons[3].weights[3]));

		print(String.Format("4-0: {0}",nnMove.neurons[4].weights[0]));
		print(String.Format("4-1: {0}",nnMove.neurons[4].weights[1]));
		print(String.Format("4-2: {0}",nnMove.neurons[4].weights[2]));
		print(String.Format("4-3: {0}",nnMove.neurons[4].weights[3]));
		
		print(String.Format("5-0: {0}",nnMove.neurons[5].weights[0]));
		print(String.Format("5-1: {0}",nnMove.neurons[5].weights[1]));
		print(String.Format("5-2: {0}",nnMove.neurons[5].weights[2]));
		print(String.Format("5-3: {0}",nnMove.neurons[5].weights[3]));
		
		print(String.Format("6-0: {0}",nnMove.neurons[6].weights[0]));
		print(String.Format("6-1: {0}",nnMove.neurons[6].weights[1]));
		print(String.Format("6-2: {0}",nnMove.neurons[6].weights[2]));
		print(String.Format("6-3: {0}",nnMove.neurons[6].weights[3]));
		
		print(String.Format("7-0: {0}",nnMove.neurons[7].weights[0]));
		print(String.Format("7-1: {0}",nnMove.neurons[7].weights[1]));
		print(String.Format("7-2: {0}",nnMove.neurons[7].weights[2]));
		print(String.Format("7-3: {0}",nnMove.neurons[7].weights[3]));
		
		print(String.Format("8-0: {0}",nnMove.neurons[8].weights[0]));
		print(String.Format("8-1: {0}",nnMove.neurons[8].weights[1]));
		print(String.Format("8-2: {0}",nnMove.neurons[8].weights[2]));
        print(String.Format("8-3: {0}",nnMove.neurons[8].weights[3]));*/
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(health <= 0)
		{
			health = 100;
			transform.position = enemySpawn.position;
			master.GetComponent<master>().playerKills++;
		}
		/*if( Vector3.Distance(transform.position,lastPos) > wanderDist)
		{
			dir = Random.Range(0,1);
			if(dir == 0) dir = -1;
			lastPos = transform.position;
			Rotate(dir*100);
			print("lll");
		}*/
		timer += Time.deltaTime;
		if(attackMode){
			Vector3 newForward = new Vector3(transform.forward.z,0,-transform.forward.x);
			double rotationAngle = (double)Vector3.SignedAngle(newForward,player.transform.position - transform.position,Vector3.up);
			double leftRight;
			if(rotationAngle < 0){
				rotationAngle *= -1;
				leftRight = 0.0;
			}
			else{
				leftRight = 1.0;
			}
			
			//double[] outputs = nn.ForwardPropagationNorm(new double[]{rotationAngle,leftRight});
			
			double[] inputs = new double[]{rotationAngle,leftRight};
			double[] outputs = nn.ForwardPropagationNorm(inputs,new double[]{180.0,1.0});
			
			//print(String.Format("Angle: {0}, LeftRight: {1}, result: {2}",inputs[0],inputs[1],outputs[0]));
			if( outputs[0] < 0.4 ){
				//print(  String.Format("Error: {0}, Output: {1}, Target:{2}",((0.0 - outputs[0])*(0.0 - outputs[0]))/2, outputs[0], 0.0));
				Rotate(-180.0f /** Time.deltaTime*/);
			}
			else if( outputs[0] > 0.6 ){ 
				//print(String.Format("Angle: {0}, LeftRight: {1}",inputs[0],inputs[1]));
				//print(  String.Format("Error: {0}, Output: {1}, Target:{2}",((1.0 - outputs[0])*(1.0 - outputs[0]))/2, outputs[0], 1.0));
				Rotate(180.0f /** Time.deltaTime*/);
			}
			else {
				//print(String.Format("Angle: {0}, LeftRight: {1}",inputs[0],inputs[1]));
				//print(  String.Format("Error: {0}, Output: {1}, Target:{2}",((0.5 - outputs[0])*(0.5 - outputs[0]))/2, outputs[0], 0.5));
				if(timer > fireSpeed){
					timer = 0;
					Fire();
				}
			}
			
			double d = Vector3.Distance(transform.position,player.transform.position); //max 7
			rotationAngle = (double) Vector3.SignedAngle(player.transform.forward,transform.position - player.transform.position, Vector3.up);
			if(rotationAngle < 0){
				leftRight = 0.0;
				rotationAngle *= -1;
			}
			else{
				leftRight = 1.0;
			}

			outputs = nnMove.ForwardPropagationNorm(new double[]{rotationAngle,leftRight,d},new double[]{180.0,1.0,7.0});
			
			if(outputs[0] > 0.7)
				transform.Translate(speed*Time.deltaTime,0,0);
			if(outputs[1] > 0.7)
				transform.Translate(-speed*Time.deltaTime,0,0);
			if(outputs[2] > 0.7)
				transform.Translate(0,0,speed*Time.deltaTime);
			if(outputs[3] > 0.7)
				transform.Translate(0,0,-speed*Time.deltaTime);
		}
		
		else{
			steering = GetWanderForce();
			steering = Vector3.ClampMagnitude(steering, speed);
			
			
			lastVelocity = velocity;
			velocity = Vector3.ClampMagnitude(velocity + steering, speed * Time.deltaTime);
			
			float rotationAngle = Vector3.SignedAngle(lastVelocity,velocity,Vector3.up);
			transform.Rotate(0,rotationAngle * Time.deltaTime,0);
			
			velocity = new Vector3(velocity.magnitude, 0, 0);
			
			
			//transform.position += velocity;
			
			MoveForward(velocity);
		}
	}
	
	void MoveForward(Vector3 v){
		transform.Translate(v.x, v.y, v.z);
	}
	
	public void Rotate(float angle){
		transform.Rotate(0,angle * Time.deltaTime,0);
	}
	
	Vector3 GetWanderForce(){
		Vector3 circleCenter = velocity;
		circleCenter = Vector3.Normalize(circleCenter);
		circleCenter *= wanderDist;
		
		Vector3 displacement = new Vector3(0,0,1);
		displacement *= wanderRad;
		displacement = SetAngle(displacement);
		
		wanderAngle += UnityEngine.Random.Range(0f,1f) * angleChange -  angleChange * 0.5f;
		
		
		//Debug.Log(displacement + circleCenter);
		return displacement + circleCenter;
		
	}
	
	Vector3 SetAngle(Vector3 displacement){
		float magnitude = displacement.magnitude;
		return new Vector3(Mathf.Cos(wanderAngle) * magnitude, 0, Mathf.Sin(wanderAngle) * magnitude);
	}
	
	void Fire(){
		var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		//bullet.transform.eulerAngles = transform.eulerAngles;
		bullet.GetComponent<Rigidbody>().velocity = new Vector3(bullet.transform.forward.z,0,-bullet.transform.forward.x) * 10f ;
		
		//print(transform.forward);
	}
}
