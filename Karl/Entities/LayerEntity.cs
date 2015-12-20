using System;
using Karl.Collision;
using Karl.Core;
using Karl.Graphics;

namespace Karl.Entities
{
    public class LayerEntity : Entity
    {
        private Layer _layer;
        private Shape _shape;
        private readonly SpriteInstance _spriteInstance = new SpriteInstance();

        protected LayerEntity()
        {
        }

        protected LayerEntity(Layer layer)
        {
            _layer = layer;
        }

        protected override void Spawn()
        {
            base.Spawn();

            AddShape();
            AddSpriteInstance();

            UpdateShape();
        }

        protected override void Kill()
        {
            RemoveSpriteInstance();
            RemoveShape();

            base.Kill();
        }

        protected override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            UpdateShape();
        }

        private void AddShape()
        {
            if (World != null && _shape != null)
                World.Space.AddShape(_shape);
        }

        private void RemoveShape()
        {
            if (World != null && _shape != null)
                World.Space.RemoveShape(_shape);
        }

        private void UpdateShape()
        {
            if (_shape != null)
                _shape.Transform = Transform;
        }

        private void AddSpriteInstance()
        {
            if (World != null && _layer != null)
                _layer.Sprites.Add(_spriteInstance);
        }

        private void RemoveSpriteInstance()
        {
            if (World != null && _layer != null)
                _layer.Sprites.Remove(_spriteInstance);
        }

        public Layer Layer
        {
            get { return _layer; }
            set
            {
                RemoveSpriteInstance();
                _layer = value;
                AddSpriteInstance();
            }
        }

        public Shape Shape
        {
            get { return _shape; }
            set
            {
                RemoveShape();
                _shape = value;
                AddShape();
            }
        }

        public Sprite Sprite
        {
            get { return _spriteInstance.Sprite; }
            set { _spriteInstance.Sprite = value; }
        }

        public Transform Transform
        {
            get { return _spriteInstance.Transform; }
            set { _spriteInstance.Transform = value; }
        }
    }
}
