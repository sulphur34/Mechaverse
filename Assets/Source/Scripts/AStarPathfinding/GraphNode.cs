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
        public bool IsCostCalculated;

        public GraphNode(Vector2Int position)
        {
            Position = position;
        }

        public void CalculateCost(Vector2Int position, Vector2Int destination)
        {
            if(IsCostCalculated)
                return;

            CostDistanceFromStart = Mathf.Abs(Position.x - position.x) + Mathf.Abs(Position.y - position.y);
            CostDistanceFromGoal = Mathf.Abs(Position.x - destination.x) + Mathf.Abs(Position.y - destination.y);

            TotalCost = CostDistanceFromStart + CostDistanceFromGoal;

            IsCostCalculated = true;
        }
    }
}