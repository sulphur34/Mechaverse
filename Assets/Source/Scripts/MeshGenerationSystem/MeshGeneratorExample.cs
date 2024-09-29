using UnityEditor;
using UnityEngine;

namespace MeshGenerationSystem
{
    public class MeshGeneratorExample : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;

        private void OnDrawGizmosSelected()
        {
            Mesh mesh = _meshFilter.sharedMesh;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (int index = 0; index < mesh.vertices.Length; index++)
            {
                Vector3 vertex = mesh.vertices[index];
                Vector3 normal = mesh.normals[index];
                Vector3 normalColor = (normal + Vector3.one) / 2f;
                Gizmos.color = new Color(normalColor.x, normalColor.y, normalColor.z);
                Gizmos.DrawLine(vertex, vertex + normal);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vertex, 0.1f);
            }
        }

        public void GenerateMesh()
        {
            MeshConstructionHelper meshConstructionHelper = new MeshConstructionHelper();
            VertexData vertexA = new VertexData()
            {
                Position = new Vector3(0, 0, 0),
                Uv = new Vector2(0, 0)
            };
            VertexData vertexB = new VertexData()
            {
                Position = new Vector3(0, 1, 0),
                Uv = new Vector2(0, 1)
            };
            VertexData vertexC = new VertexData()
            {
                Position = new Vector3(1, 1, 0),
                Uv = new Vector2(1, 1)
            };
            VertexData vertexD = new VertexData()
            {
                Position = new Vector3(1, 0, 0),
                Uv = new Vector2(1, 0)
            };
            VertexData vertexE = new VertexData()
            {
                Position = new Vector3(0, 0, 1),
                Uv = new Vector2(1, 0)
            };
            VertexData vertexF = new VertexData()
            {
                Position = new Vector3(0, 1, 1),
                Uv = new Vector2(0, 1)
            };
            VertexData vertexG = new VertexData()
            {
                Position = new Vector3(1, 1, 1),
                Uv = new Vector2(1, 1)
            };
            VertexData vertexH = new VertexData()
            {
                Position = new Vector3(1, 0, 1),
                Uv = new Vector2(1, 0)
            };
            CreateQuad(vertexA, vertexB, vertexC, vertexD, ref meshConstructionHelper);
            CreateQuad(vertexF, vertexE, vertexH, vertexG, ref meshConstructionHelper);
            CreateQuad(vertexC, vertexB, vertexF, vertexG, ref meshConstructionHelper);
            CreateQuad(vertexE, vertexF,vertexB, vertexA,  ref meshConstructionHelper);
            CreateQuad(vertexD, vertexC,vertexG, vertexH,  ref meshConstructionHelper);
            CreateQuad(vertexE, vertexA,vertexD, vertexH,  ref meshConstructionHelper);

            _meshFilter.sharedMesh = meshConstructionHelper.ConstructMesh();
        }

        private void CreateQuad(
            VertexData vertexA,
            VertexData vertexB,
            VertexData vertexC,
            VertexData vertexD,
            ref MeshConstructionHelper meshConstructionHelper)
        {
            meshConstructionHelper.AddMeshSection(vertexA, vertexB, vertexC);
            meshConstructionHelper.AddMeshSection(vertexC, vertexD, vertexA);
        }
    }
}