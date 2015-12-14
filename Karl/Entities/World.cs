using System;
using System.Collections.Generic;
using Karl.Collision;
using Karl.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Entities
{
    public class World
    {
        private readonly List<Entity> _entitiesToAdd = new List<Entity>();
        private readonly List<Entity> _entitiesToRemove = new List<Entity>();

        protected List<Entity> Entities = new List<Entity>();
        
        public World()
        {
            Space = new Space();
            Layers = new LayerCollection();
        }

        public void AddEntity(Entity entity)
        {
            if (!_entitiesToAdd.Contains(entity))
                _entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (!_entitiesToRemove.Contains(entity))
                _entitiesToRemove.Add(entity);
        }

        public void LoadContent(ContentManager content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            Content = content;

            foreach (var entity in Entities)
                entity._LoadContent();
        }

        public void UnloadContent()
        {
            foreach (var entity in Entities)
                entity._UnloadContent();

            Content = null;
        }

        public void Update(TimeSpan elapsedTime)
        {
            BeforeUpdate(elapsedTime);

            foreach (var entity in Entities)
                entity._Update(elapsedTime);

            foreach (var entity in _entitiesToRemove)
            {
                entity._Kill(this);
                Entities.Remove(entity);
            }
            _entitiesToRemove.Clear();

            foreach (var entity in _entitiesToAdd)
            {
                Entities.Add(entity);
                entity._Spawn(this);
            }
            _entitiesToAdd.Clear();

            AfterUpdate(elapsedTime);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (var layer in Layers)
                layer.Draw(spriteBatch, camera);
        }

        protected virtual void BeforeUpdate(TimeSpan elapsedTime)
        {
        }

        protected virtual void AfterUpdate(TimeSpan elapsedTime)
        {
            Space.Update();
        }

        public ContentManager Content { get; private set; }

        public LayerCollection Layers { get; private set; }

        public Space Space { get; private set; }
    }
}
