using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public World World;

    public UIController UIController;

    int mKillCount = 0;

    float lastKillTime;

    public void AddKillCount()
    {
        mKillCount++;
        lastKillTime = Time.time;
    }

    public int KillCount
    {
        get
        {
            return mKillCount;
        }
    }

    void Update()
    {
        if((Time.time - lastKillTime) > 5)
        {
            mKillCount = 0;
        }
    }
}
