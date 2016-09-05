using UnityEngine;
using System.Collections;
using System;

public class Level : MonoBehaviour {

    public int MaxRow;
    public int MaxColumn;

    public GameObject[] FloorPrefabs;
    public GameObject[] BoxPrefabs;
    public GameObject[] TheePrefabs;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NormalizeLevel()
    {
        var firstChild = transform.GetChild(0);
        Vector3 delta = transform.position - firstChild.transform.position;

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.localPosition += delta;
        }

        transform.position = Vector3.zero;
    }

    public void GenerateFloor()
    {
        int floorPrefabsSize = FloorPrefabs.Length;
        int boxPrefabsSize = BoxPrefabs.Length;
        int theePrefabsSize = TheePrefabs.Length;

        for (int y = 0; y < MaxRow; y++)
        {
            for(int x = 0; x < MaxColumn; x++)
            {
                if(!IsOnEdge(x,y))
                {
                    if(floorPrefabsSize > 0)
                    {
                        Instantiate(FloorPrefabs[UnityEngine.Random.Range(0, floorPrefabsSize)], new Vector3(x, 0, y), Quaternion.identity, transform);
                    }

                } else
                {
                    if(boxPrefabsSize > 0)
                    {
                        Instantiate(BoxPrefabs[UnityEngine.Random.Range(0, boxPrefabsSize)], new Vector3(x, -0.5f, y), Quaternion.identity, transform);
                    }

                }

                if(theePrefabsSize > 0)
                {
                    GameObject treeObject = Instantiate(TheePrefabs[UnityEngine.Random.Range(0, theePrefabsSize)], new Vector3(x, 0, y), Quaternion.identity, transform) as GameObject;
                    treeObject.transform.localScale *= UnityEngine.Random.Range(0.5f, 1.0f);
                }

            }
        }
    }

    bool IsOnEdge(int x, int y)
    {
        if(x == 0 || x == MaxColumn - 1 || y == 0 || (y == MaxRow - 1))
        {
            return true;
        }
        return false;
    }
}
