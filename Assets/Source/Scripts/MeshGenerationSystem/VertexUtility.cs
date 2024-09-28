using System.Collections.Generic;
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

        public static HashSet<Vector2> GetBorderVerticies(Vector3[] vertices, int[] triangles)
        {
            Dictionary<(int, int), int> edgeCount = new Dictionary<(int, int), int>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                AddEdge(edgeCount, triangles[i], triangles[i + 1]);
                AddEdge(edgeCount, triangles[i + 1], triangles[i + 2]);
                AddEdge(edgeCount, triangles[i + 2], triangles[i]);
            }

            HashSet<Vector2> edgeVertices = new HashSet<Vector2>();
            foreach (var edge in edgeCount)
            {
                if (edge.Value == 1)
                {
                    edgeVertices.Add(vertices[edge.Key.Item1]);
                    edgeVertices.Add(vertices[edge.Key.Item2]);
                }
            }

            return edgeVertices;
        }

        private static void AddEdge(Dictionary<(int, int), int> edgeCount, int vertexIndex1, int vertexIndex2)
        {
            int v1 = Mathf.Min(vertexIndex1, vertexIndex2);
            int v2 = Mathf.Max(vertexIndex1, vertexIndex2);
            var edge = (v1, v2);

            if (edgeCount.ContainsKey(edge))
                edgeCount[edge]++;
            else
                edgeCount[edge] = 1;
        }
    }
}