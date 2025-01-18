using UnityEngine;

namespace BossRush.Classes.Data
{
    [CreateAssetMenu(fileName = "PlayableClassData", menuName = "Scriptable Objects/PlayableClassData")]
    public class PlayableClassData : ScriptableObject
    {
        [Tooltip("Player model of the class")] 
        public Mesh Model;

        [Tooltip("The weapon model that the class uses")]
        public Mesh Weapon;

        [Tooltip("The secondary weapon model the claas uses, this one can be left empty")]
        public Mesh SecondaryWeapon;

        [Tooltip("The max hp the class has for each level"), Range(60, 400)]
        public float[] MaxHp;

        [Tooltip("The base dmg the class deals for each level"), Range(5, 75)]
        public float[] BaseDmg;

        [Tooltip("The animation for the primary attack")]
        public AnimationClip PrimaryAnimation;

        [Tooltip("The animation for the secondary attack")]
        public AnimationClip SecondaryAnimation;
        
        [Tooltip("The animation for the tertiary attack")]
        public AnimationClip TertiaryAnimation;

        [Tooltip("The animation for the quaternary attack")]
        public AnimationClip QuaternaryAnimation;
    }
}
