using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ParallelAction : AbilityAction  {

	IList<AbilityAction> mActions;

	public ParallelAction() {
		mActions = new List<AbilityAction> ();
	}
}
