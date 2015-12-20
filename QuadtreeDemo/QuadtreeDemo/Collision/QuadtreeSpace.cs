using Karl.Collision;
using Microsoft.Xna.Framework;
using QuadtreeDemo.Quadtree;

namespace QuadtreeDemo.Collision
{
    class QuadtreeSpace : Space
    {
        private readonly Quadtree<Shape> _quadtree;

        public QuadtreeSpace(Rectangle worldBounds)
        {
            _quadtree = new Quadtree<Shape>(worldBounds);
        }

        public override void AddShape(Shape shape)
        {
            base.AddShape(shape);
            _quadtree.AddObject(shape);
        }

        public override void RemoveShape(Shape shape)
        {
            base.RemoveShape(shape);
            _quadtree.RemoveObject(shape);
        }

        protected override void FindCurrentCollisions()
        {
            foreach (var shape in Shapes)
            {
                var candidateShapes = _quadtree.QueryRectangle(shape.BoundingBox);

                foreach (var candidateShape in candidateShapes)
                {
                    // only check weather the current shapes collide 
                    // if there is a collision group/mask match
                    if ((shape.Mask & candidateShape.Group) == 0 &&
                        (candidateShape.Mask & shape.Group) == 0)
                        continue;

                    // do the actual collision test
                    if (shape.Intersects(candidateShape))
                        AddCollision(shape, candidateShape);
                }
            }
        }
    }
}
