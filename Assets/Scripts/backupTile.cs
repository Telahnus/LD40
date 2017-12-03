using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backupTile : MonoBehaviour
{

    // GameObject references
    public GameObject baseTile;
    public GameObject fourway;
    public GameObject Tjunction;
    public GameObject Lturn;
    public GameObject straightaway;

    //properties
    public Tile[] connections;
    public bool isDiscovered = false;

    // private methods
    private void addConnection(int direction, Tile tile)
    {
        this.connections[direction] = tile;
    }

    // public methods
    public void setType(string type)
    {
        setType(type, this.gameObject);
    }
    public void setType(string type, GameObject obj)
    {
        while (gameObject.transform.childCount > 0)
        {
            GameObject.Destroy(gameObject.transform.GetChild(0));
        }
        GameObject newObject;
        switch (type)
        {
            case "fourway": newObject = Instantiate(this.fourway); break;
            case "straight": newObject = Instantiate(this.straightaway); break;
            case "corner": newObject = Instantiate(this.Lturn) as GameObject; break;
            case "tjunction": newObject = Instantiate(this.Tjunction) as GameObject; break;
            case "base":
            default:
                newObject = Instantiate(this.baseTile) as GameObject;
                newObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);
                break;
        }
        newObject.transform.parent = obj.transform;
    }

    // Use this for initialization
    void Start()
    {
        this.connections = new Tile[4]; // [N,E,S,W]
    }

    // Update is called once per frame
    void Update()
    {

    }


}
