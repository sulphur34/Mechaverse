using Data;
using UnityEngine;

namespace ECS.Components.Movement
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