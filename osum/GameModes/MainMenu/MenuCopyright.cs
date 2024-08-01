using System;
using osum.Graphics.Sprites;
using osum.GameplayElements.Beatmaps;
using OpenTK;
using OpenTK.Graphics;
using osum.Audio;
using osum.Graphics.Skins;
using osum.Helpers;

namespace osum.GameModes.MainMenu
{
    internal class MenuCopyright : pSpriteCollection
    {
        pSprite backingPlate;
        pSprite backgroundTexture;
        pText text;
        OsuMode mode;
        Vector2 originalPosition;
        string originalTexturePath;

        public MenuCopyright()
        {
            backgroundTexture = new pSprite(TextureManager.Load(@"menu-copyright"), FieldTypes.StandardSnapBottomLeft, OriginTypes.BottomLeft,
                                             ClockTypes.Mode, Vector2.Zero, 1, true, Color4.White);
            backgroundTexture.Scale = new Vector2(1, 1);
            backgroundTexture.DrawDepth = 1f;
            backgroundTexture.Alpha = 1;
            SpriteCollection.Add(backgroundTexture);

            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 0;
            backingPlate.Scale.Y = 32;
            backingPlate.Scale.X = 257;
            SpriteCollection.Add(backingPlate);

            backingPlate.OnClick += delegate
            {
            };

            backgroundTexture.OnHover += OnHover;
            backgroundTexture.OnHoverLost += OnHoverLost;
        }

        private void OnHover(object sender, EventArgs e)
        {
            backgroundTexture.Colour = Color4.Yellow;
            backgroundTexture.ScaleTo(1.3f, 200, EasingTypes.In);
        }

        private void OnHoverLost(object sender, EventArgs e)
        {
            backgroundTexture.Colour = Color4.White;
            backgroundTexture.ScaleTo(1f, 100, EasingTypes.In);
        }

        internal void MoveTo(Vector2 location)
        {
            originalPosition = location;
            if (backgroundTexture != null)
                backgroundTexture.MoveTo(location, 150, EasingTypes.In);
            backingPlate.MoveTo(location, 150, EasingTypes.In);
        }

        public pSprite GetSprite()
        {
            return backgroundTexture;
        }

        internal void SetPosition(Vector2 location)
        {
            originalPosition = location;
            if (backgroundTexture != null)
                backgroundTexture.Position = location;
            backingPlate.Position = location;
        }
    }
}
