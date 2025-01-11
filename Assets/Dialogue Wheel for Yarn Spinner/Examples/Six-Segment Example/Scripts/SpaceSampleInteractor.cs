namespace Yarn.Unity.Addons.DialogueWheel
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A minimal gameobject to associate a cinemachine camera with a character.
    /// </summary>
    /// <remarks>
    /// This is also used after trigger collision to work out which node to run when talking to characters.
    /// </remarks>
    public class SpaceSampleInteractor : MonoBehaviour
    {
        /// <summary>
        /// The name, as Yarn knows it, of the character.
        /// </summary>
        public string CharacterName;
        
        /// <summary>
        /// The node to run if you want to speak to this character.
        /// </summary>
        public string nodeName;
        
        /// <summary>
        /// The cinemachine virtual camera for this character.
        /// </summary>
        public GameObject virtualCamera;
    }
}
