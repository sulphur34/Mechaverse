using UnityEngine;

namespace MeshGenerationSystem
{
    public static class VertexUtility
    {
        public static Vector3 ComputeNormal(VertexData vertexA, VertexData vertexB, VertexData vertexC)
        {
            Vector3 sideL = vertexB.Position - vertexA.Position;
            Vector3 sideR = vertexC.Position - vertexA.Position;

            Vector3 normal = Vector3.Cross(sideL, sideR);

            return normal.normalized;
        }
    }
}