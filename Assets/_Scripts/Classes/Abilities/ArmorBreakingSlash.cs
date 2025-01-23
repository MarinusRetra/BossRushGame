using System;
using System.Collections;
using BossRush.Classes.Abilities;
using BossRush.Classes.DataObj;
using BossRush.Utility;
using UnityEngine;

namespace BossRush.Classes
{
    public class ArmorBreakingSlash : Ability
    {
        public override IEnumerator Do(EntityAbilityData entityAbilityData, AbilityData abilityData, float baseDmg,
            Transform entityTransform,
            Action<bool> callBack = null)
        {
            //  Checks if the animationClip equals null else it will default to a duration of 1 second.
            var timer = abilityData.Clip != null ? abilityData.Clip.length : 1f;

            //  TODO: If the Physics.SphereCast() gets replaced with an OnTriggerEnter() remove variable below.
            //  How big the area is the ability can hit.
            const float HIT_BOX_SIZE = 1f;
            //  How big the offset of the hit box is.
            var offset = entityTransform.forward * 1;
            //  Returns if the attack already hit a IDamageable.
            var hasDealtDmg = false;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                //  TODO: Implement logic for the animation.

                //  TODO: Delete this if statement if changed to use OnTriggerEnter().
                //  Skips the dmg step of the while loop if dmg was already dealt.
                if (hasDealtDmg) yield return null;

                //  TODO: Maybe change this for an OnTriggerEnter() when models and animations are finished.
                //  TODO: Add a LayerMask with the boss layer in the SphereCast so it won't dmg entities it isn't suppose too.
                if (Physics.SphereCast(entityTransform.position + offset, HIT_BOX_SIZE, entityTransform.forward,
                        out var hit))
                {
                    //  Checks if the user hits a IDamageable and if so deals dmg and sets hasDealtDmg to true.
                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        //  TODO: Implement functionality for status effects.

                        damageable.TakeDamageServerRpc(baseDmg * abilityData.DmgMulti);
                        hasDealtDmg = true;
                    }
                }

                yield return null;
            }

            callBack.Invoke(true);
        }
    }
}
