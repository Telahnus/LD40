using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// PROPERTIES
	public GameObject copObject;
    public GameObject currentTileObject;
	public Camera mainCamera;

	public Cop copScript;
	private Tile currentTileScript;
	private List<Tile> graph;
	public int tileCount = 0;
	private TileMaker tileMaker;
	private GUIManager guiManager;
	bool endConfirmed = false;
	public Text endTurnText;
	
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
		guiManager = this.GetComponent<GUIManager>();

		mainCamera = Camera.main;

        currentTileObject = tileMaker.createTile(); currentTileObject.name = "currentTile";
		currentTileScript = currentTileObject.GetComponent<Tile>();
		
		currentTileScript = tileMaker.flipTile(currentTileScript);
		currentTileObject = currentTileScript.gameObject;

		copObject = createObject(prefabs.cop); copObject.name = "cop";
		copScript = copObject.GetComponent<Cop>();

		endTurnText = GameObject.Find("EndTurnText").GetComponent<Text>();

		//print(guiManager);
		guiManager.updateStatsInfo();
		//copScript.x = 0; copScript.z=0; 
	}

	/// Update is called every frame, if the MonoBehaviour is enabled.
	void Update(){
		int direction = -1;
		// check for key presses
		if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow)){ direction=0; } 
		else if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)){ direction=1; } 
		else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow)){ direction=2; } 
		else if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)){ direction=3; } 
		if (direction!=-1){
			if (copScript.hasAP()){
				//currentTileScript = tileMaker.findTile(copScript.x, copScript.z);
				if (currentTileScript.openings[direction]){
					copScript.move(direction);
					if (endConfirmed){
						endConfirmed = false;
						endTurnText.text = "End Turn";
					}
					guiManager.updateCopInfo();
					mainCamera.transform.position = copObject.transform.position + new Vector3(0,5,0); 
					currentTileScript = tileMaker.findTile(copScript.x, copScript.z);
					currentTileObject = currentTileScript.gameObject;
					if (currentTileScript.type == "blank"){
						currentTileScript = tileMaker.flipTile(currentTileScript);
						currentTileObject = currentTileScript.gameObject;
						guiManager.updateStatsInfo();
					}
				}
			}
		}
		// had issues with double clicks when the keycode was 'enter' after having highlighted the button with a click first
		if (Input.GetKeyDown(KeyCode.E)){
			endTurn();
		}
	}

	public void endTurn(){
		//print("PRE - - AP="+copScript.AP+", endconfirmed=" + endConfirmed);
		if (copScript.AP>0 && !endConfirmed){
			endTurnText.text = "Are you sure?";
			endConfirmed = true;
		} else {
			endConfirmed = false;
			endTurnText.text = "End Turn";
			copScript.AP = 4;
			guiManager.updateCopInfo();
		}
		//print("POST - - AP="+copScript.AP+", endconfirmed=" + endConfirmed);	
	}

}
