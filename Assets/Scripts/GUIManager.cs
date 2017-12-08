using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {

    private GameManager GM;
    private TileMaker TM;
	public Text stats;
	public Text tile;

	void Awake () {
        GM = this.GetComponent<GameManager>();
        TM = this.GetComponent<TileMaker>();

        stats = GameObject.Find("Stats").GetComponent<Text>();
        stats.text = "Network Size:\nIncome:\nPublic Safety:\nCrime Rate:";
        tile = GameObject.Find("TileInfo").GetComponent<Text>();
        tile.text = "Tile Info:";
    }

    public void updateText(){

        int income = 0;
        int size = 0;
        int safety = 0;
        int crime = 0;
        foreach (Tile t in TM.graph){
            if (t.type != "blank") {
                print(t.type);
                size++;
                income += t.income;
                safety += t.safety;
            }
        }
        stats.text = "Network Size: "+size+"\nIncome: "+income+"\nPublic Safety:"+safety+"\nCrime Rate: "+crime;
    }
	
}
