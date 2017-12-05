using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// PROPERTIES
	public int x, z, id;
	public string type;
	public Neighbour[] neighbours; // NESW

	// CONSTRUCTOR
	public void constructor(int x, int z, int id, string type){
		setLocation(x, z);
		this.id = id;
		this.type = type;
		this.neighbours = new Neighbour[4];
	}

	public void addNeighbour(Tile tile, bool isOpen){
		Neighbour neighbour = new Neighbour();
		neighbour.constructor(tile, isOpen);
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.Translate(x, 0, z, Space.World);
	}

}
