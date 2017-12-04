using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// PROPERTIES
	public int x, z, id;
	public string type;
	public Connection[] connections; // NESW

	// CONSTRUCTOR
	public void constructor(int x, int z, int id, string type){
		setLocation(x, z);
		this.id = id;
		this.type = type;
		this.connections = new Connection[4];
	}

	public void addConnection(Tile tile, string direction, bool isOpen){
		var connection = new Connection();
		connection.constructor(tile, direction, isOpen);
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.Translate(x, 0, z, Space.World);
	}

}
