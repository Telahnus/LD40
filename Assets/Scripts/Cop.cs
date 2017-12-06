using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : MonoBehaviour {

	public int x = 0;
	public int z = 0;
	public int AP = 100;

	public bool hasAP(){
		return (this.AP>0);
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.position = new Vector3(this.x, 0.5f, this.z);
	}

	public void move(int direction){
		if (direction==0){this.z++;}
		else if (direction==1){this.x++;}
		else if (direction==2){this.z--;}
		else if (direction==3){this.x--;}
		transform.position = new Vector3(this.x, 0.5f, this.z);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
