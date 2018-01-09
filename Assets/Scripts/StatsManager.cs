using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

	public int tileCount = 0;
	public int income = 0;
	public int size = 0;
	public int safety = 0;
	public int tier = 0;
	public int nextTier = 0;
	public int crime = 0;
	public int stolen = 0;
	public int expense = 0;
	public int net = 0;
	public int turn = 1;
	public float danger = 0f;
	public int lossPercent = 0;

	public void resetAll(){
		tileCount = 0;
		income = 0;
		size = 0;
		safety = 0;
		tier = 0;
		nextTier = 0;
		crime = 0;
		stolen = 0;
		expense = 0;
		net = 0;
		turn = 1;
		danger = 0f;
		lossPercent = 0;
	}

	/* public void incrementTileCount(){
		tileCount++;
	} */

	/* public void incrementCriminalCount(){
		crime++;
	} */

	public bool checkWin(){
		calcNet();
		return net>=100;
	}
	public bool checkLoss(){
		calcLossPercent();
		return lossPercent>=100;
	}

	public void updateTileStats(List<Tile> graph){
		income = 0;
        size = 0;
        safety = 0;
		stolen = 0;
		int criminalCount = 0;
        foreach (Tile t in graph){
            if (t.type != "blank"){
                size++;
                safety += t.safety;
                if (t.hasCriminal) {
					stolen += t.income;
					criminalCount++;
				}
				else income += t.income;
            }
		}
		calcNet();
		calcLossPercent();
		if (criminalCount!=crime) print("criminal count didnt work!");
	}

	public void takeBackTile(Tile t){
		income += t.income;
		stolen -= t.income;
		calcNet();
        calcLossPercent();
	}

	public void updateTurnStats(List<Tile> graph){
		turn++;
		updateTileStats(graph);
	}

	public void addTileStats(Tile t){
		//tileCount++;
		size++;
		income += t.income;
		safety += t.safety;
		danger += t.danger;
		calcLossPercent();
		calcNet();
	}

	public void calcNet(){
		net = income - stolen - expense;
	}

	public void calcLossPercent(){
		lossPercent = Mathf.RoundToInt((crime / (safety / 10f)) * 100);
	}

	public void calcTier(){
        if (size < 10) { tier = 1; nextTier = 10; }
        else if (size < 25) { tier = 2; nextTier = 25; }
        else { tier = 3; nextTier = 99; }
	}
}
