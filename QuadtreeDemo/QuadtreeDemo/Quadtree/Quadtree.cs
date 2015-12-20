
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Karl.Core;
using Microsoft.Xna.Framework;

namespace QuadtreeDemo.Quadtree
{
    class Quadtree<TNodeDataType> where TNodeDataType : IBoundingBox
    {
        private readonly QuadtreeNode<TNodeDataType> _rootNode;

        public Quadtree(Rectangle bounds)
        {
            _rootNode = new QuadtreeNode<TNodeDataType>(bounds);
        }

        public void AddObject(TNodeDataType data)
        {
            /*if (!_rootNode.BoundingBox.Contains(data.BoundingBox))
                _rootNode.UpdateBounds(data.BoundingBox);*/
            _rootNode.AddObject(data);
        }

        public void RemoveObject(TNodeDataType data)
        {
            var affectedNodes = FindNodesForRectangle(data.BoundingBox);
            foreach (var affectedNode in affectedNodes)
            {
                affectedNode.RemoveObjectDirect(data);
            }
        }

        private QuadtreeNode<TNodeDataType>[] FindNodesForRectangle(Rectangle boundingBox)
        {
            var result = new HashSet<QuadtreeNode<TNodeDataType>>();
            _rootNode.FindNodesForRectangle(boundingBox, result);
            return result.ToArray();
        }

        public void UpdateObject(TNodeDataType data)
        {
            _rootNode.UpdateObject(data);
        }

        public TNodeDataType[] QueryRectangle(Rectangle rectangle)
        {
            var result = new HashSet<TNodeDataType>();
            _rootNode.QueryRectangle(rectangle, result);
            return result.ToArray();
        }

        public void RunVisitor(Func<QuadtreeNode<TNodeDataType>, int, bool> iterateeFunc)
        {
            _rootNode.Visit(iterateeFunc, 0);
        }
    }
}
