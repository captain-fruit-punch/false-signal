namespace Yarn.Unity.Addons.DialogueWheel
{
    using System;
    using System.Collections;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using Yarn.Unity;

    /// <summary>
    /// A heavily cut back version of the existing LineView.
    /// Changes are all removals of features.
    /// </summary>
    public class SpaceSampleLineView : DialogueViewBase
    {
        [SerializeField]
        internal CanvasGroup canvasGroup;

        [SerializeField]
        internal bool useFadeEffect = true;

        [SerializeField]
        [Min(0)]
        internal float fadeInTime = 0.25f;

        [SerializeField]
        [Min(0)]
        internal float fadeOutTime = 0.05f;

        [SerializeField]
        internal TextMeshProUGUI lineText = null;

        [SerializeField]
        internal bool showCharacterNameInLineView = true;

        [SerializeField]
        internal bool useTypewriterEffect = false;

        [SerializeField]
        [Min(0)]
        internal float typewriterEffectSpeed = 0f;

        [SerializeField]
        [Min(0)]
        internal float holdTime = 1f;

        LocalizedLine currentLine = null;

        Effects.CoroutineInterruptToken currentStopToken = new Effects.CoroutineInterruptToken();

        private void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        private void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        /// <inheritdoc/>
        public override void DismissLine(Action onDismissalComplete)
        {
            // ok so in here we want to add in another little tweak
            // basically if the line is tagged as lastline
            // then we leave it up
            if (currentLine.Metadata != null)
            {
                foreach (var tag in currentLine.Metadata)
                {
                    if (tag == "lastline")
                    {
                        onDismissalComplete.Invoke();
                        return;
                    }
                }
            }

            currentLine = null;
            StartCoroutine(DismissLineInternal(onDismissalComplete));
        }

        private IEnumerator DismissLineInternal(Action onDismissalComplete)
        {
            // disabling interaction temporarily while dismissing the line
            // we don't want people to interrupt a dismissal
            var interactable = canvasGroup.interactable;
            canvasGroup.interactable = false;

            // If we're using a fade effect, run it, and wait for it to finish.
            if (useFadeEffect)
            {
                yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeOutTime, currentStopToken));
                currentStopToken.Complete();
            }

            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            // turning interaction back on, if it needs it
            canvasGroup.interactable = interactable;

            if (onDismissalComplete != null)
            {
                onDismissalComplete();
            }
        }

        /// <inheritdoc/>
        public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
        {
            currentLine = dialogueLine;

            // Cancel all coroutines that we're currently running. This will
            // stop the RunLineInternal coroutine, if it's running.
            StopAllCoroutines();

            // for now we are going to just immediately show everything
            // later we will make it fade in
            lineText.gameObject.SetActive(true);
            canvasGroup.gameObject.SetActive(true);

            int length;

            if (showCharacterNameInLineView)
            {
                lineText.text = dialogueLine.Text.Text;
                length = dialogueLine.Text.Text.Length;
            }
            else
            {
                lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                length = dialogueLine.TextWithoutCharacterName.Text.Length;
            }

            // Show the entire line's text immediately.
            lineText.maxVisibleCharacters = length;

            // Make the canvas group fully visible immediately, too.
            canvasGroup.alpha = 1;

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            onInterruptLineFinished();
        }

        /// <inheritdoc/>
        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Stop any coroutines currently running on this line view (for
            // example, any other RunLine that might be running)
            StopAllCoroutines();

            // Begin running the line as a coroutine.
            StartCoroutine(RunLineInternal(dialogueLine, onDialogueLineFinished));
        }

        private IEnumerator RunLineInternal(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            IEnumerator PresentLine()
            {
                lineText.gameObject.SetActive(true);
                canvasGroup.gameObject.SetActive(true);

                // Should we show the character name in the main text view?
                if (showCharacterNameInLineView)
                {
                    // Yep! Show the entire text.
                    lineText.text = dialogueLine.Text.Text;
                }
                else
                {
                    // Nope! Show just the text without the character name.
                    lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                }

                if (useTypewriterEffect)
                {
                    // If we're using the typewriter effect, hide all of the
                    // text before we begin any possible fade (so we don't fade
                    // in on visible text).
                    lineText.maxVisibleCharacters = 0;
                }
                else
                {
                    // Ensure that the max visible characters is effectively
                    // unlimited.
                    lineText.maxVisibleCharacters = int.MaxValue;
                }

                // If we're using the fade effect, start it, and wait for it to
                // finish.
                if (useFadeEffect)
                {
                    yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeInTime, currentStopToken));
                    if (currentStopToken.WasInterrupted)
                    {
                        // The fade effect was interrupted. Stop this entire
                        // coroutine.
                        yield break;
                    }
                }

                // If we're using the typewriter effect, start it, and wait for
                // it to finish.
                if (useTypewriterEffect)
                {
                    // setting the canvas all back to its defaults because if we didn't also fade we don't have anything visible
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    yield return StartCoroutine(
                        Effects.Typewriter(
                            lineText,
                            typewriterEffectSpeed,
                            null,
                            currentStopToken
                        )
                    );
                    if (currentStopToken.WasInterrupted)
                    {
                        // The typewriter effect was interrupted. Stop this
                        // entire coroutine.
                        yield break;
                    }
                }
            }
            currentLine = dialogueLine;

            // Run any presentations as a single coroutine. If this is stopped,
            // which UserRequestedViewAdvancement can do, then we will stop all
            // of the animations at once.
            yield return StartCoroutine(PresentLine());

            currentStopToken.Complete();

            // All of our text should now be visible.
            lineText.maxVisibleCharacters = int.MaxValue;

            // Our view should at be at full opacity.
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            // If we have a hold time, wait that amount of time, and then
            // continue.
            if (holdTime > 0)
            {
                yield return new WaitForSeconds(holdTime);
            }

            // Our presentation is complete; call the completion handler.
            onDialogueLineFinished();
        }

        /// <inheritdoc/>
        public override void UserRequestedViewAdvancement()
        {
            // We received a request to advance the view. If we're in the middle of
            // an animation, skip to the end of it. If we're not current in an
            // animation, interrupt the line so we can skip to the next one.

            // we have no line, so the user just mashed randomly
            if (currentLine == null)
            {
                return;
            }

            // we may want to change this later so the interrupted
            // animation coroutine is what actually interrupts
            // for now this is fine.
            // Is an animation running that we can stop?
            if (currentStopToken.CanInterrupt)
            {
                // Stop the current animation, and skip to the end of whatever
                // started it.
                currentStopToken.Interrupt();
            }
            else
            {
                // No animation is now running. Signal that we want to
                // interrupt the line instead.
                requestInterrupt?.Invoke();
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// If a line is still being shown dismisses it.
        /// </remarks>
        public override void DialogueComplete()
        {
            // do we still have a line lying around?
            if (currentLine != null)
            {
                currentLine = null;
                StartCoroutine(DismissLineInternal(null));
            }
        }
    }
}
