using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : MonoBehaviour {

	public int x = 0;
	public int z = 0;
	public int AP = 4;
	public int maxAP = 4;
	public float yOffset = 0.5f;

	public bool hasAP(){
		return (this.AP>0);
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.position = new Vector3(this.x, yOffset, this.z);
	}

	public void move(int direction){
		if (direction==0){this.z++;}
		else if (direction==1){this.x++;}
		else if (direction==2){this.z--;}
		else if (direction==3){this.x--;}
		transform.position = new Vector3(this.x, yOffset, this.z);
		this.AP--;
	}

}
