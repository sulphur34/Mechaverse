using Data;
using UnityEngine;

namespace ECS.Components.Movement
{
    public struct RigidbodyInstantMovableComponent
    {
        public Rigidbody2D rigidbody;
        public Vector2 velocity;
    }
}