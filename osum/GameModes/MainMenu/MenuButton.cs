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
    internal class MenuButton : pSpriteCollection
    {
        pSprite backingPlate;
        pSprite backgroundTexture;
        pText text;
        OsuMode mode;
        Vector2 originalPosition;
        string originalTexturePath;

        public MenuButton(OsuMode mode, string texturePath)
        {
            this.mode = mode;
            this.originalTexturePath = texturePath;

            if (!string.IsNullOrEmpty(texturePath))
            {
                backgroundTexture = new pSprite(TextureManager.Load(texturePath), FieldTypes.Standard, OriginTypes.TopLeft,
                                                 ClockTypes.Mode, Vector2.Zero, 1, true, Color4.White);
                backgroundTexture.Scale.Y = 1;
                backgroundTexture.Scale.X = 1;
                backgroundTexture.DrawDepth = 0.9f;
                SpriteCollection.Add(backgroundTexture);
            }

            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 0;
            backingPlate.Scale.Y = 89;
            backingPlate.Scale.X = 583;
            SpriteCollection.Add(backingPlate);

            backingPlate.OnClick += delegate {
                AudioEngine.PlaySample(OsuSamples.MenuHit);
                backingPlate.UnbindAllEvents();

                Director.ChangeMode(this.mode);
            };

            backingPlate.HandleClickOnUp = true;

            backingPlate.OnHover += OnHover;
            backingPlate.OnHoverLost += OnHoverLost;
        }

        private void OnHover(object sender, EventArgs e)
        {
            MoveTo(originalPosition + new Vector2(30, 0));
            ChangeTexture(true);
            AudioEngine.PlaySample(OsuSamples.MenuClick);
        }

        private void OnHoverLost(object sender, EventArgs e)
        {
            MoveTo(originalPosition + new Vector2(-30, 0));
            ChangeTexture(false);
        }

        private void ChangeTexture(bool isHovered)
        {
            if (backgroundTexture != null)
            {
                string newTexturePath = isHovered ? $"{originalTexturePath}-over" : originalTexturePath;
                backgroundTexture.Texture = TextureManager.Load(newTexturePath);
            }
        }

        internal void MoveTo(Vector2 location)
        {
            originalPosition = location;
            if (backgroundTexture != null)
                backgroundTexture.MoveTo(location, 150);
            backingPlate.MoveTo(location, 150);
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
