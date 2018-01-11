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
	private bool selected = false;

	public Criminal criminal;
	private GUIManager GM;
	private TileMaker TM;

	void Awake(){
		GM = GameObject.Find("GameManager").GetComponent<GUIManager>();
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
		GM.updateTileInfo(this);
		/* foreach(Tile t in TM.graph) {
			if (t.selected){
				t.selected = false;
				t.UnHighlight();
			}
		}
		selected = true; */
	}

	void Highlight(){
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

	void UnHighlight(){
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

	void OnMouseEnter(){
		Highlight();
	}

	void OnMouseExit(){
        UnHighlight();
    }
	
}
