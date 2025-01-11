namespace Yarn.Unity.Addons.DialogueWheel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// A custom dialogue view which manages cinemachine cameras.
    /// </summary>
    /// <remarks>
    /// Each character has a cinemachine virtual camera associated with them.
    /// Each time a line comes in we grab the character off it and disable all cameras that aren't associated with that character.
    /// Creating a look similar to games like Mass Effect (albiet with much less manual camera work)
    /// </remarks>
    public class SpaceSampleCameraManager : DialogueViewBase
    {
        /// <summary>
        /// The player camera gets special treatment in options.
        /// As such we need to know the name of the player.
        /// </summary>
        [SerializeField] private string playerName;
        
        private SpaceSampleInteractor[] characters;

        void Start()
        {
            characters = FindObjectsOfType<SpaceSampleInteractor>();

            // we turn off all cameras by default
            foreach (var c in characters)
            {
                c.virtualCamera.SetActive(false);
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Has a list of cinemachine cameras and maps this to who each one is for.
        /// Then grabs the current character out of the line and turns on their camera
        /// and turns off everyone elses.
        /// </remarks>
        public override void RunLine(LocalizedLine dialogueLine, System.Action onDialogueLineFinished)
        {
            if (dialogueLine.CharacterName != null)
            {
                var disables = characters.Where(c => c.CharacterName != dialogueLine.CharacterName).Select(c => c.virtualCamera);
                var enable = characters.Where(c => c.CharacterName == dialogueLine.CharacterName).First().virtualCamera;
                if (enable == null)
                {
                    Debug.LogWarning($"Asked to show the camera for {dialogueLine.CharacterName} but there is no camera associated with that character name");
                }
                else
                {
                    enable.SetActive(true);
                    foreach (var c in disables)
                    {
                        c.SetActive(false);
                    }
                }
            }
            onDialogueLineFinished.Invoke();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Works in a similar fashion to the <see cref="RunLine"/> but always assumes the player camera should be shown.
        /// </remarks>
        public override void RunOptions(DialogueOption[] dialogueOptions, System.Action<int> onOptionSelected)
        {
            if (playerName == null)
            {
                Debug.LogWarning($"Attempting to set the camera for options to look at the player but playerName is null");
                return;
            }
            // we want it so that the options always show the player
            var disables = characters.Where(c => c.CharacterName != playerName).Select(c => c.virtualCamera);
            var enable = characters.Where(c => c.CharacterName == playerName).First().virtualCamera;
            if (enable == null)
            {
                Debug.LogWarning($"Attempted to show the camera for {playerName} options but there is no camera associated with that name");
                return;
            }

            enable.SetActive(true);
            foreach (var c in disables)
            {
                c.SetActive(false);
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Just turns off all dialogue cameras.
        /// </remarks>
        public override void DialogueComplete()
        {
            foreach (var c in characters)
            {
                c.virtualCamera.SetActive(false);
            }
        }
    }
}