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
using osum.GameModes.MainMenu;
namespace osum.GameModes
{
    public class Failed : GameMode
    {
        private CursorSprite cursorSprite;
        internal static pText OptionsText;


        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        internal override void Initialize()
        {
            PauseButton PauseRetry = new PauseButton(OsuMode.Play, @"pause-retry");
            spriteManager.Add(PauseRetry);
            PauseRetry.SetPosition(new Vector2(180, 200));

            PauseButton PauseBack = new PauseButton(OsuMode.SongSelect, @"pause-back");
            spriteManager.Add(PauseBack);
            PauseBack.SetPosition(new Vector2(180, 270));

            OptionsText = new pText("Failed", 10, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(OptionsText);
            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);
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
            cursorSprite.Update();
            base.Update();

        }

    }
}

