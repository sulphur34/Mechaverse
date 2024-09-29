using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteDestructionSystem
{
    public partial class QuadTree : IEnumerable<Quad>
    {
        private SpriteData _spriteData;
        private Quad _rootQuad;

        public QuadTree(SpriteData spriteData)
        {
            _spriteData = spriteData;
            _rootQuad = new Quad(0,0,spriteData.Width,spriteData.Height);

            InitializeQuad(_rootQuad);
        }

        private void InitializeQuad(Quad rootQuad)
        {
            Stack<Quad> quadsToInitialize = new Stack<Quad>();
            quadsToInitialize.Push(rootQuad);

            while (quadsToInitialize.TryPop(out Quad quad))
            {
                if (!_spriteData.IsQuadUniform(quad))
                {
                    Quad[] children = quad.Subdivide();

                    foreach (var child in children)
                    {
                        if (child.IsDivisible())
                        {
                            quadsToInitialize.Push(child);
                        }
                    }
                }
            }
        }

        public IEnumerator<Quad> GetEnumerator()
        {
            return new QuadEnumerator(_rootQuad);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}