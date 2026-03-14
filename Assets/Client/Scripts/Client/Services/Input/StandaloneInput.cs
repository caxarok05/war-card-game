using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class StandaloneInput : AbstractInput
    {
        public override bool GetInputDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public override bool GetInput()
        {
            return Input.GetMouseButton(0);
        }

        public override bool GetInputUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        public override Vector3 GetScreenPosition()
        {
            return Input.mousePosition;
        }
    }
}