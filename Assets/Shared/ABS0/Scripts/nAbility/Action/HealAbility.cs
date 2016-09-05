using UnityEngine;
using System.Collections;
using UniRx;

public class HealAbility : ActionAbility {

    CharacterProperty mOwner;

    public bool Check = true;
    public int value;
	// Use this for initialization
	void Start () {
        mOwner = GetComponent<CharacterProperty>();

        InitTriggerObserver(target =>
        {
            if(Check)
            {
                Cast(mOwner, target);
            } else
            {
                DoAbility(target);
            }
            
        });
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void DoAbility(CharacterProperty target)
    {
        target.IncreaseHP(value);

        GameObject Prefab = Resources.Load<GameObject>("Effects/Hits/SCFX_LightHeal");
        GameObject effectInstance = UnityEngine.Object.Instantiate(Prefab, target.transform.position, target.transform.rotation) as GameObject;
        effectInstance.transform.SetParent(target.transform);
        effectInstance.transform.localPosition = Prefab.transform.position;
        UnityEngine.Object.Destroy(effectInstance, 1.5f);
    }
}
