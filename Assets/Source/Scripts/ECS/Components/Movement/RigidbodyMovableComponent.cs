using Data;
using UnityEngine;

namespace Components
{
    public struct RigidbodyMovableComponent
    {
        public Rigidbody2D rigidbody;
        public Transform transform;
        public MovingData movingData;
        public Vector2 velocity;
        public bool isMoving;
    }
}