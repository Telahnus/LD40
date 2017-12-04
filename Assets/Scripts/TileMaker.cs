using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaker : MonoBehaviour {

	public GameObject tileFolder;
	public int tileCount = 0;

	[System.Serializable]
    public class Prefabs{
        public GameObject fourway;
        public GameObject straight;
        public GameObject threeway;
        public GameObject corner;
        public GameObject blank;
    }
    public Prefabs prefabs = new Prefabs();

	public GameObject createTile(string type){
        GameObject tileObject;
        switch (type){
            case "fourway": tileObject = Instantiate(prefabs.fourway); break;
            case "threeway": tileObject = Instantiate(prefabs.threeway); break;
            case "corner": tileObject = Instantiate(prefabs.corner); break;
            case "straight": tileObject = Instantiate(prefabs.straight); break;
            case "blank":
            default:
                tileObject = Instantiate(prefabs.blank);
                tileObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
                break;
        }
        tileObject.transform.parent = this.tileFolder.transform;

        // put tile declarations here
		Tile tileScript = tileObject.AddComponent<Tile>();
        tileScript.constructor(0, 0, tileCount++, type);

        return tileObject;
    }

}
