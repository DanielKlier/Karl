using System.Collections.Generic;
using System.Linq;

namespace Karl.Collision
{
    public class Space
    {
        public struct Collision
        {
            public Collision(Shape shapeA, Shape shapeB)
            {
                ShapeA = shapeA;
                ShapeB = shapeB;
            }

            public Shape ShapeA;
            public Shape ShapeB;
        }

        private readonly List<Shape> _shapes = new List<Shape>();
        private List<Collision> _currCollisions = new List<Collision>();
        private List<Collision> _lastCollisions = new List<Collision>();

        public bool Collides(Shape testShape, bool generateEvents = false)
        {
            var collides = false;

            foreach (var spaceShape in _shapes.Where(spaceShape => (testShape.Mask & spaceShape.Group) != 0).Where(testShape.Intersects))
            {
                // found a collision!
                collides = true;

                // generate events if desired
                if (generateEvents)
                    testShape.OnCollision(this, spaceShape);                    
                // otherwise break
                else
                    break;
            }

            // return weather there was one (or more) collision(s).
            return collides;
        }

        public void Update()
        {
            FindCurrentCollisions();
            TriggerCollisionEvents();
            TriggerSeperationEvents();
            ClearCurrentCollisions();
        }

        public virtual void AddShape(Shape shape)
        {
            Shapes.Add(shape);
        }

        public virtual void RemoveShape(Shape shape)
        {
            Shapes.Remove(shape);
        }

        private void ClearCurrentCollisions()
        {
            var tempCollisions = _lastCollisions;
            _lastCollisions = _currCollisions;
            _currCollisions = tempCollisions;
            _currCollisions.Clear();
        }

        private void TriggerSeperationEvents()
        {
            // trigger seperation event for every shape which was colliding 
            // during the last update but not colliding during this update anymore.
            foreach (var collision in _lastCollisions)
            {
                if ((collision.ShapeA.Mask & collision.ShapeB.Group) != 0)
                    collision.ShapeA.OnSeparation(this, collision.ShapeB);

                if ((collision.ShapeB.Mask & collision.ShapeA.Group) != 0)
                    collision.ShapeB.OnSeparation(this, collision.ShapeA);
            }
        }

        private void TriggerCollisionEvents()
        {
            // trigger collision event for every currently colliding shape.
            foreach (var collision in _currCollisions)
            {
                // remove collision from the last collisions list 
                // if the same two shapes are still colliding.
                if (_lastCollisions.Contains(collision))
                    _lastCollisions.Remove(collision);

                if ((collision.ShapeA.Mask & collision.ShapeB.Group) != 0)
                    collision.ShapeA.OnCollision(this, collision.ShapeB);

                if ((collision.ShapeB.Mask & collision.ShapeA.Group) != 0)
                    collision.ShapeB.OnCollision(this, collision.ShapeA);
            }
        }

        protected virtual void FindCurrentCollisions()
        {
            // find current collisions
            for (int numShapes = _shapes.Count, idxShapeA = 0; idxShapeA < numShapes; ++idxShapeA)
            {
                for (var idxShapeB = idxShapeA + 1; idxShapeB < numShapes; ++idxShapeB)
                {
                    var shapeA = _shapes[idxShapeA];
                    var shapeB = _shapes[idxShapeB];

                    // only check weather the current shapes collide 
                    // if there is a collision group/mask match
                    if ((shapeA.Mask & shapeB.Group) == 0 &&
                        (shapeB.Mask & shapeA.Group) == 0)
                        continue;

                    // do the actual collision test
                    if (shapeA.Intersects(shapeB))
                        AddCollision(shapeA, shapeB);
                }
            }
        }

        protected void AddCollision(Shape shapeA, Shape shapeB)
        {
            _currCollisions.Add(new Collision(shapeA, shapeB));
        }

        protected IList<Shape> Shapes
        {
            get { return _shapes; }
        }
    }
}
