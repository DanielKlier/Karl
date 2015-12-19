using System;
using Karl.Entities;
using Karl.Graphics;
using Microsoft.Xna.Framework;
using QuadtreeDemo.Quadtree;

namespace QuadtreeDemo.Entities
{
    public abstract class BaseEntity : LayerEntity, IBoundingBox
    {
        protected BaseEntity() { }

        protected BaseEntity(Layer layer) : base(layer) { }

        public abstract Rectangle BoundingBox { get; }

        public int Index { get; set; }
    }
}
