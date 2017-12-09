using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {

    private GameManager GM;
    private TileMaker TM;

	public Text statsInfo;
	public Text tileInfo;
    public Text copInfo;
    public Text endTurnText;

	void Awake () {
        GM = this.GetComponent<GameManager>();
        TM = this.GetComponent<TileMaker>();

        statsInfo = GameObject.Find("StatsInfo").GetComponent<Text>();
        statsInfo.text = "Network Size:\nIncome:\nPublic Safety:\nCrime Rate:";
        tileInfo = GameObject.Find("TileInfo").GetComponent<Text>();
        tileInfo.text = "Tile Info:";
        copInfo = GameObject.Find("CopInfo").GetComponent<Text>();
        copInfo.text = "Actions: * * * *";
        endTurnText = GameObject.Find("EndTurnText").GetComponent<Text>();
    }

    public void updateStatsInfo(){
        int income = 0;
        int size = 0;
        int safety = 0;
        int crime = 0;
        foreach (Tile t in TM.graph){
            if (t.type != "blank") {
                size++;
                income += t.income;
                safety += t.safety;
            }
        }
        statsInfo.text = "Network Size: "+size+"\nIncome: "+income+"\nPublic Safety:"+safety+"\nCrime Rate: "+crime;
    }

    public void updateCopInfo(){
        int AP = GM.copScript.AP;
        copInfo.text = "Actions:";
        for (int i = 0; i < AP; i++) { copInfo.text += " *"; }
    }
	
}
