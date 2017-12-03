using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour {

	private List<Node> graph;

	public GameObject undiscovered;
	public GameObject fourway;
	public GameObject Tjunction;
	public GameObject Lturn;
	public GameObject straightaway;

	// Use this for initialization
	void Start () {
		//initGraph();
		graph = new List<Node>();
		GameObject starter = Instantiate(fourway, Vector3.zero, Quaternion.identity);
		GameObject back = Instantiate(undiscovered, Vector3.back, Quaternion.identity);
		GameObject forward = Instantiate(undiscovered, Vector3.forward, Quaternion.identity);
		GameObject left = Instantiate(undiscovered, Vector3.left, Quaternion.identity);
		GameObject right = Instantiate(undiscovered, Vector3.right, Quaternion.identity);
		back.transform.Rotate(0, Random.Range(0, 4) * 90, 0);		
		forward.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
		left.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
		right.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void initGraph(){
		graph = new List<Node>();

	}
}
