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
	//public Text endTurnText;
	//private GameObject endTurnButton;
	private CriminalManager criminalManager;
	
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
		criminalManager = this.GetComponent<CriminalManager>();

		mainCamera = Camera.main;

        currentTileObject = tileMaker.createTile(); currentTileObject.name = "currentTile";
		currentTileScript = currentTileObject.GetComponent<Tile>();
		
		currentTileScript = tileMaker.flipTile(currentTileScript);
		currentTileObject = currentTileScript.gameObject;

		copObject = createObject(prefabs.cop); copObject.name = "cop";
		copScript = copObject.GetComponent<Cop>();
		copScript.setLocation(0,0);

		//endTurnText = GameObject.Find("EndTurnText").GetComponent<Text>();
		//endTurnButton = GameObject.Find("EndTurnButton");

		//print(guiManager);
		guiManager.updateStatsInfo(tileMaker.graph, 0);
		//copScript.x = 0; copScript.z=0; 
	}

	/// Update is called every frame, if the MonoBehaviour is enabled.
	void Update(){
		// check for movement key presses
		if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow)){ moveCommand(0); } 
		else if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)){ moveCommand(1); } 
		else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow)){ moveCommand(2); } 
		else if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)){ moveCommand(3); } 
		// had issues with double clicks when the keycode was 'enter' 
			//after having highlighted the button with a click first
		if (Input.GetKeyDown(KeyCode.E)){ endTurn(); }
		if (Input.GetKeyDown(KeyCode.Q)) { doAction(); }
    }

	// action button is contextual
	// meaning... gonna be a lot of ifs
	public void doAction(){
		if (copScript.hasAP()){
			guiManager.displayAPWarning(false);
			if (currentTileScript.hasCriminal){
				if (copScript.AP >= currentTileScript.criminal.level){
					copScript.AP -= currentTileScript.criminal.level;
					criminalManager.captureCriminal(currentTileScript.criminal, currentTileScript);
					guiManager.updateCopInfo(copScript.AP);
					callUpdateStats();
					guiManager.updateTileInfo(currentTileScript);
				} else {
					guiManager.displayAPWarning(true);
				}
			}
		} else {
			guiManager.displayAPWarning(true);
		}
	}

	public void callUpdateStats(){
		guiManager.updateStatsInfo(tileMaker.graph, criminalManager.criminals.Count);
	}

	public void moveCommand(int direction){
		if (copScript.hasAP()){
			guiManager.displayAPWarning(false);
			//currentTileScript = tileMaker.findTile(copScript.x, copScript.z);
			if (currentTileScript.openings[direction]){
				copScript.move(direction);
				if (endConfirmed){ endConfirmed = false; }
				if (copScript.AP==0){
					guiManager.updateEndTurn(true,false);
				} else { 
					guiManager.updateEndTurn(false,false);
				}
				guiManager.updateCopInfo(copScript.AP);
				mainCamera.transform.position = copObject.transform.position + new Vector3(0,4.5f,0); 
				currentTileScript = tileMaker.findTileAtLocation(copScript.x, copScript.z);
				currentTileObject = currentTileScript.gameObject;
				if (currentTileScript.type == "blank"){
					currentTileScript = tileMaker.flipTile(currentTileScript);
					currentTileObject = currentTileScript.gameObject;
					callUpdateStats();
				}
			}
		} else {
			guiManager.displayAPWarning(true);
		}
	}

	public void endTurn(){
		guiManager.displayAPWarning(false);
		if (copScript.AP>0 && !endConfirmed){
			guiManager.updateEndTurn(true, true);
			endConfirmed = true;
		} else {
			endConfirmed = false;
			guiManager.updateEndTurn(false, false);
			criminalManager.moveCriminals();
			criminalManager.spawnCriminals(tileMaker.graph);
			// check win/loss conditions
			copScript.AP = copScript.maxAP;
			guiManager.updateCopInfo(copScript.AP);
			callUpdateStats();
		}
	}

}
