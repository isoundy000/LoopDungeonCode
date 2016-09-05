using UnityEngine;
using System.Collections;
using UniRx;
using System;

public interface ITrigger {

   IObservable<CharacterProperty> OnBeTriggerredObservable();
}
