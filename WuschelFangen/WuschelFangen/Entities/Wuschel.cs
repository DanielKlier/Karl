using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Karl.Collision;
using Karl.Core;
using Karl.Graphics;
using Karl.Entities;

namespace WuschelFangen.Entities
{
    public class Wuschel : LayerEntity
    {
        private const float LinearVelocity = 100;
        private Shape _sensor;
        private Vector2 _targetPosition;
        private SpriteAnimation _aniIdle, _aniAttack;
        private SpriteAnimationState _animationState;
        private SoundEffect _explosion;

        public Wuschel()
        {
            CreateShapes();
        }

        public Wuschel(Layer layer) : base(layer)
        {
            CreateShapes();
        }

        private void CreateShapes()
        {
            Shape = new Circle(30)
            {
                Group = CollisionGroup.Enemies,
                Mask = CollisionGroup.Projectiles | CollisionGroup.Player,
                Tag = this
            };
            Shape.Collision += OnCollision;

            _sensor = new Circle(200)
            {
                Group = CollisionGroup.None,
                Mask = CollisionGroup.Player
            };
            _sensor.Collision += OnSensorCollision;
            _sensor.Separation += OnSensorSeparation;
        }

        protected override void Spawn()
        {
            base.Spawn();

            _targetPosition = Transform.Position;
            _sensor.Transform = Transform;

            World.Space.Shapes.Add(_sensor);
        }

        protected override void Kill()
        {
            World.Space.Shapes.Remove(_sensor);

            base.Kill();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _aniIdle = Content.Load<SpriteAnimation>("wuschel\\idle");
            _aniAttack = Content.Load<SpriteAnimation>("wuschel\\attack");
            _explosion = Content.Load<SoundEffect>("sfx\\explosion");

            _animationState = new SpriteAnimationState(_aniIdle) {Looping = true};
            Sprite = _animationState.Sprite;
        }

        protected override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            var deltaSeconds = (float)elapsedTime.TotalSeconds;
            var deltaTransform = Transform.Zero;

            var dir = _targetPosition - Transform.Position;
            var dist = dir.Length();

            if (dist < 0.5f)
            {
                dir = Vector2.Zero;
                _animationState.Animation = _aniIdle;
            }
            else
            {               
                dir /= dist;
                _animationState.Animation = _aniAttack;
            }

            deltaTransform.Position = dir * MathHelper.Min(dist, deltaSeconds * LinearVelocity);
            Transform += deltaTransform;

            _animationState.Update(elapsedTime);
            var sprWuschel = _animationState.Sprite;
            if (deltaTransform.Position.X > 0)
                sprWuschel.Effects = SpriteEffects.FlipHorizontally;
            if (deltaTransform.Position.X < 0)
                sprWuschel.Effects = SpriteEffects.None;
            Sprite = sprWuschel;

            _sensor.Transform = Transform;
        }

        private void OnCollision(Space space, Shape other)
        {
            _explosion.Play();
            World.RemoveEntity(this);
        }

        private void OnSensorCollision(Space space, Shape other)
        {
            // a player is near! Lets hunt him!
            _targetPosition = other.Transform.Position;
        }

        private void OnSensorSeparation(Space space, Shape other)
        {
            // wuschel lost the player. Stay where it is right now.
            _targetPosition = Transform.Position;
        }
    }
}
