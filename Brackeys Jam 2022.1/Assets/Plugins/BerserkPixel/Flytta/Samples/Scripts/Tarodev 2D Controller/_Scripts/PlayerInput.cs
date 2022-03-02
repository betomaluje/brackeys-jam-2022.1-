using UnityEngine;

namespace TarodevController
{
    public class PlayerInput
    {
        public FrameInput Input { get; private set; }

        public void Update()
        {
            Input = new FrameInput {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal"),
                Push = UnityEngine.Input.GetKeyDown(KeyCode.G)
            };
        }
    }
}