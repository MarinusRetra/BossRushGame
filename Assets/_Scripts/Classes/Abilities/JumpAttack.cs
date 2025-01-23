using System;
using System.Collections;
using BossRush.Classes.Abilities;
using BossRush.Classes.DataObj;
using BossRush.Utility;
using UnityEngine;

namespace BossRush.Classes
{
    public class JumpAttack : Ability
    {
        public override IEnumerator Do(EntityAbilityData entityAbilityData, AbilityData abilityData, float baseDmg, Transform entityTransform,
            Action<bool> callBack = null)
        {
            bool jumped = false;
            bool regrounded = false;
            //  Checks if the animationClip equals null else it will default to a duration of 1 second.
            var clipLength = abilityData.Clip != null ? abilityData.Clip.length : 1;
            //  How big the offset of the groundCheck.
            var offsetGroundCheck = Vector3.down * 1;

            //  TODO: If the Physics.SphereCast() gets replaced with an OnTriggerEnter() remove variable below.
            //  How big the area is the ability can hit.
            const float HIT_BOX_SIZE = 1f;
            //  How big the offset of the hit box is.
            var offset = -entityTransform.forward * 0.5f;


            while (!regrounded)
            {
                clipLength -= Time.deltaTime;

                //  TODO: Implement logic for the animation.

                //  Check if the animation of the ability is still on going, if so skips the rest of the Enumerator.
                if (clipLength > 1) yield return null;

                //  Makes the entity jump forward if the entity hasn't jumped yet.
                if (!jumped)
                {
                    jumped = true;
                    entityAbilityData.Rb.AddRelativeForce((Vector3.forward + Vector3.up) * 2, ForceMode.Impulse);
                }

                if (clipLength > 0) yield return null;
                //  TODO: Add LayerMask for ground layer.
                //  Checks if the entity landed on the ground again, if not skips the rest of the Enumerator.
                if (!Physics.SphereCast(entityTransform.position + offsetGroundCheck, 0.1f, Vector3.down,
                        out var groundHit))
                {

                }

                //  Set regrounded to true so the Enumerator will end.
                regrounded = true;

                //  TODO: Maybe change this for an OnTriggerEnter() when models and animations are finished.
                //  TODO: Add a LayerMask with the boss layer in the SphereCast so it won't dmg entities it isn't suppose too.
                if (Physics.SphereCast(entityTransform.position + offset, HIT_BOX_SIZE, entityTransform.forward, out var hit))
                {
                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamageServerRpc(baseDmg * abilityData.DmgMulti);
                    }
                }
                
                //  Makes the callBack return true to let the entity know they can use an ability again

                yield return null;
            }
            
            callBack.Invoke(true);
        }
    }
}
