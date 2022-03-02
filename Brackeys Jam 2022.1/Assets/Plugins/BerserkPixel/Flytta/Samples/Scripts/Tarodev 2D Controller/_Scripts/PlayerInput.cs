using System;
using UnityEngine;

namespace TarodevController
{
    public class PlayerInput
    {
        private FrameInput Input { get; set; }

        public Action<bool, bool> OnJump = delegate { };
        public Action<float> OnMovementXChange = delegate { };
        public Action OnPushPressed = delegate {  };

        public void Update()
        {
            Input = new FrameInput {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal"),
                Push = UnityEngine.Input.GetKeyDown(KeyCode.G)
            };

            OnJump?.Invoke(Input.JumpDown, Input.JumpUp);
            
            OnMovementXChange?.Invoke(Input.X);

            if (Input.Push)
            {
                OnPushPressed?.Invoke();
            }
        }
    }
}