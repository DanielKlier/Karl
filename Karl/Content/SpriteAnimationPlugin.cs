using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Karl.Graphics;

namespace Karl.Content
{
    public class SpriteAnimationPlugin : ExtensibleContentManager.IPlugin
    {
        private readonly Dictionary<string, SpriteAnimation> _animations = new Dictionary<string, SpriteAnimation>();

        public T Load<T>(ExtensibleContentManager content, string assetPath)
        {
            if (typeof(T) != typeof(SpriteAnimation))
                throw new ContentLoadException("This plugin can load SpriteAnimations only.");

            Parse(content, assetPath);

            SpriteAnimation animation;
            if (!_animations.TryGetValue(assetPath, out animation))
                throw new ContentLoadException("Error loading \"" + assetPath + "\". SpriteAnimation not found.");

            return (T)((object) animation);
        }

        private void Parse(ContentManager content, string assetPath)
        {
            var assetDir = Path.GetDirectoryName(assetPath);

            Debug.Assert(assetDir != null, "assetDir != null");

            if (_animations.ContainsKey(assetPath))
                return;
            
            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            
            var directory = Path.Combine(content.RootDirectory, assetDir);
            var filename = Path.Combine(directory, assetName + ".ani");

            var animation = new SpriteAnimation();
            using (var reader = new StreamReader(filename))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    var tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // skip empty and comment lines
                    if (tokens.Length < 1 || tokens[0].StartsWith("#"))
                        continue;

                    if (tokens.Length < 2)
                        throw new InvalidDataException("Invalid line in " + filename);

                    SpriteAnimationFrame frame;

                    if (!TimeSpan.TryParse(tokens[0], out frame.Time))
                        throw new InvalidDataException("Invalid time in " + filename);

                    frame.Sprite = content.Load<Sprite>(tokens[1]);

                    if (!animation.AddFrame(frame))
                        throw new InvalidDataException("Invalid frame in " + filename);
                }
            }

            _animations.Add(assetPath, animation);
        }
    }
}
