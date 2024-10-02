using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AStarPathfinding
{
    public class Pathfinder
    {
        private Graph _grid;

        private GraphNode _startNode;
        private List<GraphNode> _nodesToCheck = new List<GraphNode>();
        private List<GraphNode> _nodesChecked = new List<GraphNode>();


        public Pathfinder(Graph grid)
        {
            _grid = grid;
        }

        public IReadOnlyList<GraphNode> NodesToCheck => _nodesToCheck;
        public IReadOnlyList<GraphNode> NodesChecked => _nodesChecked;

        public List<Vector2> FindPath(Vector2 position, Vector2 destination)
        {
            if (_grid == null)
                return null;

            Vector2Int destinationOnGrid = _grid.ConvertWorldPositionToGridPosition(destination);
            Vector2Int currentGridPosition = _grid.ConvertWorldPositionToGridPosition(position);
            //_destinationPositionDebug = destination;
            _startNode = _grid.GetNodeFromPoint(currentGridPosition);
            //_startPositionDebug = _grid.ConvertGridPositionToWorldPosition(_startNode.Position);

            GraphNode currentNode = _startNode;

            bool isDoneFindingPath = false;
            int pickOrder = 1;

            while (!isDoneFindingPath)
            {
                _nodesToCheck.Remove(currentNode);
                currentNode.PickOrder = pickOrder;
                pickOrder++;
                _nodesChecked.Add(currentNode);

                if (currentNode.Position == destinationOnGrid)
                {
                    isDoneFindingPath = currentNode.Position == destinationOnGrid;
                    break;
                }

                CalculateCostForNodeAndNeighbours(currentNode, currentGridPosition, destinationOnGrid);

                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (_nodesChecked.Contains(neighbor))
                        continue;

                    if (_nodesToCheck.Contains(neighbor))
                        continue;

                    _nodesToCheck.Add(neighbor);
                }

                _nodesToCheck = _nodesToCheck
                    .OrderBy(x => x.TotalCost)
                    .ThenBy(y => y.CostDistanceFromGoal)
                    .ToList();

                if (_nodesToCheck.Count == 0)
                    return null;

                currentNode = _nodesToCheck[0];
            }

            return CreatePath(currentGridPosition);
        }

        private List<Vector2> CreatePath(Vector2Int currentGridPosition)
        {
            List<Vector2> resultPath = new List<Vector2>();
            List<GraphNode> path = new List<GraphNode>();
            bool isPathCreated = false;

            _nodesChecked.Reverse();

            GraphNode currentNode = _nodesChecked[0];
            path.Add(currentNode);

            int attempts = 0;

            while (!isPathCreated)
            {
                attempts++;

                if (attempts > 1000)
                    break;

                currentNode.OrderNeighbors();

                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (!path.Contains(neighbor) && _nodesChecked.Contains(neighbor))
                    {
                        path.Add(neighbor);
                        currentNode = neighbor;

                        break;
                    }
                }

                if (currentNode == _startNode)
                    isPathCreated = true;
            }

            foreach (var node in path)
            {
                resultPath.Add(_grid.ConvertGridPositionToWorldPosition(node.Position));
            }

            resultPath.Reverse();

            return resultPath;
        }

        private void CalculateCostForNodeAndNeighbours(GraphNode node, Vector2Int position, Vector2Int destination)
        {
            node.CalculateCost(position, destination);

            foreach (var neighbor in node.Neighbors)
            {
                neighbor.CalculateCost(position, destination);
            }
        }
    }
}