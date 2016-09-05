using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UniRx;

public class Damage
{
    public CharacterProperty from;
    public CharacterProperty to;
    public float value;
    public float power;
    public bool ignore = false;
    public bool isCirtical = false;
    public bool miss = false;

    public Damage(CharacterProperty from, CharacterProperty to, float damage)
    {
        this.from = from;
        this.to = to;
        this.value = damage;
    }
}

public class CharacterProperty : MonoBehaviour {

    World mWorld;
    WorldCell ownerCell;

    Dictionary<Object, float> mWalkSpeedModifiers;
    float mWalkSpeedModifier;

    Dictionary<Object, float> mAttackValueModifiers;
    float mAttackValueModifier;

    Dictionary<Object, float> mCriticalRateModifiers;
    float mCriticalRateModifier;

    public Character Prototype;

    Character mCharacter;

    //
    Subject<CharacterProperty> OnDied;

    public IObservable<CharacterProperty> OnDiedAsObservable
    {
        get
        {
            return OnDied ?? (OnDied = new Subject<CharacterProperty>());
        }
    }

    private bool mDamageImmune;

    Subject<Damage> OnDamageTake;

    public IObservable<Damage> OnDamageTakeAsObservable
    {
        get
        {
            return OnDamageTake ?? (OnDamageTake = new Subject<Damage>());
        }
    }

    Subject<Damage> BeforeDamageTake;

    public IObservable<Damage> BeforeDamageTakeAsObservable
    {
        get
        {
            return BeforeDamageTake ?? (BeforeDamageTake = new Subject<Damage>());
        }
    }

    // Use this for initialization
    void Start () {

        mWalkSpeedModifiers = new Dictionary<Object, float>();
        mAttackValueModifiers = new Dictionary<Object, float>();
        mCriticalRateModifiers = new Dictionary<Object, float>();

        mDamageImmune = false;
        GameObject gameGameController = GameObject.FindGameObjectWithTag("GameController");

        if(gameGameController)
        {
            GameController gameController = gameGameController.GetComponent<GameController>();
            mWorld = gameController.World;
        }
    }

	void Awake () {
		mCharacter = Instantiate (Prototype) as Character;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public float AttackValue
    {
        get
        {
            return mCharacter.AttackValue + mAttackValueModifier;
        }
    }

    public float DefenceValue
    {
        get
        {
            return mCharacter.DefenceValue;
        }
    }

    public float DodgeRate
    {
        get
        {
            return mCharacter.DodgeRate;
        }
    }

    public float CriticalRate
    {
        get
        {
            return mCharacter.CriticalRate + mCriticalRateModifier;
        }
    }

    public float HitRate
    {
        get
        {
            return mCharacter.HitRate;
        }
    }

    public float HP {
		get {
			return mCharacter.HP;
		}
	}

	public void IncreaseHP(float value) {
		mCharacter.HP += value;

		if (mCharacter.HP >= mCharacter.MaxHP) {
			mCharacter.HP = mCharacter.MaxHP;
		}
	}

	public void IncreaseMP(float value) {
		mCharacter.MP += value;

		if (mCharacter.MP >= mCharacter.MaxMP) {
			mCharacter.MP = mCharacter.MaxMP;
		}
	}

	public void Hit(CharacterProperty target, float value) {
		if (IsAlive) {

            if(!DamageImmune)
            {
                float damage = (value * target.AttackValue) - DefenceValue;

                if(damage < 0)
                {
                    damage = 1;
                }

                Damage damageContext = new Damage(target, this, damage);

                damageContext.power = value;

                if (UnityEngine.Random.value < target.CriticalRate)
                {
                    damageContext.isCirtical = true;
                    damageContext.value *= 2;
                    Debug.Log("Cirtical->" + damageContext.value);
                }

                if (UnityEngine.Random.value < DodgeRate)
                {
                    damageContext.miss = true;
                }


                if (BeforeDamageTake != null)
                {
                    BeforeDamageTake.OnNext(damageContext);
                } 
                if(!damageContext.miss)
                {
                    ReduceHP(damageContext.value);
                }
                if (OnDamageTake != null)
                {
                    OnDamageTake.OnNext(damageContext);
                }

                if(mWorld != null)
                {
                    mWorld.SetDamageInfo(damageContext);
                }
            }

            if (IsDied && OnDied != null)
            {
                OnDied.OnNext(target);
                OnDied.OnCompleted();
            }
        }
	}

	public void ReduceHP(float value) {
		
		if (IsDied) {
			return;
		}

		mCharacter.HP -= value;

		if (mCharacter.HP <= 0) {
			mCharacter.HP = 0;
        }
	}

	public void ReduceMP(float value) {

		if (IsDied) {
			return;
		}

		mCharacter.MP -= value;

		if (mCharacter.MP <= 0) {
			mCharacter.MP = 0;
		}
	}

	public float WalkSpeed {
		get {
			return mCharacter.WalkSpeed + mWalkSpeedModifier;
		}
		set {
			mCharacter.WalkSpeed = value;
		}
	}

	public float ResetWalkSpeed() {
		mCharacter.WalkSpeed = Prototype.WalkSpeed;
		return mCharacter.WalkSpeed;
	}

	public bool IsAlive {
		get {
			return mCharacter.HP > 0;
		}
	}

	public bool IsDied {
		get {
			return mCharacter.HP <= 0;
		}
	}

	public float PersentHP {
		get {
			return (mCharacter.MaxHP != 0) ? mCharacter.HP / mCharacter.MaxHP : 0;
		}
	}

	public float PersentMP {
		get {
			return (mCharacter.MaxMP != 0) ? mCharacter.MP / mCharacter.MaxMP : 0;
		}
	}

	public bool HasEnoughHP(float cost) {
		return (mCharacter.HP - cost) >= 0;
	}

	public bool HasEnoughMP(float cost) {
		return (mCharacter.MP - cost) >= 0;
	}

    public bool DamageImmune
    {
        get
        {
            return mDamageImmune;
        }
        set
        {
            mDamageImmune = value;
        }
        
    }


    public float AttackRange {
		get {
			return mCharacter.AttackRange;
		}
	}

	public float AttackSpeed {
		get {
			return mCharacter.AttackSpeed;
		}
	}

    public float ViewDistance
    {
        get
        {
            return mCharacter.ViewDistance;
        }
    }

    public float FieldOfViewAngle
    {
        get
        {
            return mCharacter.FieldOfViewAngle;
        }
    }

    public int[] Skills {
		get {
			return mCharacter.Skills;
		}
	}

    public void SetWalkSpeedModifier(Object applier, float value)
    {
        if(!mWalkSpeedModifiers.ContainsKey(applier))
        {
            mWalkSpeedModifiers.Add(applier, value);
        } else
        {
            mWalkSpeedModifiers[applier] = value;
        }

        mWalkSpeedModifier = 0;

        List<float> values = new List<float>(mWalkSpeedModifiers.Values);

        for(int i = 0; i < values.Count; i ++)
        {
            mWalkSpeedModifier += values[i];
        }
    }

    public void ResetWalkSpeedModifier(Object applier)
    {
        if (mWalkSpeedModifiers.ContainsKey(applier))
        {
            mWalkSpeedModifiers.Remove(applier);
        }

        mWalkSpeedModifier = 0;

        List<float> values = new List<float>(mWalkSpeedModifiers.Values);

        for (int i = 0; i < values.Count; i++)
        {
            mWalkSpeedModifier += values[i];
        }
    }

    public void SetAttackValueModifier(Object applier, float value)
    {
        if (!mAttackValueModifiers.ContainsKey(applier))
        {
            mAttackValueModifiers.Add(applier, value);
        }
        else
        {
            mAttackValueModifiers[applier] = value;
        }

        mAttackValueModifier = 0;

        List<float> values = new List<float>(mAttackValueModifiers.Values);

        for (int i = 0; i < values.Count; i++)
        {
            mAttackValueModifier += values[i];
        }
    }

    public void ResetAttackValueModifier(Object applier)
    {
        if (mAttackValueModifiers.ContainsKey(applier))
        {
            mAttackValueModifiers.Remove(applier);
        }

        mAttackValueModifier = 0;

        List<float> values = new List<float>(mAttackValueModifiers.Values);

        for (int i = 0; i < values.Count; i++)
        {
            mAttackValueModifier += values[i];
        }
    }

    public void SetCriticalRateModifier(Object applier, float value)
    {
        if (!mCriticalRateModifiers.ContainsKey(applier))
        {
            mCriticalRateModifiers.Add(applier, value);
        }
        else
        {
            mCriticalRateModifiers[applier] = value;
        }

        mCriticalRateModifier = 0;

        List<float> values = new List<float>(mCriticalRateModifiers.Values);

        for (int i = 0; i < values.Count; i++)
        {
            mCriticalRateModifier += values[i];
        }
    }

    public void ResetCriticalRateModifier(Object applier)
    {
        if (mCriticalRateModifiers.ContainsKey(applier))
        {
            mCriticalRateModifiers.Remove(applier);
        }

        mCriticalRateModifier = 0;

        List<float> values = new List<float>(mCriticalRateModifiers.Values);

        for (int i = 0; i < values.Count; i++)
        {
            mCriticalRateModifier += values[i];
        }
    }

    public void Revive()
    {
        mCharacter.HP = 1;
    }

#if UNITY_EDITOR

    void DrawAttackRange()
    {
        Vector3 positionOffset = Vector3.zero;
        var oldColor = UnityEditor.Handles.color;
        var color = Color.red;
        color.a = 0.2f;
        UnityEditor.Handles.color = color;

        var beginDirection = Quaternion.AngleAxis(360, Vector3.up) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), transform.up, beginDirection, 360, Prototype.AttackRange);

        UnityEditor.Handles.color = oldColor;
    }

    void DrawViewRange()
    {
        Vector3 positionOffset = Vector3.zero;
        float angleOffset = 0;
        var oldColor = UnityEditor.Handles.color;
        var color = Color.green;
        color.a = 0.1f;
        UnityEditor.Handles.color = color;

        var halfFOV = Prototype.FieldOfViewAngle * 0.5f + angleOffset;
        var beginDirection = Quaternion.AngleAxis(-halfFOV, Vector3.up) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), transform.up, beginDirection, Prototype.FieldOfViewAngle, Prototype.ViewDistance);

        UnityEditor.Handles.color = oldColor;
    }

    public void OnDrawGizmosSelected()
    {
        if(Prototype)
        {
            
            DrawViewRange();

            DrawAttackRange();
        }

    }
#endif
}
