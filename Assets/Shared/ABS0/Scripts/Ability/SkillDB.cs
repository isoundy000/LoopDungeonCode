using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDB : ScriptableObject {

	public TextAsset[] SkillDatas;


	public static Ability CreateAbility(CharacterProperty agent, int id) {
		if (id == 0) {
			Ability ability = new Ability (agent, 0, "IceArrow", 2.0f, 1.0f)
				.AddAction (new ProjectileAction (10, "Arrows/IceArrow", 8))
				.AddAction (new DamageAction (5))
				.AddAction (new EffectAction (new Effect[] {
					new SlowEffect (5.0f, 0.5f)
			}));
			
			return ability;
		} else if (id == 1) {
			Ability ability = new Ability (agent, 1, "FireBall", 0.0f, 10.0f)
				.AddAction (new DelayAction (0.3f))
				.AddAction (new ProjectileAction (10, "FireMega", 5))
				.AddAction (new DamageAction (20))
				.AddAction (new EffectAction (new Effect[] {
                    new SlowEffect (5.0f, 0.5f)
            }));
			return ability;
		} else if (id == 2) {
			Ability ability = new Ability (agent, 2, "Heal", 3.0f, 1.0f)
				.AddAction (new HealAction (10));
			return ability;
		} else if (id == 3) {
			Ability ability = new Ability (agent, 3, "Explose On Dead", 3.0f, 20.0f)
				.AddAction (new EffectAction (new Effect[] {
				new AfterDiedEffect ("Effects/Blasts/WaterSphereBlast")
			}));
			return ability;
		} else if (id == 4) {
            Ability ability = new Ability(agent, 4, "Storm Blast", 10.0f, 10.0f)
                .AddAction(new BlastAction(10, 10, "Effects/Blasts/ShadowSphereBlast"));
			return ability;
		} else if (id == 5) {
			Ability ability = new Ability (agent, 5, "Physic Attack", 3.0f, 0.0f)
				.AddAction (new DamageAction (50));
			return ability;
		} else if (id == 6) {
			Ability ability = new Ability (agent, 6, "Physic 3xAttack", 3.0f, 0.0f)
				.AddAction (new AnimationAction("attack0","attack0_0"))
				.AddAction (new DamageAction (5))
				.AddAction (new AnimationAction("attack0","attack0_1"))
				.AddAction (new DamageAction (10))
				.AddAction (new AnimationAction("attack0","attack0_2"))
				.AddAction (new DamageAction (30));
			return ability;
		}
		return null;
	}
}
