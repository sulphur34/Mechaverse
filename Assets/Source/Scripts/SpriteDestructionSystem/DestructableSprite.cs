using MeshGenerationSystem;
using UnityEngine;

namespace SpriteDestructionSystem
{
    public class DestructableSprite : MonoBehaviour
    {
        [SerializeField] private Texture2D _texture2D;
        [SerializeField] private float _pixelPerUnit = 10f;
        [SerializeField] private MeshFilter _meshFilter;

        private SpriteData _spriteData;
        private QuadTree _quadTree;

        private void Awake()
        {
            GenerateSprite();
            GetComponent<MeshCollider>().sharedMesh = _meshFilter.sharedMesh;
        }

        private void OnDrawGizmosSelected()
        {
            if (_quadTree != null)
            {
                // _spriteData.DrawGizmos(10f);
                _quadTree.DrawGizmos();
            }
        }

        public void GenerateSprite()
        {
            _spriteData = new SpriteData(_texture2D);
            _quadTree = new QuadTree(_spriteData);
            ConstructMeshes();
        }

        private void ConstructMeshes()
        {
            MeshConstructionHelper meshConstructionHelper = new MeshConstructionHelper();

            foreach (Quad quad in _quadTree)
            {
                if (!quad.HasChildren)
                {
                    bool isQuadSolid = _spriteData.IsSolid(quad);

                    if (isQuadSolid)
                    {
                        ConstructQuad(quad, meshConstructionHelper);
                    }
                }
            }

            _meshFilter.mesh = meshConstructionHelper.ConstructMesh();
        }

        private void ConstructQuad(Quad quad, MeshConstructionHelper meshConstructionHelper)
        {
            VertexData[] vertices = GetQuadCorners(quad);
            meshConstructionHelper.AddMeshSection(vertices[0], vertices[2], vertices[1]);
            meshConstructionHelper.AddMeshSection(vertices[0], vertices[3], vertices[2]);
        }

        private VertexData ImageCoordinatesToVertex(int x, int y)
        {
            float uvX = (x / (float)_texture2D.width);
            float uvY = (y / (float)_texture2D.height);

            Vector3 position = new Vector3(x - (_texture2D.width / 2f), y - (_texture2D.height / 2f), 0) /
                               _pixelPerUnit;

            return new VertexData()
            {
                Position = position,
                Uv = new Vector2(uvX, uvY),
                Normal = transform.forward,
            };
        }

        private VertexData[] GetQuadCorners(Quad quad)
        {
            VertexData[] verticies = new[]
            {
                ImageCoordinatesToVertex(quad.XAdress, quad.YAdress),
                ImageCoordinatesToVertex(quad.XAdress + quad.Width, quad.YAdress),
                ImageCoordinatesToVertex(quad.XAdress + quad.Width, quad.YAdress + quad.Height),
                ImageCoordinatesToVertex(quad.XAdress, quad.YAdress + quad.Height),
            };

            return verticies;
        }
    }
}