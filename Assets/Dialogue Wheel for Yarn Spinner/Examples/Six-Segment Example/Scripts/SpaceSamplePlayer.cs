#if USE_INPUTSYSTEM
namespace Yarn.Unity.Addons.DialogueWheel
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using TMPro;
    using Yarn.Unity;

    /// <summary>
    /// A basic 3rd person player that handles movement and launching dialogue.
    /// </summary>
    public class SpaceSamplePlayer : MonoBehaviour, SampleControls.IMitchellActions
    {
        /// <summary>
        /// The overlay to show a "talk to X" overlay
        /// </summary>
        [SerializeField] GameObject helpOverlay;

        /// <summary>
        /// Our custom inputactions controls
        /// </summary>
        [SerializeField] SampleControls controls;

        /// <summary>
        /// The 3rd person camera, used to determine the movement camera-relative of the player
        /// </summary>
        [SerializeField] new Transform camera;
        /// <summary>
        /// a transform that keeps the camera relative facing so we can use it for movement.
        /// </summary>
        [SerializeField] Transform facing;

        /// <summary>
        /// How fast the player moves
        /// </summary>
        [SerializeField] float moveSpeed = 10;
        /// <summary>
        /// How fast the player rotates
        /// </summary>
        [SerializeField] float rotationSpeed = 10;

        /// <summary>
        /// The rate at which the player moves downward (i.e. gravity factor)
        /// </summary>
        [SerializeField] private float gravity;

        

        private DialogueRunner runner;
        private string node;

        private Vector2 inputDiff;

        private CharacterController controller;
        
        // just ensuring we have the controls ready
        private void OnEnable()
        {
            if (controls != null)
            {
                return;
            }

            controls = new SampleControls();
            controls.Mitchell.SetCallbacks(this);
            controls.Mitchell.Enable();
        }

        // likewise making sure they are disabled
        private void OnDisable()
        {
            controls.Mitchell.Disable();
        }

        void Awake() {
            helpOverlay.SetActive(false);
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Start()
        {
            runner = FindObjectOfType<DialogueRunner>();
            controller = GetComponent<CharacterController>();
        }

        // after dialogue is finished we want to turn the player controls back on
        // called by the dialogue runner event
        public void OnDialogueComplete()
        {

            controls.Mitchell.Enable();
        }

        // called when the player presses the talk button
        // just checks we aren't already talking and starts the dialogue running
        public void OnTalk(InputAction.CallbackContext context)
        {
            if (runner.IsDialogueRunning)
            {
                return;
            }

            if (node == null)
            {
                return;
            }

            if (!context.performed)
            {
                return;
            }

            controls.Mitchell.Disable();
            helpOverlay.gameObject.SetActive(false);
            runner.StartDialogue(node);
        }
        // Called when moving, we just store the movement vector for later use
        public void OnMove(InputAction.CallbackContext context)
        {
            inputDiff = context.ReadValue<Vector2>();
        }
        // is not called, just need to have it to comply with the interface
        public void OnLook(InputAction.CallbackContext context)
        {
            // Debug.Log("Looking");
            return;
        }

        void Update()
        {
            // if dialogue is running we don't want to allow any form of movement
            // the views are in control now
            if (runner.IsDialogueRunning)
            {
                return;
            }

            // if the movement value hasn't changed we do nothing
            if (inputDiff == Vector2.zero)
            {
                return;
            }

            // setting the rotation
            // we have a facing transform which always points in the camera relative direction
            // this is the direction we use to work out which way forward is instead of player obj's forward
            // this is so we don't end up with tank style controls where the camera itself has no impact on direction
            var cameraPos = new Vector3(camera.position.x, this.transform.position.y, camera.position.z);
            var viewDirection = this.transform.position - cameraPos;
            facing.forward = viewDirection.normalized;
            var rotation = facing.forward * inputDiff.y + facing.right * inputDiff.x;
            this.transform.forward = Vector3.Lerp(this.transform.forward, rotation, rotationSpeed * Time.deltaTime);

            // now we move
            // similar to the above because we now have a valid forward direction we can just move along it
            var movementDirection = (facing.forward * inputDiff.y + facing.right * inputDiff.x).normalized;
            movementDirection += Vector3.down * gravity;
            controller.Move(movementDirection * moveSpeed * Time.deltaTime);

            if (Physics.Raycast(transform.position, Vector3.down, out var hit)) {
                transform.position = hit.point + Vector3.up * 0.05f;
            }
            

        }

        // called when we bonk into the interaction volume of the characters in the scene
        private void OnTriggerEnter(Collider other)
        {
            // we grab the interactor off the character so we know who to talk to
            var interactor = other.GetComponent<SpaceSampleInteractor>();
            if (interactor != null)
            {
                // show the talk to X overlay
                var text = helpOverlay.GetComponentInChildren<TMP_Text>();
                if (text != null) {
                    text.text = $"Talk to {interactor.CharacterName}";
                }
                // save this character node so that when the player does start the conversation we know what node to run
                node = interactor.nodeName;

                if (!runner.IsDialogueRunning)
                {
                    helpOverlay.SetActive(true);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            // clear the overlay and the node to run
            helpOverlay.SetActive(false);
            node = null;
        }
    }
}
#endif