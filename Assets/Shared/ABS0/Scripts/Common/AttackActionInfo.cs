using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AttackActionInfo : ScriptableObject {

    public int id;
    public int colliderId;
    public float value;

    public int ability = -1;

    public AttackActionInfo(int colliderId, float value)
    {
        colliderId = id;
        this.value = value;
    }
}