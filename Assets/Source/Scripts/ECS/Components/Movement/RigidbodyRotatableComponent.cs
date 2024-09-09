using Data;
using UnityEngine;

namespace ECS.Components.Movement
{
    public struct RigidbodyRotatableComponent
    {
        public Rigidbody2D rigidbody;
        public RotationData rotationData;
    }
}