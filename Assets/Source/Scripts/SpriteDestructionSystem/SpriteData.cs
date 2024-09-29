using System;
using MeshGenerationSystem;
using UnityEngine;

namespace SpriteDestructionSystem
{
    public class SpriteData
    {
        private readonly int _height;
        private readonly int _width;

        private bool[,] _points;

        public SpriteData(Texture2D texture)
        {
            _height = texture.height;
            _width = texture.width;
            _points = new bool[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _points[i, j] = texture.GetPixel(i,j).a > Single.Epsilon ;
                }
            }
        }

        public int Width => _width;
        public int Height => _height;

        public void DrawGizmos(float pixelsPerUnit)
        {
            if (_points != null)
            {
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _height; y++)
                    {
                        Vector3 position = new Vector3(x, y, 0) / pixelsPerUnit;
                        Gizmos.color = _points[x, y] ? Color.red : Color.blue;

                        Gizmos.DrawSphere(position, 0.5f / pixelsPerUnit);
                    }
                }
            }
        }

        public void DestroySprite(int centerX, int centerY, int range)
        {
            int xOrigin = Mathf.Max(0, centerX - range);
            int xEnd =  Mathf.Min(_width-1,centerX + range);
            int yOrigin = Mathf.Max(0,centerY - range);
            int yEnd =  Mathf.Min(_height-1, centerY + range);

            for (int x = xOrigin; x < xEnd; x++)
            {
                for (int y = yOrigin; y < yEnd; y++)
                {
                    _points[x,y] = false;
                }
            }
        }

        public bool IsQuadUniform(Quad quad)
        {
            bool isOriginSolid = _points[quad.XAdress, quad.YAdress];

            for (int xDelta = 0; xDelta <= quad.Width-1; xDelta++)
            {
                for (int yDelta = 0; yDelta <= quad.Height-1; yDelta++)
                {
                    bool isOtherSolid = _points[quad.XAdress + xDelta, quad.YAdress + yDelta];

                    if (isOriginSolid != isOtherSolid)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsSolid(Quad quad)
        {
            return _points[quad.XAdress, quad.YAdress];
        }
    }
}