using UnityEngine;

namespace ECS.Components
{
    public struct ProjectileComponent
    {
        public Rigidbody2D rigidbody2D;
        public Collider2D collider;
    }
}