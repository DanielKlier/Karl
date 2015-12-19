using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuadtreeDemo.Quadtree
{
    internal class QuadtreeNode<TNodeDataType> where TNodeDataType : IBoundingBox
    {
        private delegate Rectangle BoundingBoxCalculator(Rectangle parent);

        private Rectangle CalculateBoundingBoxTopLeft(Rectangle parent)
        {
            return new Rectangle(parent.Left, parent.Top, parent.Width / 2, parent.Height / 2);
        }

        private Rectangle CalculateBoundingBoxBottomLeft(Rectangle parent)
        {
            return new Rectangle(parent.Left, parent.Top + parent.Height / 2, parent.Width / 2, parent.Height / 2);
        }

        private Rectangle CalculateBoundingBoxBottomRight(Rectangle parent)
        {
            return new Rectangle(parent.Left + parent.Width - parent.Width / 2, parent.Top + parent.Height / 2,
                    (int)Math.Ceiling(parent.Width / 2f), (int)Math.Ceiling(parent.Height / 2f));
        }

        private Rectangle CalculateBoundingBoxTopRight(Rectangle parent)
        {
            return new Rectangle(parent.Left + parent.Width - parent.Width / 2, parent.Top,
                    (int)Math.Ceiling(parent.Width / 2f), parent.Height / 2);
        }

        private const int Capacity = 10;
        private const int MaxDepth = 10;
        private Rectangle _boundingBox;
        private readonly HashSet<TNodeDataType> _data = new HashSet<TNodeDataType>();
        private readonly QuadtreeNode<TNodeDataType>_parent ;
        private QuadtreeNode<TNodeDataType>[] _children = new QuadtreeNode<TNodeDataType>[0];
        private readonly BoundingBoxCalculator _boundingBoxCalculator;
        private readonly int _depth;

        public QuadtreeNode()
        {
            
        }

        private QuadtreeNode(BoundingBoxCalculator boundingBoxCalculator, QuadtreeNode<TNodeDataType> parent, int depth)
        {
            _boundingBoxCalculator = boundingBoxCalculator;
            _parent = parent;
            _boundingBox = boundingBoxCalculator(parent._boundingBox);
            _depth = depth;
        }

        public Rectangle BoundingBox { get { return _boundingBox; } }

        public IList<TNodeDataType> Data { get { return _data.ToList(); } }

        public void AddObject(TNodeDataType data)
        {
            if (_children.Length == 0)
            {
                _data.Add(data);
                
                if (_data.Count > Capacity && _boundingBox.Width > 1 && _boundingBox.Height > 1 && _depth < MaxDepth)
                {
                    Split();
                }
            }
            else
            {
                foreach (var child in _children)
                {
                    if(data.BoundingBox.Intersects(child._boundingBox))
                        child.AddObject(data);
                }
            }
        }

        public void RemoveObject(TNodeDataType data)
        {
            throw new System.NotImplementedException();
        }
        public void UpdateObject(TNodeDataType data)
        {
            throw new System.NotImplementedException();
        }

        public void QueryRectangle(Rectangle rectangle, HashSet<TNodeDataType> queryResult)
        {
            if (_children.Length > 0)
            {
                foreach (var child in _children)
                {
                    child.QueryRectangle(rectangle, queryResult);
                }
            }
            else
            {
                foreach (var intersectingObject in _data.Where(data => data.BoundingBox.Intersects(rectangle)))
                {
                    queryResult.Add(intersectingObject);
                }
            }
        }

        public void Visit(Func<QuadtreeNode<TNodeDataType>, int, bool> iterateeFunc, int depth)
        {
            var keepGoing = iterateeFunc(this, depth);
            if (keepGoing)
            {
                foreach (var child in _children)
                {
                    child.Visit(iterateeFunc, depth + 1);
                }
            }
        }

        public void UpdateBounds(Rectangle objectBoundingBox)
        {
            if (_data.Count == 0 && _children.Length == 0)
            {
                _boundingBox = objectBoundingBox;
            }
            else
            {
                var left = Math.Min(objectBoundingBox.Left, _boundingBox.Left);
                var top = Math.Min(objectBoundingBox.Top, _boundingBox.Top);
                var width = Math.Max(objectBoundingBox.Right, _boundingBox.Right) - left;
                var height = Math.Max(objectBoundingBox.Bottom, _boundingBox.Bottom) - top;

                _boundingBox.X = left;
                _boundingBox.Y = top;
                _boundingBox.Width = width;
                _boundingBox.Height = height;

                foreach (var child in _children)
                {
                    child.InitializeBoundingBox(_boundingBox);
                }
            }
        }

        private void InitializeBoundingBox(Rectangle parentBoundingBox)
        {
            _boundingBox = _boundingBoxCalculator(parentBoundingBox);

            if (_children.Length == 0)
            {
                var dataCopy = new HashSet<TNodeDataType>(_data);
                _data.Clear();
                foreach (var data in dataCopy)
                {
                    _parent.AddObject(data);
                }
            }
            else
            {
                foreach (var child in _children)
                {
                    child.InitializeBoundingBox(_boundingBox);
                }
            }
        }

        private void Split()
        {
            CreateChildNodes();
            RedistributeData();
        }

        private void CreateChildNodes()
        {
            _children = new[]
            {
                new QuadtreeNode<TNodeDataType>(CalculateBoundingBoxTopLeft, this, _depth + 1),
                new QuadtreeNode<TNodeDataType>(CalculateBoundingBoxBottomLeft, this, _depth + 1),
                new QuadtreeNode<TNodeDataType>(CalculateBoundingBoxBottomRight, this, _depth + 1),
                new QuadtreeNode<TNodeDataType>(CalculateBoundingBoxTopRight, this, _depth + 1)
            };
        }

        private void RedistributeData()
        {
            // Re-evaluate all the data in this node and place it into the child nodes. If an object's bounding box overlaps two or more nodes, place a reference in each of the nodes
            foreach (var dataObject in _data)
            {
                foreach (var child in _children)
                {
                    if (dataObject.BoundingBox.Intersects(child._boundingBox))
                    {
                        child.AddObject(dataObject);
                    }
                }
            }

            // Objects are all moved to the new leaf nodes so we can clear this node's data
            _data.Clear();
        }
    }
}