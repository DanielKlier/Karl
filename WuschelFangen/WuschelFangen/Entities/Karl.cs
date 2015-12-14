using Microsoft.Xna.Framework.Input;
using System;
using Karl.Collision;
using Karl.Core;
using Karl.Graphics;
using Karl.Entities;

namespace WuschelFangen.Entities
{
    public class Karl : LayerEntity
    {
        private readonly float _linearVelocity = 300;
        private KeyboardState _prevKeyboardState;

        public Karl()
        {
            CreateShape();
        }

        public Karl(Layer layer) : base(layer)
        {
            CreateShape();
        }

        private void CreateShape()
        {
            Shape = new Circle(30)
            {
                Group = CollisionGroup.Player,
                Mask = CollisionGroup.World | CollisionGroup.Enemies,
                Tag = this
            };
            Shape.Collision += OnCollision;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = Content.Load<Sprite>("karl");
        }

        private void Fire()
        {
            var fireball = new Fireball(Layer) {Transform = new Transform(66, 13)*Transform};
            World.AddEntity(fireball);
        }

        protected override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            var deltaSeconds = (float)elapsedTime.TotalSeconds;

            var currTransform = Transform;
            var deltaTransform = Transform.Zero;
            var currKeyboardState = Keyboard.GetState();

            if (currKeyboardState.IsKeyDown(Keys.Left))
                deltaTransform.Position.X -= _linearVelocity;
            if (currKeyboardState.IsKeyDown(Keys.Right))
                deltaTransform.Position.X += _linearVelocity;
            if (currKeyboardState.IsKeyDown(Keys.Up))
                deltaTransform.Position.Y -= _linearVelocity;
            if (currKeyboardState.IsKeyDown(Keys.Down))
                deltaTransform.Position.Y += _linearVelocity;

            if (_prevKeyboardState.IsKeyUp(Keys.Space) &&
                currKeyboardState.IsKeyDown(Keys.Space))
                Fire();

            currTransform += deltaSeconds * deltaTransform;
            Transform = currTransform;

            _prevKeyboardState = currKeyboardState;
        }
        
        void OnCollision(Space space, Shape other)
        {
            World.RemoveEntity(this);
        }
    }
}
