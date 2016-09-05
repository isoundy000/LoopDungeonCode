using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class DropItemDataItem
{
    public GameObject ItemObject;
    public float possibility;
}

public class DropItemWhenDied : MonoBehaviour {

    CharacterProperty mCharacterProperty;
    public DropItemDataItem[] DropItemDataItems;

    public GameObject DeathEffect;

    public float R;

    // Use this for initialization
    void Start () {
        mCharacterProperty = GetComponent<CharacterProperty>();

        mCharacterProperty
        .OnDiedAsObservable
        .Subscribe(t =>
        {
            DropItem();

            if(DeathEffect)
            {
                GameObject DeathEffectObj = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(DeathEffectObj, 1.0f);
                Destroy(gameObject);
            }

        });
    }

    void DropItem()
    {
        for(int i = 0; i < DropItemDataItems.Length; i++)
        {
            DropItemDataItem itemData = DropItemDataItems[i];

            if(UnityEngine.Random.value < itemData.possibility)
            {
                Vector2 r = UnityEngine.Random.insideUnitCircle * R;
                Vector3 position = new Vector3(r.x, transform.position.y, r.y);
                GameObject itemObj = Instantiate(itemData.ItemObject, position + transform.position, Quaternion.identity) as GameObject;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
