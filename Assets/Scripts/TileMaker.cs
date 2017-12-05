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
        
        Tile newInfo = newTile.AddComponent<Tile>();
        newInfo.constructor(oldInfo.x, oldInfo.z, oldInfo.id, type);
        newInfo.neighbours = oldInfo.neighbours;
        graph.Remove(oldInfo);
        graph.Add(newInfo);

        return newTile;

    }

    public void flipTile(Tile tile){
        // to determine what type of road to make, need to know neighbours
            // and next step required neighbours, so we never dead end
        int openings = 0;
        int neighbours = 0;
        //bool[] roads = [false, false, false, false];
        //bool north, east, south, west = false;

        // check for existing neighbours first
        for (int i = 0; i < 4; i++){
            Neighbour neighbour = tile.neighbours[i];
            if (neighbour){ // neighbours ONLY exist if given by a flipped tile
                neighbours ++;
                if (neighbour.isOpen) {
                    openings++;
                }
            }
        }

        if (neighbours<4){
            int start = Random.Range(0,4);
            for (int i=0; i<4; i++){
                int j = (i+start)%4;
                if (tile.neighbours[j]){
                    // skip existing neighbour
                } else {
                    Neighbour newNeighbour = new Neighbour();
                    if (openings==0||openings==1){
                        newNeighbour.constructor(null,true);
                        openings++;
                    } else if (openings==2){
                        if (Random.value>0.5){
                            newNeighbour.constructor(null,true);
                            openings++;
                        } else {
                            newNeighbour.constructor(null,false);
                        }
                    } else if (openings==3){
                        if (Random.value>0.75){
                            newNeighbour.constructor(null,true);
                            openings++;
                        } else {
                            newNeighbour.constructor(null,false);
                        }
                    }
                    tile.neighbours[j] = newNeighbour;
                }
            }
        }

        GameObject newTile;
        if (openings==4){
            newTile = changeTilePrefab("fourway", tile);
        } else if (openings==3){
            newTile = changeTilePrefab("threeway", tile);
            // set direction
        } else if (openings==2){
            var n = tile.neighbours;
            var north = n[0].isOpen;
            var south = n[2].isOpen;
            if ((north&&south)||(!north&&!south)){
               newTile = changeTilePrefab("straight", tile); 
               // set direction
            } else {
                newTile = changeTilePrefab("corner", tile);
                //set direction
            }
        } else if (openings==1){
            newTile = changeTilePrefab("deadend", tile);
            // set direction
        } else {
            // you fucked up....
        }
    }

    public Tile findTile(int x, int z){
        foreach (Tile t in graph){
            if (t.x==x && t.z==z) return t;
        }
        return null;
    }

    /* public void Start(){
        graph = new List<Tile>();
    } */

}
