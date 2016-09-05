using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HealAction : AbilityAction {

	float value;

	public HealAction(float value) {
		this.value = value;
	}

	protected override void Start ()
	{
        for (int i = 0; i < mTargets.Count; i++) {
            if (mTargets[i] == null)
            {
                continue;
            }
			mTargets[i].IncreaseHP (value);

            GameObject Prefab = Resources.Load<GameObject>("Effects/Hits/SCFX_LightHeal");
            GameObject effectInstance = UnityEngine.Object.Instantiate(Prefab, mTargets[i].transform.position, mTargets[i].transform.rotation) as GameObject;
            effectInstance.transform.SetParent(mTargets[i].transform);
            effectInstance.transform.localPosition = Prefab.transform.position;
            UnityEngine.Object.Destroy(effectInstance, 1.5f);
        }

		status = AbilityActionStatus.Success;
	}
}
