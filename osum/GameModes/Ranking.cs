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
    public class Ranking : GameMode
    {
        internal static Score RankableScore;
        internal static pText ScoreDisplay;


        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        internal override void Initialize()
        {
            //add a temporary button to allow returning to song select
            pSprite backButton = new pSprite(TextureManager.Load("menu-back"), FieldTypes.StandardSnapBottomLeft, OriginTypes.BottomLeft,
                                     ClockTypes.Game, Vector2.Zero, 1, true, new Color4(1, 1, 1, 0.4f));

            backButton.OnClick += delegate {
                AudioEngine.PlaySample(OsuSamples.MenuBack);
                backButton.UnbindAllEvents();
                Director.ChangeMode(OsuMode.SongSelect);
            };

            spriteManager.Add(backButton);

            ScoreDisplay = new pText("", 10, Vector2.Zero, new Vector2(512, 40), 1, true, Color4.White, false);
            spriteManager.Add(ScoreDisplay);
        }

        public Ranking()
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

