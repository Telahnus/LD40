using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// PROPERTIES
	public int x, z, id;
	public string type;
	public Tile[] neighbours; // NESW
	public bool[] openings;

	// CONSTRUCTOR
	public void constructor(int x, int z, int id, string type){
		setLocation(x, z);
		this.id = id;
		this.type = type;
		this.neighbours = new Tile[4];
		this.openings = new bool[4]{false, false, false, false};
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
		transform.Translate(x, 0, z, Space.World);
	}

}
