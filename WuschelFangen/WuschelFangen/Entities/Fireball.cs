using Microsoft.Xna.Framework;
using System;
using Karl.Collision;
using Karl.Core;
using Karl.Core.Extensions;
using Karl.Graphics;
using Karl.Entities;

namespace WuschelFangen.Entities
{
    public class Fireball : LayerEntity
    {
        private readonly float _linearVelocity = 600;
        private readonly Timer _lifeTimer = new Timer(TimeSpan.FromSeconds(0.5));

        public Fireball()
        {
            CreateShape();
        }

        public Fireball(Layer layer) : base(layer)
        {
            CreateShape();
        }

        private void CreateShape()
        {
            Shape = new Circle(5)
            {
                Group = CollisionGroup.Projectiles,
                Mask = CollisionGroup.Enemies,
                Tag = this
            };
            Shape.Collision += OnCollision;
        }

        protected override void Spawn()
        {
            base.Spawn();

            _lifeTimer.Expired += OnLifeTimeExpired;
            _lifeTimer.Start();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = Content.Load<Sprite>("fireball");
        }

        protected override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            var deltaSeconds = (float)elapsedTime.TotalSeconds;
            var deltaTransform = Transform.Zero;
            deltaTransform.Position = Vector2.UnitX.Rotate(Transform.Rotation) * _linearVelocity;
            Transform += deltaSeconds * deltaTransform;

            _lifeTimer.Update(elapsedTime);
        }

        private void OnCollision(Space space, Shape other)
        {
            World.RemoveEntity(this);
        }

        private void OnLifeTimeExpired(object sender, EventArgs e)
        {
            World.RemoveEntity(this);
        }
    }
}
