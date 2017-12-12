using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Criminal : MonoBehaviour {

	public int x = 0;
	public int z = 0;
	public int health = 1;
	public string type;
	private float offset = 0.25f;
	public bool canMove = false;
	public int level = 1;

	public void initialize(){
		if (Random.value>.5){
			this.level = 2;
			this.canMove = true;
		}
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.position = new Vector3(this.x+offset, 0.5f, this.z+offset);
	}

	public void move(int direction){
		if (direction==0){this.z++;}
		else if (direction==1){this.x++;}
		else if (direction==2){this.z--;}
		else if (direction==3){this.x--;}
		transform.position = new Vector3(this.x+offset, 0.5f, this.z+offset);
	}
}
