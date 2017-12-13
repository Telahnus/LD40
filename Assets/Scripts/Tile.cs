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
	public Criminal criminal;
	private GUIManager GM;

	void Start(){
		GM = GameObject.Find("GameManager").GetComponent<GUIManager>();
	}

	// CONSTRUCTOR
	public void constructor(int x, int z, int id, string type){
		setLocation(x, z);
		this.id = id;
		this.type = type;
		this.neighbours = new Tile[4];
		this.openings = new bool[4]{false, false, false, false};
		initIncome();
		initSafety();
		initDanger();
	}

	public void initDanger(){
		float val = Random.value;
		if (val<.9) this.danger = 0.01f;
		else if (val<.97) this.danger = 0.05f;
		else if (val<1)this.danger = 0.10f;
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

	public void setCriminal(Criminal c){
		this.criminal = c;
		this.hasCriminal = c!=null;
	}

	void OnMouseDown(){
		GM.updateTileInfo(this);
	}

	void OnMouseEnter(){
		foreach (Renderer r in GetComponentsInChildren<Renderer>()){
			if (r.tag == "Border"){
				Color newColor = new Color();
				newColor = r.material.color;
				newColor.r += 0.2f;
				newColor.g += 0.2f;
				newColor.b += 0.2f;
				r.material.color = newColor;
			}
        }
	}

	void OnMouseExit(){
        foreach (Renderer r in GetComponentsInChildren<Renderer>()){
			if (r.tag == "Border"){
				Color newColor = new Color();
				newColor = r.material.color;
				newColor.r -= 0.2f;
				newColor.g -= 0.2f;
				newColor.b -= 0.2f;
				r.material.color = newColor;
			}
        }
    }
	
}
