using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class GraphNode
    {
        public readonly Vector2Int Position;

        public List<GraphNode> Neighbors;

        public bool IsObstacle;

        public int CostDistanceFromStart;
        public int CostDistanceFromGoal;
        public int TotalCost;
        public int PickOrder;
        public int IsCostCalculated;

        public GraphNode(Vector2Int position)
        {
            Position = position;
        }
    }
}