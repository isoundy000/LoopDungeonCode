using UnityEngine;
using System.Collections;
using System;

public class WorldCell : MonoBehaviour {

    public string ConnectivityData;

    int mConnectivityValue;

    static int[] MASK_DIRECTION = { 3584, 448, 56, 7 };
    static int[] MASK = { 4032, 63 };

    void CalcConnectivityValue()
    {
        mConnectivityValue = Convert.ToInt32(ConnectivityData, 2);
    }

    public int ConnectivityValue
    {
        get
        {
            //if(mConnectivityValue == 0)
            //{
                CalcConnectivityValue();
            //}

            return mConnectivityValue;
        }
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool CanBeConnect(WorldCell other, int direction)
    {
        int otherValue = other.ConnectivityValue;

        int value = (otherValue & MASK[0]) >> 6 | (otherValue & MASK[1]) << 6;

        bool block = (value & MASK_DIRECTION[direction]) == 0 && (ConnectivityValue & MASK_DIRECTION[direction]) == 0;
        return block || (value & ConnectivityValue & MASK_DIRECTION[direction]) != 0;
    }
}
