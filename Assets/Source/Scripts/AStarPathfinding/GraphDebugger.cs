using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStarPathfinding
{
    public class GraphDebugger : MonoBehaviour
    {
        private int _gridSizeX = 10;
        private int _gridSizeY = 10;

        private GraphNode[,] _grid;
        private GraphNode _startNode;
        private int _cellSize = 5;

        private Vector3 _startPositionDebug = new Vector3(1000, 0, 0);
        private Vector3 _destinationPositionDebug = new Vector3(1000, 0, 0);

        private List<GraphNode> _nodesToCheck = new List<GraphNode>();
        private List<GraphNode> _nodesChecked = new List<GraphNode>();

        private List<Vector2> _resultPath = new List<Vector2>();

        private void Start()
        {
            CreateGrid();
            FindPath(new Vector2(20, 20));
        }

        private void CreateGrid()
        {
            _grid = new GraphNode[_gridSizeX, _gridSizeY];

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    _grid[x, y] = new GraphNode(new Vector2Int(x, y));

                    Vector3 position = ConvertGridPositionToWorldPosition(_grid[x, y]);
                    Collider2D hitCollider2D = Physics2D.OverlapCircle(position, _cellSize / 2.0f);
                    _grid[x, y].IsObstacle = hitCollider2D != null;
                }
            }

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    _grid[x, y].Neighbors = GetNeighbours(x, y);
                }
            }
        }

        private List<Vector2> FindPath(Vector2 destination)
        {
            if (_grid == null)
                return null;

            Vector2Int destinationOnGrid = ConvertWorldPositionToGridPosition(destination);
            Vector2Int currentGridPosition = ConvertWorldPositionToGridPosition(transform.position);

            _destinationPositionDebug = destination;

            _startNode = GetNodeFromPoint(currentGridPosition);

            _startPositionDebug = ConvertGridPositionToWorldPosition(_startNode);

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

            _resultPath = CreatePath(currentGridPosition);

            return null;
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

                currentNode.Neighbors = currentNode.Neighbors.OrderBy(x => x.PickOrder).ToList();

                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (!path.Contains(neighbor) && _nodesChecked.Contains(neighbor))
                    {
                        path.Add(neighbor);
                        currentNode = neighbor;

                        break;
                    }
                }

                if(currentNode == _startNode)
                    isPathCreated = true;
            }

            foreach (var node in path)
            {
                resultPath.Add(ConvertGridPositionToWorldPosition(node));
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

        private GraphNode GetNodeFromPoint(Vector2Int point)
        {
            if (!IsPointWithingGrid(point))
                return null;

            return _grid[point.x, point.y];
        }

        private bool IsPointWithingGrid(Vector2Int point)
        {
            return point.x >= 0 && point.x < _gridSizeX && point.y >= 0 && point.y < _gridSizeY;
        }

        private List<GraphNode> GetNeighbours(int x, int y)
        {
            var neighbours = new List<GraphNode>();

            AddNeighbour(neighbours, x - 1, y);
            AddNeighbour(neighbours, x + 1, y);
            AddNeighbour(neighbours, x, y - 1);
            AddNeighbour(neighbours, x, y + 1);

            return neighbours;
        }

        private void AddNeighbour(List<GraphNode> neighbours, int newX, int newY)
        {
            if (newX >= 0 && newX < _gridSizeX && newY >= 0 && newY < _gridSizeY && !_grid[newX, newY].IsObstacle)
            {
                neighbours.Add(_grid[newX, newY]);
            }
        }

        private Vector3 ConvertGridPositionToWorldPosition(GraphNode gridNode)
        {
            return new Vector3(gridNode.Position.x * _cellSize - (_gridSizeX * _cellSize) / 2.0f,
                gridNode.Position.y * _cellSize - (_gridSizeY * _cellSize) / 2.0f, 0f);
        }

        private Vector2Int ConvertWorldPositionToGridPosition(Vector2 gridPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(gridPosition.x / _cellSize + _gridSizeX / 2.0f), Mathf.RoundToInt(
                gridPosition.y / _cellSize + _gridSizeY / 2.0f));
        }

        private void OnDrawGizmos()
        {
            if (_grid == null)
                return;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Gizmos.color = _grid[x, y].IsObstacle ? Color.red : Color.green;
                    Gizmos.DrawWireCube(ConvertGridPositionToWorldPosition(_grid[x, y]),
                        new Vector3(_cellSize, _cellSize, _cellSize));
                }
            }

            foreach (var node in _nodesToCheck)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(node), _cellSize / 2f);
            }

            foreach (var node in _nodesChecked)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(node), _cellSize / 2f);
            }

            Vector3 lastPosition = Vector3.zero;
            bool isFirstStep = true;

            foreach (var point in _resultPath)
            {
                if(!isFirstStep)
                    Gizmos.DrawLine(lastPosition, point);

                lastPosition = point;
                isFirstStep = false;
            }

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_startPositionDebug, _cellSize / 2f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_destinationPositionDebug, _cellSize / 2f);
        }
    }
}