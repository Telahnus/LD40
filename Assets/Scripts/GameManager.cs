using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// PROPERTIES
	private GameObject player;
    public GameObject currentTile;
	private List<Tile> graph;
	public int tileCount = 0;
	private TileMaker tileMaker;
	
	// PREFABS
	[System.Serializable]
	public class Prefabs {
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

	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	void Start(){
    	tileMaker = this.GetComponent<TileMaker>();

        player = createObject(prefabs.cop); player.name = "player";
		currentTile = tileMaker.createTile(); currentTile.name = "currentTile";

		Tile t = currentTile.GetComponent<Tile>();
		tileMaker.flipTile(t);
	}

	/// Update is called every frame, if the MonoBehaviour is enabled.
	void Update(){
		// check for key presses
			// get current tile
			// check if direction is open
				// move cop
				// check new tile
				// check if blank
					// flip tile
	}

}
