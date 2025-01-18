using System;
using System.Collections.Generic;
using BossRush.Classes;
using UnityEngine;

namespace BossRush.Classes.Utility
{
    [Serializable]
    public struct ClassItem
    {
        [Tooltip("The name of the class")]
        public string Key;
        [Tooltip("The class")]
        public PlayableClass Value;
    }

    [Serializable]
    public class CreateClassDictionary
    {
        [SerializeField]
        public ClassItem[] ClassItems;

        /// <summary>
        /// Creates a dictionary out of ClassItems
        /// </summary>
        /// <returns>The dictionary</returns>
        public Dictionary<string, PlayableClass> ToDictionary()
        {
            Dictionary<string, PlayableClass> newDictonary = new Dictionary<string, PlayableClass>();

            foreach (var item in ClassItems)
            {
                newDictonary.Add(item.Key, item.Value);    
            }

            return newDictonary;
        }
    }
}
