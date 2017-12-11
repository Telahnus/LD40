using System.Collections;
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
		criminalScript.setLocation(x,z);
	}

	public void moveCriminals(){
		foreach (Criminal c in criminals){
			//
		}
	}

}
