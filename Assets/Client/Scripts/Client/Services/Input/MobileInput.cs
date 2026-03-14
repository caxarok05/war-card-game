using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class MobileInput : AbstractInput
    {
        public override bool GetInputDown()
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }

        public override bool GetInput()
        {
            return Input.touchCount > 0;
        }

        public override bool GetInputUp()
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
        }

        public override Vector3 GetScreenPosition()
        {
            if (Input.touchCount == 0)
            {
                return Vector3.zero;
            }

            return Input.GetTouch(0).position;
        }
    }
}