using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class Graph
    {
        public readonly GridData GridData;

        private GraphNode[,] _grid;
        private Transform _meshTransform;

        public Graph(GridData gridData, Transform meshTransform)
        {
            GridData = gridData;
            _meshTransform = meshTransform;
            CreateGrid();
        }

        public GraphNode this[int i, int j]
        {
            get => _grid[i, j];
        }

        public Vector2Int ConvertWorldPositionToGridPosition(Vector2 gridPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(gridPosition.x / GridData.CellSize.x + GridData.GridSize.x / 2.0f), Mathf.RoundToInt(
                gridPosition.y / GridData.CellSize.y + GridData.GridSize.y / 2.0f));
        }

        public Vector3 ConvertGridPositionToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(_meshTransform.position.x + gridPosition.x * GridData.CellSize.x - (GridData.GridSize.x * GridData.CellSize.x) / 2.0f,
                _meshTransform.position.y + gridPosition.y * GridData.CellSize.y - (GridData.GridSize.y * GridData.CellSize.y) / 2.0f, 0f);
        }

        public GraphNode GetNodeFromPoint(Vector2Int point)
        {
            if (!IsPointWithingGrid(point))
                return null;

            return _grid[point.x, point.y];
        }

        private void CreateGrid()
        {
            _grid = new GraphNode[GridData.GridSize.x, GridData.GridSize.y];

            for (int x = 0; x < GridData.GridSize.x; x++)
            {
                for (int y = 0; y < GridData.GridSize.y; y++)
                {
                    _grid[x, y] = new GraphNode(new Vector2Int(x, y));

                    Vector2 position = ConvertGridPositionToWorldPosition(_grid[x, y].Position);

                    Vector2 cellSize = new Vector2(GridData.CellSize.x, GridData.CellSize.y);
                    Collider2D hitCollider2D = Physics2D.OverlapBox(position, cellSize, 0f);
                    _grid[x, y].IsObstacle = hitCollider2D != null;
                }
            }

            for (int x = 0; x < GridData.GridSize.x; x++)
            {
                for (int y = 0; y < GridData.GridSize.y; y++)
                {
                    _grid[x, y].SetNeighbors(GetNeighbours(x, y));
                }
            }
        }

        private bool IsPointWithingGrid(Vector2Int point)
        {
            return point.x >= 0 && point.x < GridData.GridSize.x && point.y >= 0 && point.y < GridData.GridSize.y;
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
            if (newX >= 0 && newX < GridData.GridSize.x && newY >= 0 && newY < GridData.GridSize.y && !_grid[newX, newY].IsObstacle)
            {
                neighbours.Add(_grid[newX, newY]);
            }
        }
    }
}