using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

	public StatsManager statsManager;
	public GUIManager guiManager;
	public GameManager gameManager;

	[System.Serializable]
    public class Buttons{
        public Button outpost;
        public Button training;
        public Button equipment;
        public Button chopper;
        public Button roadblock;
    } public Buttons buttons = new Buttons();

	[System.Serializable]
    public class Prices{
        public int training = 5;
		public int outpost = 5;
		public int chopper = 5;
		public int equipment = 5;
		public int roadblock = 1;
    } public Prices prices = new Prices();

	private Cop cop;

	public GameObject outpostPrefab;
	public GameObject roadblockPrefab;
	public GameObject upgrades_folder;

	//public int tier = 0;
	//public int next = 10;
	//public int trainingCount = 0;

	public void resetAll(){
		buttons.outpost.interactable = true;
		buttons.training.interactable = true;
		buttons.equipment.interactable = true;
		buttons.chopper.interactable = true;
		buttons.roadblock.interactable = true;

		foreach (Transform child in upgrades_folder.transform){
            Destroy(child.gameObject);
        }
	}

	public void setCop(Cop pCop){
		cop = pCop;
	}

	public void chopper(){
		if (statsManager.net >= prices.chopper){
			addExpense(prices.chopper);
            buttons.chopper.interactable = false;
			gameManager.increaseView();
        } else {
            guiManager.displayTooltip("too expensive");
        }
	}

	public void addExpense(int expense){
		statsManager.expense += expense;
		statsManager.calcNet();
		guiManager.displayStats(statsManager);
	}

	public void training(){
		if (statsManager.net >= prices.training){
			cop.maxAP += 1;
			addExpense(prices.training);
            buttons.training.interactable = false;
		} else {
			guiManager.displayTooltip("too expensive");
		}
	}

	public void outpost(){
		if (statsManager.net >=  prices.outpost){
			gameManager.requestResources("outpost"); //grabs required objects then comes back here
        } else {
            guiManager.displayTooltip("too expensive");
        }
	}

	public void buildOutpost(Tile t, TileMaker tm){
		if (t.hasBuilding){
			guiManager.displayTooltip("This block already has a building.");
		} else {
			t.hasBuilding = true;
			t.hasOutpost = true;
			for (int i=-1; i<=1; i++){
				for (int j=-1; j<=1; j++){
					Tile s = tm.findTileAtLocation(t.x+i, t.z+j);
					if (s != null) s.isWatched = true;
				}
			}
			addExpense(prices.outpost);

			GameObject outpostObject = Instantiate(outpostPrefab);
			outpostObject.transform.parent = upgrades_folder.transform;
			outpostObject.transform.position = new Vector3(t.x, 0, t.z);
        }
	}

	public void roadblock(){
		if (statsManager.net >=  prices.roadblock){
			gameManager.requestResources("roadblock"); //grabs required objects then comes back here
        } else {
            guiManager.displayTooltip("too expensive");
        } 
	}

	public void buildRoadBlock(Tile t, TileMaker tm){
		if (t.hasBuilding){
			guiManager.displayTooltip("This block already has a building.");
		} else {
			t.hasBuilding = true;
			t.hasRoadBlock = true;
			addExpense(prices.roadblock);

			GameObject roadblockObject = Instantiate(roadblockPrefab);
			roadblockObject.transform.parent = upgrades_folder.transform;
			roadblockObject.transform.position = new Vector3(t.x, 0, t.z);
		}
	}

	public void equipment(){
		if (statsManager.net >= prices.equipment){
			cop.power += 1;
			addExpense(prices.equipment);
            buttons.equipment.interactable = false;
		} else {
			guiManager.displayTooltip("too expensive");
		}
	}

}
