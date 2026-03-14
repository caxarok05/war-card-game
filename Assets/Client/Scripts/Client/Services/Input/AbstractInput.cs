using UnityEngine;

namespace Client.Scripts.Client
{
    public abstract class AbstractInput
    {
        protected Camera Camera { get; private set; }

        public virtual void Init()
        {
            Camera = Camera.main;
        }

        public abstract bool GetInputDown();
        public abstract bool GetInput();
        public abstract bool GetInputUp();
        public abstract Vector3 GetScreenPosition();

        public virtual Vector3 GetWorldPosition(float distanceFromCamera = 0f)
        {
            Vector3 screenPosition = GetScreenPosition();
            screenPosition.z = distanceFromCamera <= 0f
                ? Mathf.Abs(Camera.transform.position.z)
                : distanceFromCamera;

            return Camera.ScreenToWorldPoint(screenPosition);
        }
    }
}