using System;
using osum.Graphics.Sprites;
using osum.GameplayElements.Beatmaps;
using OpenTK;
using OpenTK.Graphics;
using osum.Audio;

namespace osum.GameModes.MainMenu
{
    internal class MenuButton : pSpriteCollection
    {
        pSprite backingPlate;
        pText text;
        OsuMode mode;
        Vector2 originalPosition;

        public MenuButton(string buttonText, OsuMode mode)
        {
            this.mode = mode;

            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 1;
            backingPlate.AlwaysDraw = true;
            backingPlate.Colour = Color4.OrangeRed;
            backingPlate.Scale.Y = 89;
            backingPlate.Scale.X = 583;
            backingPlate.DrawDepth = 0.8f;
            SpriteCollection.Add(backingPlate);

            backingPlate.OnClick += delegate {
                AudioEngine.PlaySample(OsuSamples.MenuHit);
                backingPlate.UnbindAllEvents();
                backingPlate.Colour = Color4.LightSkyBlue;
                Director.ChangeMode(this.mode);
            };

            backingPlate.HandleClickOnUp = true;

            text = new pText(buttonText, 10, Vector2.Zero, new Vector2(100, 80), 1, true, Color4.White, false);
            text.Offset = new Vector2(10, 0);
            SpriteCollection.Add(text);

            backingPlate.OnHover += OnHover;
            backingPlate.OnHoverLost += OnHoverLost;
        }

        private void OnHover(object sender, EventArgs e)
        {
            MoveTo(originalPosition + new Vector2(30, 0));
            AudioEngine.PlaySample(OsuSamples.MenuClick);
        }

        private void OnHoverLost(object sender, EventArgs e)
        {
            MoveTo(originalPosition + new Vector2(-30, 0)); ;
        }


        internal void MoveTo(Vector2 location)
        {
            originalPosition = location;
            backingPlate.MoveTo(location, 150);
            text.MoveTo(location + new Vector2(10, 0), 150);
        }

        internal void SetPosition(Vector2 location)
        {
            originalPosition = location;
            backingPlate.Position = location;
            text.Position = location + new Vector2(10, 0);
        }
    }
}
