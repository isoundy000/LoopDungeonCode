using UnityEngine;
using System.Collections;
using System;

public class EnchantEffect : MonoBehaviour {

    CharacterProperty mCharacterProperty;

    public GameObject EffectPrefab;
    public EnchantType type;
    public float value;
    public float time;

    float mStartTime;

    GameObject effectInstance;

    public void Reset()
    {
        if(mCharacterProperty)
        {
            mStartTime = Time.time;

            if(effectInstance == null)
            {
                effectInstance = Instantiate(EffectPrefab, transform.position + EffectPrefab.transform.position, EffectPrefab.transform.rotation, transform) as GameObject;
            }
 
            switch (type)
            {
                case EnchantType.WalkSpeed:
                    mCharacterProperty.SetWalkSpeedModifier(this, value);
                    break;
                case EnchantType.AttackValue:
                    mCharacterProperty.SetAttackValueModifier(this, value);
                    break;
                case EnchantType.CriticalRate:
                    mCharacterProperty.SetCriticalRateModifier(this, value);
                    break;
            }
        }
    }

    void Start()
    {
        mCharacterProperty = GetComponent<CharacterProperty>();
        Reset();
    }

    void Update()
    {
        if((Time.time - mStartTime) > time)
        {
            switch (type)
            {
                case EnchantType.WalkSpeed:
                    mCharacterProperty.ResetWalkSpeedModifier(this);
                    break;
                case EnchantType.AttackValue:
                    mCharacterProperty.ResetAttackValueModifier(this);
                    break;
                case EnchantType.CriticalRate:
                    mCharacterProperty.ResetCriticalRateModifier(this);
                    break;
            }
            Destroy(effectInstance);
            Destroy(this);
        }
    }
}
