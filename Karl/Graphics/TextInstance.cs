using Karl.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics
{
    public class TextInstance
    {
        private SpriteFont _font;
        private Vector2 _origin;
        private string _text;
        private Align _align;

        public TextInstance(string text, Transform transform, Align align, Vector2 origin, Color color)
        {
            _text = text;
            Transform = transform;
            _align = align;
            _origin = origin;
            Color = color;
        }

        public TextInstance()
        {
            Color = Color.White;
        }

        public TextInstance(string text)
            : this()
        {
            _text = text;
        }

        public TextInstance(string text, Align alignment)
            : this()
        {
            _text = text;
            _align = alignment;
        }

        public TextInstance(string text, Transform transform, Align alignment = Align.None)
            : this()
        {
            Transform = transform;
            _text = text;
            _align = alignment;
        }

        public TextInstance(SpriteFont font)
            : this()
        {
            _font = font;
        }

        public TextInstance(SpriteFont font, string text)
            : this()
        {
            _font = font;
            _text = text;
        }

        public TextInstance(SpriteFont font, string text, Transform transform, Align align = Align.None)
            : this()
        {
            _font = font;
            _text = text;
            _align = align;
            Transform = transform;
            UpdateOrigin();
        }

        public TextInstance(SpriteFont font, string text, Transform transform, Vector2 origin, Color color)
        {
            _font = font;
            _text = text;
            _origin = origin;
            Color = color;
            Transform = transform;
        }

        public TextInstance(SpriteFont font, string text, Transform transform, Align align, Color color)
        {
            _font = font;
            _text = text;
            _align = align;
            Color = color;
            Transform = transform;
            UpdateOrigin();
        }

        public Align Align
        {
            get { return _align; }
            set 
            {
                _align = value;
                UpdateOrigin();
            }
        }

        public Color Color { get; set; }

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                UpdateOrigin();
            }
        }

        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                if (_align == Align.None)
                    _origin = value;
            }
        }
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                UpdateOrigin();
            }
        }

        public Transform Transform { get; set; }

        public Vector2 Size
        {
            get
            {
                if (_font == null || _text == null)
                    return Vector2.Zero;

                return _font.MeasureString(_text);
            }
        }

        private void UpdateOrigin()
        {
            Vector2 factor;
            switch (_align)
            {
                case Align.TopLeft:
                    factor.X = 0.0f; factor.Y = 0.0f;
                    break;

                case Align.TopCenter:
                    factor.X = 0.5f; factor.Y = 0.0f;
                    break;

                case Align.TopRight:
                    factor.X = 1.0f; factor.Y = 0.0f;
                    break;

                case Align.MiddleLeft:
                    factor.X = 0.0f; factor.Y = 0.5f;
                    break;

                case Align.MiddleCenter:
                    factor.X = 0.5f; factor.Y = 0.5f;
                    break;

                case Align.MiddleRight:
                    factor.X = 1.0f; factor.Y = 0.5f;
                    break;

                case Align.BottomLeft:
                    factor.X = 0.0f; factor.Y = 1.0f;
                    break;

                case Align.BottomCenter:
                    factor.X = 0.5f; factor.Y = 1.0f;
                    break;

                case Align.BottomRight:
                    factor.X = 1.0f; factor.Y = 1.0f;
                    break;

                default:
                    return;
            }

            _origin = factor * Size;
        }
    }
}
