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
        private CursorSprite cursorSprite;


        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        internal override void Initialize()
        {
            pSprite backButton = BackButton.CreateBackButton(OsuMode.SongSelect);
            spriteManager.Add(backButton);

            ScoreDisplay = new pText("", 10, Vector2.Zero, new Vector2(512, 40), 1, true, Color4.White, false);
            spriteManager.Add(ScoreDisplay);
            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);
        }

        public Ranking()
        {
        }

        public override void Draw()
        {
            cursorSprite.Update();
            base.Draw();
        }

        public override void Update()
        {
            base.Update();


        }

    }
}

