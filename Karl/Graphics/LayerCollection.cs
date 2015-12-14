using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Karl.Graphics
{
    public class LayerCollection : List<Layer>
    {
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (Layer layer in this)
            {
                if (layer == null)
                    continue;

                layer.Draw(spriteBatch, camera);
            }
        }        
    }
}
