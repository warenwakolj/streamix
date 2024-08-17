using System;
using OpenTK;
using OpenTK.Graphics;
using osum.Graphics.Sprites;

namespace osum.GameModes
{
    internal class OptionsButton : pSpriteCollection
    {
        private pSprite buttonBackground;
        private pText buttonText;

        public event Action OnClick;

        public OptionsButton(string text, Vector2 position, Vector2 size, Color4 backgroundColor)
        {
            buttonBackground = pSprite.FullscreenWhitePixel;
            buttonBackground.Colour = backgroundColor;
            buttonBackground.Alpha = 1;
            buttonBackground.AlwaysDraw = true;
            buttonBackground.Scale = size;  
            buttonBackground.Position = position;

            buttonBackground.OnClick += delegate
            {
                OnClick?.Invoke();
            };
            buttonBackground.HandleClickOnUp = true; 

            buttonText = new pText(text, 10, position + new Vector2((size.X) / 4.5f, (size.Y) / 5f), new Vector2(0, 0), 1, true, Color4.White, false);

            SpriteCollection.Add(buttonBackground);
            SpriteCollection.Add(buttonText);
        }

        public void SetPosition(Vector2 position)
        {
            buttonBackground.Position = position;
            buttonText.Position = position + new Vector2(10, (buttonBackground.Scale.Y - 20) / 2);
        }

        public void SetColor(Color4 color)
        {
            buttonBackground.Colour = color;
        }
    }
}
