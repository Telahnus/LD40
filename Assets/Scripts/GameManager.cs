using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// PROPERTIES
	private GameObject player;
    public GameObject currentTile;
	private List<Tile> graph;
	public int tileCount = 0;
	
	// PREFABS
	// to group a bunch of properties in the inspector, set as [System.Serializable]
	// then wrap in a public class, then instantiate that class
	[System.Serializable]
	public class Prefabs {
		public GameObject fourway;
        public GameObject straight;
        public GameObject tjunction;
        public GameObject lcorner;
        public GameObject blanktile;
        public GameObject cop;
        public GameObject robber;
    } public Prefabs prefabs = new Prefabs();
	
	// GAMEOBJECT REFS
	[System.Serializable]
	public class References {
		public GameObject dynamic_folder;
		public GameObject tile_folder;
	} public References refs = new References();

	// METHODS
	private GameObject createObject(GameObject obj){
		GameObject newObject = Instantiate(obj);
		newObject.transform.parent = this.refs.dynamic_folder.transform;
		return newObject;
	}

	private GameObject createTile(string type){
		GameObject newTile;
		switch(type){
			case "fourway": newTile = createObject(prefabs.fourway); break;
			case "tjunction": newTile = createObject(prefabs.tjunction); break;
			case "lcorner": newTile = createObject(prefabs.lcorner); break;
			case "straight": newTile = createObject(prefabs.straight); break;
			case "blank":
			default: 
				newTile = createObject(prefabs.blanktile); 
				newTile.transform.Rotate(0, Random.Range(0, 4) * 90, 0); 
				break;
		}
		newTile.transform.parent = this.refs.tile_folder.transform;
        Tile scriptTile = newTile.AddComponent<Tile>();
        scriptTile.constructor(0, 0, tileCount++, type);
		return newTile;
	}

	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	void Start(){
		player = createObject(prefabs.cop); player.name = "player";
		currentTile = createTile("fourway"); currentTile.name = "currentTile";
		createTile("blank").GetComponent<Tile>().setLocation(0, 1);
		createTile("blank").GetComponent<Tile>().setLocation(0, -1);
		createTile("blank").GetComponent<Tile>().setLocation(-1, 0);
		createTile("blank").GetComponent<Tile>().setLocation(1, 0);
	}

	/// Update is called every frame, if the MonoBehaviour is enabled.
	void Update(){
		
	}

}
