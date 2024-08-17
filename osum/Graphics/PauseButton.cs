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
    internal class PauseButton : pSpriteCollection
    {
        pSprite backingPlate;
        pSprite backgroundTexture;
        pText text;
        OsuMode mode;
        Vector2 originalPosition;
        string originalTexturePath;


        public PauseButton(OsuMode mode, string texturePath)
        {
            this.mode = mode;
            this.originalTexturePath = texturePath;

            if (!string.IsNullOrEmpty(texturePath))
            {
                // Change OriginTypes to Centre to allow scaling from the center
                backgroundTexture = new pSprite(TextureManager.Load(texturePath), FieldTypes.Standard, OriginTypes.Centre,
                                                 ClockTypes.Mode, Vector2.Zero, 1, true, Color4.White);
                backgroundTexture.Scale.Y = 1;
                backgroundTexture.Scale.X = 1;
                backgroundTexture.DrawDepth = 2;
                backgroundTexture.Alpha = 1;
                SpriteCollection.Add(backgroundTexture);
            }

            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 1;
            backingPlate.Scale.Y = 122;
            backingPlate.Scale.X = 441;
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
            AudioEngine.PlaySample(OsuSamples.MenuClick);

            if (backgroundTexture != null)
            {
                backgroundTexture.ScaleTo(1.1f, 200, EasingTypes.In);
            }
        }

        private void OnHoverLost(object sender, EventArgs e)
        {
            if (backgroundTexture != null)
            {
                backgroundTexture.ScaleTo(1, 200, EasingTypes.In);
            }
        }

        public pSprite GetSprite()
        {
            return backgroundTexture;
        }

        internal void SetPosition(Vector2 location)
        {
            originalPosition = location;

            if (backgroundTexture != null)
            {
                backgroundTexture.Position = location + new Vector2(backingPlate.Scale.X / 3, backingPlate.Scale.Y / 3);
            }

            backingPlate.Position = location;
        }
    }
}
