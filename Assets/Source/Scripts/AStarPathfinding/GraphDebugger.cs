using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class GraphDebugger : MonoBehaviour
    {
        private int _gridSizeX = 96;
        private int _gridSizeY = 96;

        private GraphNode[,] _grid;
        private GraphNode _startNode;
        private int _cellSize = 5;

        private Vector3 _startPositionDebug = new Vector3(1000, 0, 0);
        private Vector3 _destinationPositionDebug = new Vector3(1000, 0, 0);

        private void Start()
        {
            CreateGrid();

            FindPath(new Vector2(41, 72));
        }

        private void CreateGrid()
        {
            _grid = new GraphNode[_gridSizeX, _gridSizeY];

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    _grid[x, y] = new GraphNode(new Vector2Int(x, y));
                }
            }

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 position = ConvertGridPositionToWorldPosition(_grid[x, y]);
                    Collider2D hitCollider2D = Physics2D.OverlapCircle(position, _cellSize / 2.0f);

                    if (hitCollider2D != null)
                    {
                        _grid[x, y].IsObstacle = true;
                        continue;
                    }

                    _grid[x, y].Neighbors = GetNeighbours(x, y);
                }
            }
        }

        private List<Vector2> FindPath(Vector2 destination)
        {
            if(_grid == null)
                return null;

            Vector2Int destinationOnGrid = ConvertWorldPositionToGridPosition(destination);
            Vector2Int currentGridPosition = ConvertWorldPositionToGridPosition(transform.position);

            _destinationPositionDebug = destination;

            _startNode = GetNodeFromPoint(currentGridPosition);

            _startPositionDebug = ConvertGridPositionToWorldPosition(_startNode);

            return null;
        }

        private GraphNode GetNodeFromPoint(Vector2Int point)
        {
            if(!IsPointWithingGrid(point))
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
            return new Vector2Int(Mathf.RoundToInt(gridPosition.x / _cellSize + _gridSizeX / 2.0f),Mathf.RoundToInt(
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

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_startPositionDebug, _cellSize/2f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_destinationPositionDebug, _cellSize/2f);
        }
    }
}