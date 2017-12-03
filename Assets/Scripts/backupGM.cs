using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backupGM : MonoBehaviour {

	// GameObject references
	public GameObject tile;
    public GameObject cop;
	public GameObject Dynamic;

	// properties
	private GameObject player;
	private List<Node> graph;

    // Use this for initialization
    void Start () {
		player = createObject(this.cop); player.name = "player";
		//GameObject first = createTile(); first.GetComponent<Tile>().setType("fourway"); first.name = "first"; 
		GameObject forward = createTile(); forward.transform.Translate(0, 0, 1, Space.World); forward.name = "forward";
		GameObject back = createTile(); back.transform.Translate(0, 0, -1, Space.World); back.name = "back";
		GameObject left = createTile(); left.transform.Translate(-1, 0, 0, Space.World); left.name = "left";
		GameObject right = createTile(); right.transform.Translate(1, 0, 0, Space.World); right.name = "right";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* private GameObject createObject(GameObject obj, string name){
		GameObject newObject = createObject(obj);
		newObject.name = name;
		return newObject;
	} */

	private GameObject createObject(GameObject obj){
		GameObject newObject = Instantiate(obj);
		newObject.transform.parent = this.Dynamic.transform;
		return newObject;
	}

	private GameObject createTile(){
		GameObject obj = createObject(this.tile);
		Tile script = tile.GetComponent<Tile>();
		//script.setType("base", obj);
        return obj;
	}
}
