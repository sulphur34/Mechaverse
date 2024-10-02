using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStarPathfinding
{
    public class GraphNode
    {
        public readonly Vector2Int Position;

        private List<GraphNode> _neighbors;
        public bool IsObstacle;
        public int PickOrder;

        public GraphNode(Vector2Int position)
        {
            Position = position;
        }

        public int CostDistanceFromStart {get; private set;}
        public int CostDistanceFromGoal {get; private set;}
        public int TotalCost {get; private set;}
        public bool IsCostCalculated {get; private set;}

        public IReadOnlyList<GraphNode> Neighbors => _neighbors;

        public void SetNeighbors(List<GraphNode> neighbors)
        {
            _neighbors = neighbors;
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

        public void OrderNeighbors()
        {
            _neighbors = _neighbors.OrderBy(x => x.PickOrder).ToList();
        }
    }
}