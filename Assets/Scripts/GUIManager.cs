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
    public Text mainText;

    // user awake to find everything
	void Awake () {
        techInfo = GameObject.Find("TechInfo").GetComponent<Text>();
        statsInfo = GameObject.Find("StatsInfo").GetComponent<Text>();
        tileInfo = GameObject.Find("TileInfo").GetComponent<Text>();
        copInfo = GameObject.Find("CopInfo").GetComponent<Text>();
        endTurnText = GameObject.Find("EndTurnText").GetComponent<Text>();
        endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        APWarning = GameObject.Find("APWarning").GetComponent<Text>();
        upgradeInfo = GameObject.Find("UpgradeInfo").GetComponent<Text>();
    }

    // use start to pass refs and set things
    void Start(){
        APWarning.enabled = false;
        upgradeInfo.text = "";
        endTurnButton.colors = normalColors;
        pauseScreen.SetActive(true);
    }

    public void displayStats(StatsManager sm){
        statsInfo.text =  "Turn: " + sm.turn + 
                        "\nSize: " + sm.size + 
                        "\nSafety:" + sm.safety + 
                        "\nCrime: " + sm.crime;
        if (sm.lossPercent>=80){
            statsInfo.text += "\n<color=red>Loss: " + sm.lossPercent + "%</color>";
        } else {
            statsInfo.text += "\nLoss: " + sm.lossPercent +"%";
        }
        statsInfo.text +=   "\nIncome: " + sm.income + 
                            "\nStolen: " + sm.stolen + 
                            "\nExpenses: " + sm.expense +
                            "\nNet: " + sm.net;

        techInfo.text =   "Rank: " + sm.tier + 
                        "\nNext Rank: " + sm.size + "/" + sm.nextTier;
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
            //if (t.isWatched) tileInfo.text += "\nUnder surveillance";
        }
        if (t.hasCriminal) tileInfo.text+="\n\nCriminal Info\nLevel: "+t.criminal.level;
    }

    public void displayTooltip(string text){
        string output = "";
        switch (text)
        {
            case "outpost": output = "Setup outpost on current tile, reducing danger of surrounding tiles, Cost 5"; break;
            case "training": output = "Increase maximum AP by 1, Cost 5"; break;
            case "chopper": output = "Increase view of the city, Cost 5"; break;
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

    public void displayWin(){
        mainText.text = "You're a true crime fighter! Congratulations!";
    }

    public void displayLoss(){
        mainText.text = "This is worse than Gotham! You lost!";
    }

    public void resetDisplay(){
        mainText.text = "Welcome to the gam!";
    }

}
