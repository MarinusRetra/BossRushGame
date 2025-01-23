using System;
using System.Collections.Generic;
using BossRush.Classes.Utility;
using BossRush.Utility;
using UnityEngine;
using BossRush.Classes;

namespace BossRush.Managers
{
    public class ClassManager : Singleton<ClassManager>
    {
        [Tooltip("The items made in here will be set into the classDictionary")]
        [SerializeField] private CreateClassDictionary _createClassDictionary;

        [Tooltip("The dictionary that holds all the classes by name")]
        private Dictionary<string, PlayableClass> _classDictionary;

        protected override void Awake()
        {
            base.Awake();

            _classDictionary = _createClassDictionary.ToDictionary();
        }

        /// <summary>
        /// Changes to class to the one that corresponds to the key.
        /// </summary>
        /// <param name="key">The name of the class you want to change to.</param>
        /// <returns>The class you change to.</returns>
        public PlayableClass ChangeClass(string key) => _classDictionary[key];
    }
}