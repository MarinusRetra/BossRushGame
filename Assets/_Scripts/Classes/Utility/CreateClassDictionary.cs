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
        public string key;
        [Tooltip("The class")]
        public PlayableClass value;
    }

    [Serializable]
    public class CreateClassDictionary
    {
        [SerializeField]
        public ClassItem[] classItems;

        /// <summary>
        /// Creates a dictionary out of ClassItems
        /// </summary>
        /// <returns>The dictionary</returns>
        public Dictionary<string, PlayableClass> ToDictionary()
        {
            Dictionary<string, PlayableClass> newDictonary = new Dictionary<string, PlayableClass>();

            foreach (var item in classItems)
            {
                newDictonary.Add(item.key, item.value);    
            }

            return newDictonary;
        }
    }
}
