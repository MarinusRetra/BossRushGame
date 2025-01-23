using BossRush.Classes.Abilities;
using UnityEngine;

namespace BossRush.Classes
{
    public class Berserker : PlayableClass
    {
        protected override void Awake()
        {
            PrimaryAbility = gameObject.AddComponent<BasicCombo>();
            SecondaryAbility = gameObject.AddComponent<SpinAttack>();
            TertiaryAbility = gameObject.AddComponent<SpinAttack>();
            QuaternaryAbility = gameObject.AddComponent<ArmorBreakingSlash>();
        }
    }
}
