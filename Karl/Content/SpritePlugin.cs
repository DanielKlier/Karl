using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Karl.Graphics;

namespace Karl.Content
{
    public class SpritePlugin : ExtensibleContentManager.IPlugin
    {
        private readonly SortedSet<string> _parsedIndexFiles = new SortedSet<string>();
        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string,Sprite>();

        public T Load<T>(ExtensibleContentManager content, string assetPath)
        {
            if (typeof(T) != typeof(Sprite))
                throw new ContentLoadException("This plugin can load Sprites only.");

            Parse(content, assetPath);

            Sprite sprite;
            if (!_sprites.TryGetValue(assetPath, out sprite))
                throw new ContentLoadException("Error loading \"" + assetPath + "\". Sprite not found.");

            return (T)((object) sprite);
        }

        private void Parse(ContentManager content, string assetPath)
        {
            var assetDir = Path.GetDirectoryName(assetPath);
            Debug.Assert(assetDir != null, "assetDir != null");
            var directory = Path.Combine(content.RootDirectory, assetDir);
            var filename = Path.Combine(directory, "index.spr");

            if (_parsedIndexFiles.Contains(filename))
                return;

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

                    var spriteName = tokens[0];
                    var spritePath = Path.Combine(assetDir, spriteName);

                    var texture = content.Load<Texture2D>(tokens[1]);
                    var sprite = new Sprite(texture);

                    if (tokens.Length == 4)
                    {
                        float originX, originY;
                        if (float.TryParse(tokens[2], NumberStyles.Float, CultureInfo.InvariantCulture, out originX) &&
                            float.TryParse(tokens[3], NumberStyles.Float, CultureInfo.InvariantCulture, out originY))
                        {
                            sprite.Origin = new Vector2(originX, originY);
                        }
                    }

                    _sprites.Add(spritePath, sprite);
                }
            }

            _parsedIndexFiles.Add(filename);
        }
    }
}
