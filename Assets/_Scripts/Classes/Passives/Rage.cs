using BossRush.Classes.Abilities;
using UnityEngine;

namespace BossRush.Classes
{
    public class Rage : PassiveAbility
    {
        public override void Do(ref EntityAbilityData data)
        {
            const float INCREASE_VALUE = 0.01f;
            const float MAX_TIMER = 5;
            data.Timer -= Time.deltaTime;
            
            if (data.HasTakenDmg)
            {
                data.Timer = MAX_TIMER;
                data.AttackSpeedMulti += INCREASE_VALUE;
                data.DmgMulti += INCREASE_VALUE;
            }
            else if (data.Timer < 0)
            {
                data.AttackSpeedMulti = 1;
                data.DmgMulti = 1;
            }
        }
    }
}
