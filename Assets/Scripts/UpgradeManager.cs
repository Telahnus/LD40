using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

	public StatsManager statsManager;
	public GUIManager guiManager;
	public GameManager gameManager;

	public Button outpostBtn;
	public Button trainingBtn;
	public Button equipmentBtn;
	public Button chopperBtn;
	public Button roadBlockBtn;

	private Cop cop;

	public GameObject outpostPrefab;
	public GameObject dynamic_folder;

	//public int tier = 0;
	//public int next = 10;
	//public int trainingCount = 0;

	public void setCop(Cop pCop){
		cop = pCop;
	}

	public void training(){
		if (statsManager.income-statsManager.stolen-statsManager.expense>=5){
			cop.maxAP += 1;
            statsManager.expense += 5;
            trainingBtn.interactable = false;
			guiManager.displayStats(statsManager);
		} else {
			guiManager.displayTooltip("too expensive");
		}
	}

	public void outpost(){
		if (statsManager.income - statsManager.stolen - statsManager.expense >= 5){
			gameManager.requestOutpost();
        } else {
            guiManager.displayTooltip("too expensive");
        }
	}

	public void buildOutpost(Tile t, TileMaker tm){
		if (t.hasOutpost){
			guiManager.displayTooltip("This block already has an outpost.");
		} else {
			t.hasOutpost = true;
			for (int i=-1; i<=1; i++){
				for (int j=-1; j<=1; j++){
					Tile s = tm.findTileAtLocation(t.x+i, t.z+j);
					if (s != null) s.isWatched = true;
				}
			}
            statsManager.expense += 5;
            guiManager.displayStats(statsManager);

			GameObject outpostObject = Instantiate(outpostPrefab);
			outpostObject.transform.parent = dynamic_folder.transform;
			outpostObject.transform.position = new Vector3(t.x, 0, t.z);
        }
	}

}
