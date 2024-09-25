using UnityEngine;

namespace SpriteDestructionSystem
{
    public class DestructableSprite : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _texture2D;

        private SpriteData _spriteData;
        private QuadTree _quadTree;

        private void OnDrawGizmosSelected()
        {
            if (_quadTree != null)
            {
                _spriteData.DrawGizmos(10f);
                // _quadTree.DrawGizmos();
            }
        }

        public void GenerateSprite()
        {
            _spriteData = new SpriteData(_texture2D);
            _quadTree = new QuadTree(_spriteData);
        }
    }
}