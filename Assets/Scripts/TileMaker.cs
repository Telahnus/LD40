using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaker : MonoBehaviour {

	public GameObject tileFolder;
	public int tileCount = 0;
    private List<Tile> graph = new List<Tile>();

    [System.Serializable]
    public class Prefabs{
        public GameObject fourway;
        public GameObject straight;
        public GameObject threeway;
        public GameObject corner;
        public GameObject blank;
        public GameObject deadend;
    }
    public Prefabs prefabs = new Prefabs();

    // used to create new tiles, should ALWAYS be
	public GameObject createTile(){
        GameObject tileObject = Instantiate(prefabs.blank);
        tileObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
        tileObject.transform.parent = this.tileFolder.transform;

        // put tile declarations here
		Tile tileScript = tileObject.AddComponent<Tile>();
        graph.Add(tileScript);
        tileScript.constructor(0, 0, tileCount++, "blank");

        return tileObject;
    }

    public GameObject changeTilePrefab(string type, Tile oldInfo)
    {   
        GameObject newTile;
        
        switch(type)
        {
            case "fourway" : newTile = Instantiate(prefabs.fourway); break;
            case "threeway" : newTile = Instantiate(prefabs.threeway); break;
            case "straight" : newTile = Instantiate(prefabs.straight); break;
            case "corner" : newTile = Instantiate(prefabs.corner); break;
            case "deadend" : newTile = Instantiate(prefabs.deadend); break;
            default: return null;
        }

        newTile.transform.parent = this.tileFolder.transform;

        Tile newInfo = newTile.AddComponent<Tile>();
        newInfo.constructor(oldInfo.x, oldInfo.z, oldInfo.id, type);
        newInfo.copyNeighbours(oldInfo);
        graph.Add(newInfo);

        return newTile;

    }

    // flip a blank tile into a road tile according to a whole bunch of rules
    public void flipTile(Tile tile){

        int openings = 0;
        int neighbours = 0;

        // check existing connections first
        for (int i = 0; i < 4; i++){
            if (tile.neighbours[i] !=null){ // neighbours ONLY exist if given by a flipped tile
                neighbours ++;
                if (tile.openings[i]) {
                    openings++;
                }
            }
        }

        // fill up remaining spaces randomly
        if (neighbours<4){
            if (tileCount <= 1){
                for (int i = 0; i < 4; i++){
                    tile.openings[i] = true;
                    openings++;
                }
            } else {
                int start = Random.Range(0,4); // to avoid bias, start at a random direction
                for (int i=0; i<4; i++){
                    int j = (i+start)%4;
                    if (tile.neighbours[j]!=null){
                        // skip existing neighbour
                    } else {
                        if (openings==0||openings==1){
                            tile.openings[j] = true;
                            openings++;
                        } else if (openings==2){
                            if (Random.value>0.50){
                                tile.openings[j] = true;
                                openings++;
                            }
                        } else if (openings==3){
                            if (Random.value>0.75){
                                tile.openings[j] = true;
                                openings++;
                            }
                        }
                    }
                }
            }
        }

        GameObject newTile;
        int rotation = 0;

        if (openings==4){
            newTile = changeTilePrefab("fourway", tile);
        } else if (openings==3){
            newTile = changeTilePrefab("threeway", tile);
            // prefab is already closed at N(0)
            // so dont rotate if closed at 0, rotate by 90 if closed at 1, etc
            for (int i=0; i<4; i++){
                if (!tile.openings[i]) { rotation = i*90; }
            }
        } else if (openings==2){
            var north = tile.openings[0];
            var east = tile.openings[1];
            var south = tile.openings[2];
            if (north==south){ // straight
                newTile = changeTilePrefab("straight", tile); 
                if (!(north && south)){
                    rotation = 90;
                }
            } else { // corner
                newTile = changeTilePrefab("corner", tile);
                // corner prefab is 1,2
                // if i=1 works, then rotate by i-1
                for (int i = 0; i < 4; i++){
                    if (tile.openings[i] && tile.openings[(i+1)%4]) { rotation = (i-1)*90; }
                }
            }
        } else if (openings==1){
            newTile = changeTilePrefab("deadend", tile);
            // prefab opening is W (3)
            // so if opening is 0, i rotate CW by 90
            for (int i=0; i<4; i++){
                if (tile.openings[i]){rotation = (i+1)*90;}
            }
        } else {
            // you fucked up.... keep the old tile
            newTile = tile.gameObject;
        }

        // rotate new tile accordingly
        newTile.transform.Rotate(0, (rotation), 0);
        // remove old tile and gameobject
        graph.Remove(tile);
        Destroy(tile.gameObject);

        Tile me = newTile.GetComponent<Tile>();

        // finally create blank neighbours and/or exchange info
        for (int i=0; i<4; i++){
            // determine coordinate offset
            int x = 0; int z = 0;
            if (i==0) z = 1;
            else if (i==1) x = 1;
            else if (i==2) z =-1;
            else if (i==3) x = -1;
            // does tile already exist?
            Tile t = findTile(me.x+x, me.z+z);
            if (t==null){ // create a blank if null
                GameObject newObject = createTile();
                t = newObject.GetComponent<Tile>();
                t.setLocation(me.x+x, me.z+z);
            } // exchange info
            me.neighbours[i] = t;
            t.neighbours[(i + 2) % 4] = me;
            t.openings[(i + 2) % 4] = me.openings[i];
        }

    }

    public Tile findTile(int x, int z){
        foreach (Tile t in graph){
            if (t.x == x && t.z == z) return t;
        }
        return null;
    }

    /* public void Start(){
        print(graph.Count);
    } */

}
