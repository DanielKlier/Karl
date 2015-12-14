using Karl.Core;

namespace Karl.Graphics
{
    public class SpriteInstance
    {
        public Transform Transform;
        public Sprite Sprite;
        
        public SpriteInstance()
        {
            Transform = Transform.Identity;
        }

        public SpriteInstance(Sprite sprite) : this()
        {
            Sprite = sprite;
        }

        public SpriteInstance(Transform transform, Sprite sprite)
        {
            Transform = transform;
            Sprite = sprite;
        }
    }
}
