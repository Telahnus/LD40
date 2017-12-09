using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// PROPERTIES
	public int x, z, id;
	public string type;
	public Tile[] neighbours; // NESW
	public bool[] openings;
	public int income;
	public int safety;

	// CONSTRUCTOR
	public void constructor(int x, int z, int id, string type){
		setLocation(x, z);
		this.id = id;
		this.type = type;
		this.neighbours = new Tile[4];
		this.openings = new bool[4]{false, false, false, false};
		initIncome();
		initSafety();
	}

	public void initSafety(){
		float val = Random.value;
		if (val<.6) this.safety = 1;
		else if (val<.9) this.safety = 2;
		else if (val<1) this.safety = 3;
	}

	public void initIncome(){
		float val = Random.value;
		if (val<.6) this.income = 1;
		else if (val<.9) this.income = 2;
		else if (val<1) this.income = 3;
	}

	public void addNeighbour(Tile tile, bool isOpen, int direction){
		this.neighbours[direction] = tile;
		this.openings[direction] = isOpen;
	}

	public void copyNeighbours(Tile other){
		for (int i=0; i<4; i++){
			this.neighbours[i]=other.neighbours[i];
			this.openings[i] = other.openings[i];
		}
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.position = new Vector3(x,0,z);
	}

}
