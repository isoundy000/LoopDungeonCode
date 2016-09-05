using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class ABS0TouchInput : MonoBehaviour {

    public static Vector3 InputVector;
    public static Vector3 DashVector;
    public static bool Dash;
    public static bool Attack;
    public static bool HeavyAttack;
    public static bool Block;
    public static bool Kamae;

    void LateUpdate()
    {
        //InputVector = Vector3.zero;
        //Attack = false;
        //HeavyAttack = false;
        //Block = false;
        //Kamae = false;
    }

}
