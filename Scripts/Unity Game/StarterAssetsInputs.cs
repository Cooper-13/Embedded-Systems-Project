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

		private float leapJumpCooldown = 0.5f;   // Cooldown between jumps
		private float leapJumpTimer = 0f;        // Timer for cooldown
		private bool wasGrabbingLastFrame = false;  // Tracks grab state



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
			if (_leapServiceProvider != null && _leapServiceProvider.IsConnected())
			{
				if (leapJumpTimer > 0f)
				{
					leapJumpTimer -= Time.deltaTime;
				}

				CheckLeapMotionGesture();
			}
			else
			{
				leapMotionJump = false;
				wasGrabbingLastFrame = false;
			}

			jump = leapMotionJump || spacebarJump;

			// Reset after 1 frame
			leapMotionJump = false;
			spacebarJump = false;
		}


        private void CheckLeapMotionGesture()
		{
			Frame frame = _leapServiceProvider.CurrentFrame;

			if (frame.Hands.Count > 0)
			{
				Hand hand = frame.Hands[0];
				bool isGrabbingNow = hand.GrabStrength > 0.9f;

				// Trigger only if it's a *new* grab and cooldown expired
				if (isGrabbingNow && !wasGrabbingLastFrame && leapJumpTimer <= 0f)
				{
					leapMotionJump = true;
					leapJumpTimer = leapJumpCooldown;
				}

				wasGrabbingLastFrame = isGrabbingNow;
			}
			else
			{
				wasGrabbingLastFrame = false;
			}
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
			if (value.isPressed)
			{
				spacebarJump = true;
			}
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