using UnityEngine;

namespace BossRush.Classes.Data
{
    [CreateAssetMenu(fileName = "PlayableClassData", menuName = "Scriptable Objects/PlayableClassData")]
    public class PlayableClassData : ScriptableObject
    {
        [Tooltip("Player model of the class")] 
        public Mesh model;

        [Tooltip("The weapon model that the class uses")]
        public Mesh weapon;

        [Tooltip("The secondary weapon model the claas uses, this one can be left empty")]
        public Mesh secondaryWeapon;

        [Tooltip("The max hp the class has for each level"), Range(60, 400)]
        public float[] maxHp;

        [Tooltip("The base dmg the class deals for each level"), Range(5, 75)]
        public float[] baseDmg;

        [Tooltip("The animation for the primary attack")]
        public AnimationClip primaryAnimation;

        [Tooltip("The animation for the secondary attack")]
        public AnimationClip secondaryAnimation;
        
        [Tooltip("The animation for the tertiary attack")]
        public AnimationClip tertiaryAnimation;

        [Tooltip("The animation for the quaternary attack")]
        public AnimationClip quaternaryAnimation;
    }
}
