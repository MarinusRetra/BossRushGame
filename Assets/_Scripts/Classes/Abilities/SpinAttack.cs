using System;
using System.Collections;
using BossRush.Classes.Abilities;
using BossRush.Classes.DataObj;
using BossRush.Utility;
using UnityEngine;

namespace BossRush.Classes
{
    public class SpinAttack : Ability
    {
        public override IEnumerator Do(EntityAbilityData entityAbilityData, AbilityData abilityData, float baseDmg,
            Transform entityTransform, Action<bool> callBack = null)
        {
            //  Checks if the animationClip equals null else it will default to a duration of 2 second.
            var timer = abilityData.Clip != null ? abilityData.Clip.length : 2f;
            //  The max amount of time between hits.
            const float MAX_DMG_TIMER = 0.25f;
            //  The timer to keep track of when the hit happens.
            var dmgTimer = MAX_DMG_TIMER;

            //  TODO: If the Physics.SphereCast() gets replaced with an OnTriggerEnter() remove variable below.
            //  How big the area is the ability can hit.
            const float HIT_BOX_SIZE = 1f;
            //  How big the offset of the hit box is.
            var offset = -entityTransform.forward * 0.5f;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                dmgTimer -= Time.deltaTime;    
             
                //  TODO: Implement logic for the animation.

                //  Checks if the dmgTimer is greater than 0 else returns to skip the dmg part of the ability.
                if (dmgTimer > 0f) yield return null;
                dmgTimer = MAX_DMG_TIMER;


                //  TODO: Maybe change this for an OnTriggerEnter() when models and animations are finished.
                //  TODO: Add a LayerMask with the boss layer in the SphereCast so it won't dmg entities it isn't suppose too.
                if (Physics.SphereCast(entityTransform.position + offset, HIT_BOX_SIZE, entityTransform.forward, out var hit))
                {
                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamageServerRpc(baseDmg * abilityData.DmgMulti);
                    }
                }

                yield return null;
            }

            callBack.Invoke(true);
        }
    }
}
