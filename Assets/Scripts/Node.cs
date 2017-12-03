using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	// private variables
	private int xCoordinate, zCoordinate, ID;
	private Node[] connections = new Node[4]; //[N,E,S,W]

	// private methods
	private void addConnection(int direction, Node node){
		this.connections[direction] = node;
	}

	// public methods

	// Constructor
	public Node(int x, int z, int ID){
		this.xCoordinate = x;
		this.zCoordinate = z;
		this.ID = ID;
	}

}
