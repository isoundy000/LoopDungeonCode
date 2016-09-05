using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;

public class StatusUI : MonoBehaviour {

    public CharacterProperty target;

    public Slider HPSlider;
	// Use this for initialization
	void Start () {

        RefreshUI();
        target.OnDamageTakeAsObservable.TakeUntilDestroy(target).Subscribe(d =>
        {
            RefreshUI();
        });

    }
	
	// Update is called once per frame
	void Update () {
        RefreshUI();

    }


    void RefreshUI()
    {
        HPSlider.value = target.PersentHP;
    }
}
