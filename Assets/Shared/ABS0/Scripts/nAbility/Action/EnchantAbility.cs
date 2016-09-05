using UnityEngine;
using System.Collections;

public enum EnchantType
{
    CriticalRate,
    AttackValue,
    WalkSpeed
}

public class EnchantAbility : ActionAbility
{
    public GameObject EffectPrefab;
    CharacterProperty mOwner;

    public bool Check;
    public EnchantType type;
    public float value;
    public float time;

    void Start()
    {
        mOwner = GetComponent<CharacterProperty>();

        InitTriggerObserver(target =>
        {
            if (Check)
            {
                Cast(mOwner, target);
            }
            else
            {
                DoAbility(target);
            }

        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void DoAbility(CharacterProperty target)
    {
        bool hasEffect = false;
        EnchantEffect[] effects = target.gameObject.GetComponents<EnchantEffect>();

        for(int i = 0; i < effects.Length; i++)
        {
            EnchantEffect effect = effects[i];

            if(effect.type != type)
            {
                continue;
            }

            effect.time = time;
            effect.type = type;
            effect.value = value;
            effect.EffectPrefab = EffectPrefab;
            effect.Reset();
            hasEffect = true;
            break;
        }

        if(!hasEffect)
        {
            EnchantEffect effect = target.gameObject.AddComponent<EnchantEffect>();
            effect.time = time;
            effect.type = type;
            effect.value = value;
            effect.EffectPrefab = EffectPrefab;
        }


    }

}
