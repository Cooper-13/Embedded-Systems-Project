using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using Leap;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool rotateLeft;
		public bool rotateRight;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private LeapServiceProvider _leapServiceProvider;
		private bool leapMotionJump = false; // Tracks Leap Motion jump state
        private bool spacebarJump = false;   // Tracks spacebar jump state

		private void Awake()
        {
            // Get the Leap Motion provider
			// Service Provider (Desktop)
            _leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
            if (_leapServiceProvider == null)
            {
                Debug.LogError("Leap Motion Provider not found. Ensure Leap Motion is set up in the scene.");
            }
			
        }

        private void Update()
        {
            // Check for hand gesture to trigger jump
			if (_leapServiceProvider != null && _leapServiceProvider.IsConnected()) {
				CheckLeapMotionGesture();
			} else {
				leapMotionJump = false;
			}

			jump = leapMotionJump || spacebarJump;
            
        }

        private void CheckLeapMotionGesture()
        {
            // if (_leapServiceProvider == null) return;

			// if(!_leapServiceProvider.IsConnected()) return;

            // Get the current frame from the Leap Motion controller
            Frame frame = _leapServiceProvider.CurrentFrame;

            // Check if there is at least one hand detected
            if (frame.Hands.Count > 0)
            {
                Hand firstHand = frame.Hands[0]; // Get the first detected hand

                // Example gesture: Check if the hand is gripped
                if (firstHand.GrabStrength > 0.8f) // Adjust thresholds as needed
                {
                    leapMotionJump = true; // Trigger jump
					return;
                }
            }

			// reset jump if no hands are detected    
			leapMotionJump = false;
        }

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
			//spacebarJump = value.isPressed;
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnRotateLeft(InputValue value)
		{
			RotateLeftInput(value.isPressed);
		}

		public void OnRotateRight(InputValue value)
		{
			RotateRightInput(value.isPressed);
		}

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void RotateLeftInput(bool isPressed)
		{
			rotateLeft = isPressed;
		}

		public void RotateRightInput(bool isPressed)
		{
			rotateRight = isPressed;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}