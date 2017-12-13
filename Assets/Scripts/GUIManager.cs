﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {

    //private GameManager GM;
    //private TileMaker TM;

	private Text statsInfo;
	private Text tileInfo;
    private Text copInfo;
    private Text endTurnText;
    private Text APWarning;
    private Button endTurnButton; 
    public ColorBlock highlightColors;
    public ColorBlock normalColors;

	void Awake () {
        //GM = this.GetComponent<GameManager>();
        //TM = this.GetComponent<TileMaker>();

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

        endTurnButton.colors = normalColors;
    }

    public void updateStatsInfo(List<Tile> graph, int criminalCount){
        int income = 0;
        int size = 0;
        int safety = 0;
        //int crime = criminalCount;
        foreach (Tile t in graph){
            if (t.type != "blank") {
                size++;
                income += t.income;
                safety += t.safety;
            }
        }
        statsInfo.text = "Network Size: "+size+"\nIncome: "+income+"\nPublic Safety:"+safety+"\nCrime Rate: "+criminalCount;
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
        if (t.type!="blank") tileInfo.text += "\nIncome: "+t.income+"\nSafety: "+t.safety+"\nDanger: "+t.danger;
        if (t.hasCriminal) tileInfo.text+="\n\nCriminal Info\nLevel: "+t.criminal.level;
    }

}
