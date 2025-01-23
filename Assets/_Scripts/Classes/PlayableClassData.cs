using System;
using BossRush.Classes.Abilities;
using UnityEngine;

namespace BossRush.Classes.DataObj
{
    /// <summary>
    /// Represents data for an ability, including its associated animation and damage multiplier.
    /// </summary>
    [Serializable]
    public struct AbilityData
    {
        [Tooltip("The animation clip associated with the ability")]
        public AnimationClip Clip;
        [Tooltip("Multiply base dmg by this to get the actual dmg for the ability.")]
        [Range(0.1f, 5)]public float DmgMulti;
    }


    /// <summary>
    /// Contains data related to a playable class, including models, stats, and abilities.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayableClassData", menuName = "Scriptable Objects/PlayableClassData")]
    public class PlayableClassData : ScriptableObject
    {
        [Tooltip("The 3D model representing the player's character for this class")]
        public Mesh Model;

        [Tooltip("The weapon model that the class primarily uses")]
        public Mesh Weapon;

        [Tooltip("The secondary weapon model used by the class (optional)")]
        public Mesh SecondaryWeapon;

        [Tooltip("The maximum HP values for the class at each level. Values range from 60 to 400."), Range(60, 400)]
        public float[] MaxHp;

        [Tooltip("The base damage values the class deals at each level. Values range from 5 to 75."), Range(5, 75)]
        public float[] BaseDmg;

        [Tooltip("The data associated with the primary ability.")]
        public AbilityData PrimaryData;

        [Tooltip("The data associated with the secondary ability.")]
        public AbilityData SecondaryData;

        [Tooltip("The data associated with the tertiary ability.")]
        public AbilityData TertiaryData;

        [Tooltip("The data associated with the quaternary ability.")]
        public AbilityData QuaternaryData;
    }
}