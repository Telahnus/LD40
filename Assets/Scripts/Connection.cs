using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour {

	public Tile tile;
	public string direction;
	public bool isOpen;

	public void constructor(Tile tile, string direction, bool isOpen){
		this.tile = tile ? tile : null;
		this.direction = direction;
		this.isOpen = isOpen;
	}

}
