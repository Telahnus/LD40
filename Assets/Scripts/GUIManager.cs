using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIManager : MonoBehaviour {

    //private GameManager GM;
    //private TileMaker TM;
    public StatsManager statsManager;

	private Text techInfo;
    private Text statsInfo;
	private Text tileInfo;
    private Text copInfo;
    private Text upgradeInfo;
    private Text endTurnText;
    private Text APWarning;
    private Button endTurnButton; 
    public ColorBlock highlightColors;
    public ColorBlock normalColors;

    private bool paused = true;
    public GameObject pauseScreen;

	void Awake () {
        //GM = this.GetComponent<GameManager>();
        //TM = this.GetComponent<TileMaker>();

        techInfo = GameObject.Find("TechInfo").GetComponent<Text>();
        techInfo.text = "Rank: 1\nNext Rank: X/10";
        statsInfo = GameObject.Find("StatsInfo").GetComponent<Text>();
        statsInfo.text = "Network Size:\nIncome:\nPublic Safety:\nCrime Rate:";
        tileInfo = GameObject.Find("TileInfo").GetComponent<Text>();
        tileInfo.text = "Tile Info";
        copInfo = GameObject.Find("CopInfo").GetComponent<Text>();
        copInfo.text = "Actions: * * * *";
        endTurnText = GameObject.Find("EndTurnText").GetComponent<Text>();
        endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        APWarning = GameObject.Find("APWarning").GetComponent<Text>();
        APWarning.enabled = false;
        upgradeInfo = GameObject.Find("UpgradeInfo").GetComponent<Text>();
        upgradeInfo.text = "";

        endTurnButton.colors = normalColors;
    }

    public void displayStats(StatsManager sm){
        statsInfo.text = "Network Size: " + sm.size + 
                        "\nPublic Safety:" + sm.safety + "\nCrime Rate: " + sm.crime + 
                        "\nIncome: " + sm.income + "\nStolen: " + sm.stolen + "\nExpenses: " + sm.expense +
                        "\nNet: " + (sm.income-sm.stolen-sm.expense);
        techInfo.text = "Rank: " + sm.tier + "\nNext Rank: " + sm.size + "/" + sm.nextTier;
    }

    public void displayAPWarning(bool b){
        APWarning.enabled = b;
    }

    public void updateCopInfo(int AP){
        copInfo.text = "Actions:";
        for (int i = 0; i < AP; i++) { copInfo.text += " *"; }
        if (AP==0) updateEndTurn(true, false);
    }

    public void updateEndTurn(bool highlight, bool askConfirmation){
        if (askConfirmation) {
            endTurnText.text = "Are you sure?";
        } else {
            endTurnText.text = "End Turn";
        }

        if (highlight){
            endTurnButton.colors = highlightColors;
        } else {
            endTurnButton.colors = normalColors;
        }
    }
	
    public void updateTileInfo(Tile t){
        tileInfo.text = "Tile Info\nType: "+t.type;
        if (t.type!="blank") {
            tileInfo.text += "\nIncome: "+t.income+"\nSafety: "+t.safety+"\nDanger: "+t.danger;
            if (t.isWatched) tileInfo.text += "\nUnder surveillance";
        }
        if (t.hasCriminal) tileInfo.text+="\n\nCriminal Info\nLevel: "+t.criminal.level;
    }

    public void displayTooltip(string text){
        string output = "";
        switch (text)
        {
            case "outpost": output = "Setup outpost on current tile, reducing danger of surrounding tiles, Cost 5"; break;
            case "training": output = "Increase maximum AP by 1, Cost 5"; break;
            case "too expensive": output = "Request denied. Not enough income."; break;
            default: output = text; break;
        }
        //print(text);//data.selectedObject.name);
        upgradeInfo.text = output;
    }

    public void togglePauseScreen(){
        paused = !paused;
        pauseScreen.SetActive(paused);
    }

}
