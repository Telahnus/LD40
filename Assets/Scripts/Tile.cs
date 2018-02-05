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
	public float danger;
	public bool hasCriminal = false;
	public bool hasOutpost = false;
	public bool isWatched = false;
	public bool isSelected = false;
	public bool isHovered = false;
	public bool hasBuilding = false;
	public bool hasRoadBlock = false;

	public Criminal criminal;
	private GameManager GM;

	void Awake(){
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		this.neighbours = new Tile[4];
        this.openings = new bool[4] { false, false, false, false };
	}

	// CONSTRUCTOR
	// all the info should be generated from the TileMaker and sent in
		// trying to seperate responsibilities
	public void constructor(int x, int z, int id, string type, int income, int safety, float danger){
		setLocation(x, z);
		this.id = id;
		this.type = type;

		this.income = income;
		this.safety = safety;
		this.danger = danger;
	}

	public void addNeighbour(Tile tile, bool isOpen, int direction){
		this.neighbours[direction] = tile;
		this.openings[direction] = isOpen;
	}

	public void setLocation(int x, int z){
		this.x = x;
		this.z = z;
		transform.position = new Vector3(x,0,z);
	}

	public void setCriminal(Criminal c){
		this.criminal = c;
		this.hasCriminal = c!=null;
	}

	void OnMouseDown(){
		GM.selectTile(this);
	}
	void OnMouseEnter(){
		GM.Highlight(this);
	}
	void OnMouseExit(){
		GM.UnHighlight(this);
    }
	
}
