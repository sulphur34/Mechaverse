using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class GraphDebugger : MonoBehaviour
    {
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        [SerializeField] private SpriteRenderer _sprite;

        private Graph _grid;
        private Vector3 _startPositionDebug = new Vector3(1000, 0, 0);
        private Vector3 _destinationPositionDebug = new Vector3(1000, 0, 0);
        private List<Vector2> _resultPath = new List<Vector2>();
        private Pathfinder _pathfinder;

        private void Start()
        {
            var transform = _sprite.transform;
            var sprite = _sprite.sprite;
            var gridData = new GridData(sprite.bounds.size.x * transform.localScale.x, sprite.bounds.size.y * transform.localScale.y, 96, 96);
            _grid = new Graph(gridData, transform);
            _pathfinder = new Pathfinder(_grid);
            _startPositionDebug = _start.position;
            _destinationPositionDebug = _end.position;
            _resultPath = _pathfinder.FindPath(_startPositionDebug, _destinationPositionDebug);
        }

        private void OnDrawGizmos()
        {
            if (_grid == null)
                return;

            for (int x = 0; x < _grid.GridData.GridSize.x; x++)
            {
                for (int y = 0; y < _grid.GridData.GridSize.y; y++)
                {
                    Gizmos.color = _grid[x, y].IsObstacle ? Color.red : Color.green;
                    Gizmos.DrawWireCube(_grid.ConvertGridPositionToWorldPosition(_grid[x, y].Position),
                        new Vector3(_grid.GridData.CellSize.x, _grid.GridData.CellSize.y, _grid.GridData.CellSize.x));
                }
            }

            foreach (var node in _pathfinder.NodesToCheck)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_grid.ConvertGridPositionToWorldPosition(node.Position), _grid.GridData.CellSize.x / 2f);
            }

            foreach (var node in _pathfinder.NodesChecked)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_grid.ConvertGridPositionToWorldPosition(node.Position), _grid.GridData.CellSize.x / 2f);
            }

            Vector3 lastPosition = Vector3.zero;
            bool isFirstStep = true;

            foreach (var point in _resultPath)
            {
                if (!isFirstStep)
                    Gizmos.DrawLine(lastPosition, point);

                lastPosition = point;
                isFirstStep = false;
            }

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_startPositionDebug, _grid.GridData.CellSize.x / 2f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_destinationPositionDebug, _grid.GridData.CellSize.x / 2f);
        }
    }
}