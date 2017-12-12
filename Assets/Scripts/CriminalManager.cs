﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriminalManager : MonoBehaviour {

	public GameObject criminalFolder;
	public List<Criminal> criminals = new List<Criminal>();

	[System.Serializable]
    public class Prefabs{
        public GameObject criminal;
    }
    public Prefabs prefabs = new Prefabs();

	public void spawnCriminals(List<Tile> graph){
		foreach (Tile t in graph){
			if (t.type!="blank" && !t.hasCriminal){
				float spawnRate = t.danger;
				float val = Random.value;
				if (val<spawnRate){
					createCriminal(t.x, t.z);
					t.hasCriminal = true;
				}
			}
		}
	}

	public void createCriminal(int x, int z){
		GameObject criminalObject = Instantiate(prefabs.criminal);
		criminalObject.transform.parent = criminalFolder.transform;
		Criminal criminalScript = criminalObject.GetComponent<Criminal>();
		criminals.Add(criminalScript);
		// initialize criminal
		if (Random.value>.5){criminalScript.canMove = true;}
		criminalScript.setLocation(x,z);
	}

	public void moveCriminals(){
		TileMaker TM = this.GetComponent<TileMaker>();
		foreach (Criminal c in criminals){
			if (c.canMove){
				Tile t = TM.findTileAtLocation(c.x,c.z);
				int offset = Random.Range(0, 4);
				for (int i=0; i<4; i++){
					int mod = (offset+i)%4;
					if (t.openings[mod] && t.neighbours[mod]!=null){
						Tile n = t.neighbours[mod];
						if (n.type!="blank" && !n.hasCriminal){
							c.setLocation(n.x, n.z);
							n.hasCriminal = true;
							t.hasCriminal = false;
							break;
						}
					}
				}
				
			}
		}
	}

}