using UnityEngine;

namespace Utilitiy
{
    public static class CollidersInteractionsRemover
    {
        public static void Remove(Collider2D[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                for (int j = 0; j < colliders.Length; j++)
                {
                    Physics2D.IgnoreCollision(colliders[i], colliders[j]);
                }
            }
        }
    }
}