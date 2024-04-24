using UnityEngine;

namespace Movement
{
    public readonly struct ImpulseRequest
    {
        public readonly Vector3 direction;
        public readonly float Force;

        public ImpulseRequest(Vector3 dir, float force)
        {
            direction = dir;
            Force = force;
        }

        public Vector3 GetForceVector() => Force * direction;
    }
}