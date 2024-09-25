namespace SpriteDestructionSystem
{
    public class Quad
    {
        private int _xAdress;
        private int _yAdress;
        private int _width;
        private int _height;

        private bool _hasChildren;
        private Quad[] _children;

        public Quad(int xAdress, int yAdress, int width, int height)
        {
            _xAdress = xAdress;
            _yAdress = yAdress;
            _width = width;
            _height = height;
        }

        public int XAdress => _xAdress;
        public int YAdress => _yAdress;
        public int Width => _width;
        public int Height => _height;
        public bool HasChildren => _hasChildren;

        public Quad[] Subdivide()
        {
            int halfWidth = _width / 2;
            int halfHeight = _height / 2;
            int widthOddFix = _width % 2;
            int heightOddFix = _height % 2;
            int maxHeight = halfHeight + heightOddFix;
            int maxWidth = halfWidth + widthOddFix;

            _children = new[]
            {
                new Quad(_xAdress, _yAdress, halfWidth, halfHeight),
                new Quad(_xAdress + halfWidth, _yAdress, maxWidth, halfHeight),
                new Quad(_xAdress, _yAdress + halfHeight, halfWidth, maxHeight),
                new Quad(_xAdress + halfWidth, _yAdress + halfHeight, maxWidth, maxHeight)
            };

            _hasChildren = true;
            return _children;
        }

        public bool IsDivisible()
        {
            return  _width > 1 && _height > 1;
        }

        public bool TryGetChildren(out Quad[] children)
        {
            if (_hasChildren)
            {
                children = _children;
                return true;
            }

            children = null;
            return false;
        }
    }
}