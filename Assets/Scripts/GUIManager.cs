using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {

	public Text stats;
	public Text tile;

	// Use this for initialization
	void Start () {
        //stats = GameObject.Find("Stats").GetComponent<GUI>();
        stats = GameObject.Find("Stats").GetComponent<Text>();
        stats.text = "Network Size:\nIncome:\nPublic Safety:\nCrime Rate:";
        tile = GameObject.Find("TileInfo").GetComponent<Text>();
        tile.text = "Tile Info:";

    }
	
}
