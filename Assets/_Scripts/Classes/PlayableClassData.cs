using System;
using BossRush.Classes.Abilities;
using UnityEngine;

namespace BossRush.Classes.DataObj
{


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

        [Tooltip("The animation clip associated with the primary ability.")]
        public AnimationClip PrimaryAnimation;

        [Tooltip("The animation clip associated with the secondary ability.")]
        public AnimationClip SecondaryAnimation;

        [Tooltip("The animation clip associated with the tertiary ability.")]
        public AnimationClip TertiaryAnimation;

        [Tooltip("The animation clip associated with the quaternary ability.")]
        public AnimationClip QuaternaryAnimation;
    }
}