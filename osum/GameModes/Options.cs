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
namespace osum.GameModes
{
    public class Options : GameMode
    {
        internal static Score RankableScore;
        internal static pText OptionsText;


        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        internal override void Initialize()
        {
            pSprite backButton = BackButton.CreateBackButton(OsuMode.MainMenu);
            spriteManager.Add(backButton);

            OptionsText = new pText("Options", 10, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(OptionsText);
        }

        public Options()
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

