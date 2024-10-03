using UnityEngine;

namespace AStarPathfinding
{
    public struct GridData
    {
        public readonly Vector2Int GridSize;
        public readonly Vector2 CellSize;

        public GridData(float spriteSizeX, float spriteSizeY, int gridSizeX, int gridSizeY)
        {
            GridSize = new Vector2Int(gridSizeX, gridSizeY);
            CellSize = new Vector2((spriteSizeX / GridSize.x), (spriteSizeY / GridSize.y));
        }
    }
}