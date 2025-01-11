namespace Yarn.Unity.Addons.DialogueWheel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Yarn.Unity.Addons.DialogueWheel.Extensions;

#nullable enable


    public class ImageWheelPresentationController : PresentationController
    {
        
        // the arrow inside the centre of the wheel.
        // also represents the centre of wheel itself for angle calculations
        [SerializeField] private Transform arrow;
        
        [SerializeField] CanvasGroup canvasGroup;
        private IEnumerable<WheelOptionView> options = Array.Empty<WheelOptionView>();

        [SerializeField] Image wheelImage;

        [SerializeField] Sprite noSelectionWheelSprite;

        [SerializeField] bool showCursor = false;

        [SerializeField] HighlightMode wheelHighlightMode = HighlightMode.Immediate;
        [SerializeField] float wheelCrossfadeTime = 0.1f;

        private Coroutine? crossfadeCoroutine = null;

        public void Awake() {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }

        public override void Dismiss()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;

            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }

        public override void PresentOptionViews(IEnumerable<WheelOptionView> enumerable)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if (showCursor == false) {
                // Cursor.lockState = CursorLockMode.Locked;
                // Cursor.visible = false;
            }

            this.options = enumerable;
        }

        public override void SetCurrentAngle(float angleInDegrees)
        {
            arrow.localRotation = Quaternion.AngleAxis(angleInDegrees, Vector3.forward);
            
        }

        public override void SetHighlightedOptionView(WheelOptionView? selectedOption)
        {
            Sprite wheelSprite = noSelectionWheelSprite;

            foreach (var option in this.options) {
                if (option == selectedOption) {
                    option.SetHighlighted(true);
                    if (option is ImageWheelOptionView imageWheelOptionView) {
                        wheelSprite = imageWheelOptionView.WheelSprite;
                    }
                } else {
                    option.SetHighlighted(false);
                }
            }

            if (wheelSprite != null) {
                switch (wheelHighlightMode)
                {
                    case HighlightMode.Immediate:
                        wheelImage.sprite = wheelSprite;
                        break;
                    case HighlightMode.Crossfade:
                        if (crossfadeCoroutine != null) {
                            // We're in the middle of crossfading. Cancel that
                            // crossfade, and switch to our 'none selected'
                            // sprite so that we're only fading up the current
                            // segment.
                            wheelImage.sprite = noSelectionWheelSprite;
                            wheelImage.StopCoroutine(crossfadeCoroutine);
                        }
                        crossfadeCoroutine = wheelImage.CrossfadeImage(
                            wheelSprite,
                            wheelCrossfadeTime,
                            () => crossfadeCoroutine = null);
                        break;
                }
            }

        }
    }
}