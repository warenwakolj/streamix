using System;
using osum.GameplayElements.Scoring;
using osum.Graphics.Sprites;
using osum.Graphics.Skins;
using osum.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System.Collections;
using System.Collections.Generic;
using osum.Audio;
using System.Drawing;
namespace osum.GameModes
{
    public class Failed : GameMode
    {
        pSprite retryButton;
        pSprite backButton;
        internal static pText OptionsText;


        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        internal override void Initialize()
        {
            retryButton = new pSprite(TextureManager.Load("pause-retry"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(0, 10), 1.0f, true, Color4.White);

            retryButton.OnClick += delegate {
                retryButton.UnbindAllEvents();
                Director.ChangeMode(OsuMode.Play);
            };
            spriteManager.Add(retryButton);

            backButton = new pSprite(TextureManager.Load("pause-back"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(0, 80), 1.0f, true, Color4.White);

            backButton.OnClick += delegate {
                backButton.UnbindAllEvents();
                Director.ChangeMode(OsuMode.SongSelect);
            };
            spriteManager.Add(backButton);

            OptionsText = new pText("Failed", 10, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(OptionsText);
        }

        public Failed()
        {
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();

        }

    }
}

