using System;
using Microsoft.Xna.Framework.Content;

namespace Karl.Entities
{
    public class Entity
    {
        protected Entity()
        {
        }

        protected Entity(World world)
        {
            World = world;
        }

        protected virtual void Spawn()
        {
        }

        protected virtual void Kill()
        {
        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        protected virtual void Update(TimeSpan elapsedTime)
        {
        }

        internal void _Spawn(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            if (World != null)
                throw new InvalidOperationException("Entity is already alive!");

            World = world;

            Spawn();
            LoadContent();

            if (Spawned != null)
                Spawned(this);
        }

        internal void _Kill(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            if (World == null)
                throw new InvalidOperationException("Entity is not alive!");
            if (World != world)
                throw new InvalidOperationException("Entity is not part of this world!");

            if (Killed != null)
                Killed(this);

            UnloadContent();
            Kill();

            World = null;
        }

        internal void _LoadContent()
        {
            if (Content != null)
                LoadContent();
        }

        internal void _UnloadContent()
        {
            if (Content != null)
                UnloadContent();
        }

        internal void _Update(TimeSpan elapsedTime)
        {
            Update(elapsedTime);
        }

        public ContentManager Content
        {
            get
            {
                return World == null ? null : World.Content;
            }
        }

        public World World { get; private set; }

        public delegate void EntityHandler(Entity entity);
        public event EntityHandler Spawned;
        public event EntityHandler Killed;
    }
}
