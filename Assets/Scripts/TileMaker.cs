using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaker : MonoBehaviour {

	public GameObject tileFolder;
	public int tileCount = 0;
    public List<Tile> graph = new List<Tile>();
    public StatsManager statsManager;

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

    private DiscreteDistribution dangerSet;
    private DiscreteDistribution incomeSet;
    private DiscreteDistribution safetySet;

    public void clearAll(){
        foreach (Transform child in tileFolder.transform){
            Destroy(child.gameObject);
        }
        graph.Clear();
        tileCount = 0;
    }

    public void Awake(){
        initDistributions();
    }

    public void initDistributions()
    {
        // danger - chance of criminal spawning
        dangerSet = new DiscreteDistribution();
        dangerSet.add(0.00f, 0.90f, 0.01f);
        dangerSet.add(0.90f, 0.97f, 0.02f);
        dangerSet.add(0.97f, 1.00f, 0.03f);
        // income - gold per turn
        incomeSet = new DiscreteDistribution();
        incomeSet.add(0.00f, 0.60f, 1);
        incomeSet.add(0.60f, 0.90f, 2);
        incomeSet.add(0.90f, 1.00f, 3);
        // safety - ???
        safetySet = new DiscreteDistribution();
        safetySet.add(0.00f, 0.60f, 1);
        safetySet.add(0.60f, 0.90f, 2);
        safetySet.add(0.90f, 1.00f, 3);
    }
    
    
    // used to create new blank tiles
	public GameObject createTile(){
        // create gameObject
        GameObject tileObject = Instantiate(prefabs.blank);
        tileObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
        tileObject.transform.parent = this.tileFolder.transform;
        // attach script component
		Tile tileScript = tileObject.AddComponent<Tile>();
        graph.Add(tileScript);
        
        int income = (int)incomeSet.getRandomValue();
        int safety = (int)safetySet.getRandomValue();
        float danger = (float)dangerSet.getRandomValue();

        tileScript.constructor(0, 0, tileCount++, "blank", income, safety, danger);
        statsManager.tileCount++;

        return tileObject;
    }

    // creates a new tile, and copies all the old info to the new tile
    public Tile changeTilePrefab(string type, Tile oldInfo){  
         
        GameObject newTileObject;
        switch(type)
        {
            case "fourway" : newTileObject = Instantiate(prefabs.fourway); break;
            case "threeway" : newTileObject = Instantiate(prefabs.threeway); break;
            case "straight" : newTileObject = Instantiate(prefabs.straight); break;
            case "corner" : newTileObject = Instantiate(prefabs.corner); break;
            case "deadend" : newTileObject = Instantiate(prefabs.deadend); break;
            default: return null; // should error handle
        }
        newTileObject.transform.parent = this.tileFolder.transform;

        // initialize script component
        Tile newTileScript = newTileObject.AddComponent<Tile>();
        newTileScript.type = type;
        copyTileInfo(oldInfo, newTileScript);
        checkWatchStatus(newTileScript);

        graph.Add(newTileScript);
        statsManager.addTile(newTileScript);
        return newTileScript;
    }

    public void checkWatchStatus(Tile t){
        for (int i = -1; i <= 1; i++){
            for (int j = -1; j <= 1; j++){
                Tile s = findTileAtLocation(t.x + i, t.z + j);
                if (s != null && s.hasOutpost){
                    t.isWatched = true;
                    return;
                }
            }
        }
    }

    // TODO: dont forget to update as Tile changes
    public void copyTileInfo (Tile oldTile, Tile newTile){
        newTile.setLocation(oldTile.x, oldTile.z);
        newTile.id = oldTile.id;
        newTile.danger = oldTile.danger;
        newTile.safety = oldTile.safety;
        newTile.income = oldTile.income;
        for (int i = 0; i < 4; i++){
            newTile.neighbours[i] = oldTile.neighbours[i];
            newTile.openings[i] = oldTile.openings[i];
        }
        
    }

    // flip a blank tile into a road tile according to a whole bunch of rules
    public Tile flipTile(Tile tile){

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

        Tile newTileScript;
        int rotation = 0;

        if (openings==4){
            newTileScript = changeTilePrefab("fourway", tile);
        } else if (openings==3){
            newTileScript = changeTilePrefab("threeway", tile);
            // prefab is already closed at N(0)
            // so dont rotate if closed at 0, rotate by 90 if closed at 1, etc
            for (int i=0; i<4; i++){
                if (!tile.openings[i]) { rotation = i*90; }
            }
        } else if (openings==2){
            var north = tile.openings[0];
            var south = tile.openings[2];
            if (north==south){ // straight
                newTileScript = changeTilePrefab("straight", tile); 
                if (!(north && south)){
                    rotation = 90;
                }
            } else { // corner
                newTileScript = changeTilePrefab("corner", tile);
                // corner prefab is 1,2
                // if i=1 works, then rotate by i-1
                for (int i = 0; i < 4; i++){
                    if (tile.openings[i] && tile.openings[(i+1)%4]) { rotation = (i-1)*90; }
                }
            }
        } else if (openings==1){
            newTileScript = changeTilePrefab("deadend", tile);
            // prefab opening is W (3)
            // so if opening is 0, i rotate CW by 90
            for (int i=0; i<4; i++){
                if (tile.openings[i]){rotation = (i+1)*90;}
            }
        } else {
            // you fucked up.... keep the old tile
            newTileScript = tile;
        }

        GameObject newTileObject = newTileScript.gameObject;

        // rotate new tile accordingly
        newTileObject.transform.Rotate(0, (rotation), 0);
        // remove old tile and gameobject
        graph.Remove(tile);
        Destroy(tile.gameObject);

        // finally create blank neighbours and/or exchange info
        for (int i=0; i<4; i++){
            // determine coordinate offset
            int x = 0; int z = 0;
            if (i==0) z = 1;
            else if (i==1) x = 1;
            else if (i==2) z =-1;
            else if (i==3) x = -1;
            // does tile already exist?
            Tile neighbourScript = findTileAtLocation(newTileScript.x+x, newTileScript.z+z);
            if (neighbourScript==null){ // create a blank if null
                GameObject newObject = createTile();
                neighbourScript = newObject.GetComponent<Tile>();
                neighbourScript.setLocation(newTileScript.x+x, newTileScript.z+z);
            } // exchange info
            newTileScript.neighbours[i] = neighbourScript;
            neighbourScript.neighbours[(i + 2) % 4] = newTileScript;
            neighbourScript.openings[(i + 2) % 4] = newTileScript.openings[i];
        }

        return newTileScript;

    }

    public Tile findTileAtLocation(int x, int z){
        foreach (Tile t in graph){
            if (t.x == x && t.z == z) return t;
        }
        return null;
    }

    /* public void Start(){
        print(graph.Count);
    } */

}
