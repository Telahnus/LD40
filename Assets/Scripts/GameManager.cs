using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// PROPERTIES
	public GameObject copObject;
    public GameObject currentTileObject;
	public Camera mainCamera;

	private Cop copScript;
	private Tile currentTileScript;
	private TileMaker tileMaker;
	private GUIManager guiManager;
	private UpgradeManager upgradeManager;
	bool endConfirmed = false;
	private CriminalManager criminalManager;
	private StatsManager statsManager;
	private bool preventP = true;

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
	} public References refs = new References();

	// METHODS
	private GameObject createObject(GameObject obj){
		GameObject newObject = Instantiate(obj);
		newObject.transform.parent = this.refs.dynamic_folder.transform;
		return newObject;
	}


	/// Awake is called when the script instance is being loaded.
	void Awake(){
		tileMaker = this.GetComponent<TileMaker>();
        guiManager = this.GetComponent<GUIManager>();
        criminalManager = this.GetComponent<CriminalManager>();
        upgradeManager = this.GetComponent<UpgradeManager>();
        statsManager = this.GetComponent<StatsManager>();

        tileMaker.statsManager = statsManager;
        guiManager.statsManager = statsManager;
        criminalManager.statsManager = statsManager;
        upgradeManager.statsManager = statsManager;
		upgradeManager.guiManager = guiManager;
		upgradeManager.gameManager = this;

        mainCamera = Camera.main;
	}

	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	void Start(){

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
		if (Input.GetKeyDown(KeyCode.P)) { 
			if (!preventP) guiManager.togglePauseScreen();
		}
    }

	// action button (Q) is contextual
	// meaning... gonna be a lot of ifs
	public void doAction(){
		if (copScript.hasAP()){
			guiManager.displayAPWarning(false);
			if (currentTileScript.hasCriminal){
				if (copScript.AP >= currentTileScript.criminal.level){
					copScript.AP -= currentTileScript.criminal.level;
					criminalManager.captureCriminal(currentTileScript.criminal, currentTileScript);
					guiManager.updateCopInfo(copScript.AP);
					statsManager.takeBackTile(currentTileScript);
                    guiManager.displayStats(statsManager);
				} else { // not enuf AP to capture criminal
					guiManager.displayAPWarning(true);
				}
			}
		} else { // not enuf AP to do any actions
			guiManager.displayAPWarning(true);
		}
	}

	public void moveCommand(int direction){
		if (copScript.hasAP()){
			guiManager.displayAPWarning(false);
			if (currentTileScript.openings[direction]){

				// move the cop and camera 
				copScript.move(direction);
                mainCamera.transform.position = copObject.transform.position + new Vector3(0, 4.5f, 0);
                // update GUI as required
				if (endConfirmed) { endConfirmed = false; } // turns off "are you sure?"
				// highlight endturnbtn if appropriate
                if (copScript.AP == 0) { guiManager.updateEndTurn(true, false); }
                else { guiManager.updateEndTurn(false, false); }
				guiManager.updateCopInfo(copScript.AP); // update action points display

				// update current tile script and object
				currentTileScript = tileMaker.findTileAtLocation(copScript.x, copScript.z);
				currentTileObject = currentTileScript.gameObject;
				// flip the tile if it was blank
				if (currentTileScript.type == "blank"){
					currentTileScript = tileMaker.flipTile(currentTileScript);
					currentTileObject = currentTileScript.gameObject;
					guiManager.displayStats(statsManager);
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

			checkWinLoss();

			criminalManager.moveCriminals(tileMaker);
			criminalManager.spawnCriminals(tileMaker.graph);

			copScript.AP = copScript.maxAP;
			guiManager.updateCopInfo(copScript.AP);

			statsManager.updateTurnStats(tileMaker.graph);
			guiManager.displayStats(statsManager);
		}
	}

	public void checkWinLoss(){
		if (statsManager.checkWin()) {
			preventP = true;
			guiManager.togglePauseScreen();
			guiManager.displayWin();
		} else if (statsManager.checkLoss()){
			preventP = true;
			guiManager.togglePauseScreen();
			guiManager.displayLoss();
		}
	}

	public void requestOutpost(){
		upgradeManager.buildOutpost(currentTileScript, tileMaker);
	}

	public void startNewGame(){
		preventP = false;

		// remove existing stuff
		criminalManager.clearAll();
		tileMaker.clearAll();
		Destroy(copObject);
		statsManager.resetAll();
		upgradeManager.resetAll();
		guiManager.resetDisplay();
		mainCamera.transform.position = new Vector3(0,5,0);
		mainCamera.orthographicSize = 3;

		// re-initialize everything
        currentTileObject = tileMaker.createTile(); currentTileObject.name = "currentTile";
        currentTileScript = currentTileObject.GetComponent<Tile>();
        currentTileScript = tileMaker.flipTile(currentTileScript);
        currentTileObject = currentTileScript.gameObject;

        copObject = createObject(prefabs.cop); copObject.name = "cop";
        copScript = copObject.GetComponent<Cop>();
        copScript.setLocation(0, 0);

        statsManager.updateTileStats(tileMaker.graph);
        statsManager.calcTier();
        guiManager.displayStats(statsManager);

        upgradeManager.setCop(copScript);

		guiManager.togglePauseScreen();
	}

	public void increaseView(){
		mainCamera.orthographicSize++;
	}

}
