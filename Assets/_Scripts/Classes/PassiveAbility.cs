using BossRush.Classes.Abilities;
using UnityEngine;

namespace BossRush.Classes
{
    public abstract class PassiveAbility : MonoBehaviour
    {
        public abstract void Do(ref EntityAbilityData data);
    }
}
