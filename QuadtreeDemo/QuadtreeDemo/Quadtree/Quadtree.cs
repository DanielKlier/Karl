
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace QuadtreeDemo.Quadtree
{
    class Quadtree<TNodeDataType> where TNodeDataType : IBoundingBox
    {
        private readonly QuadtreeNode<TNodeDataType> _rootNode;

        public Quadtree()
        {
            _rootNode = new QuadtreeNode<TNodeDataType>();
        }

        public void AddObject(TNodeDataType data)
        {
            if (!_rootNode.BoundingBox.Contains(data.BoundingBox))
                _rootNode.UpdateBounds(data.BoundingBox);
            _rootNode.AddObject(data);
        }

        public void RemoveObject(TNodeDataType data)
        {
            _rootNode.RemoveObject(data);
        }
        public void UpdateObject(TNodeDataType data)
        {
            _rootNode.UpdateObject(data);
        }

        public IList<TNodeDataType> QueryRectangle(Rectangle rectangle)
        {
            var result = new HashSet<TNodeDataType>();
            _rootNode.QueryRectangle(rectangle, result);
            return result.ToList();
        }

        public void RunVisitor(Func<QuadtreeNode<TNodeDataType>, int, bool> iterateeFunc)
        {
            _rootNode.Visit(iterateeFunc, 0);
        }
    }
}
