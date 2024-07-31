using System;
using osum.GameplayElements.Scoring;
using osum.Graphics.Sprites;
using osum.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace osum.GameModes
{
    public class Options : GameMode
    {
        internal static Score RankableScore;
        internal static pText OptionsText;
        private CursorSprite cursorSprite;
        private GameWindowDesktop gameWindow; // Call game window

        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        // this is the part that won't let the game compile, tried setting gamewindow to public but it also wont compile even if it shows no errors
        // public Options(GameWindowDesktop window)
        // {
        //    gameWindow = window;
        //}

        internal override void Initialize()
        {
            pSprite backButton = BackButton.CreateBackButton(OsuMode.MainMenu);
            spriteManager.Add(backButton);

            OptionsText = new pText("Options", 10, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(OptionsText);

            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);

            // CheckBox for fullscreen toggle
            CheckBox fullscreenCheckBox = new CheckBox(new Vector2(100, 200), "Fullscreen");
            fullscreenCheckBox.OnStateChanged += (isSelected) =>
            {
                gameWindow.ToggleFullscreen();
            };
            fullscreenCheckBox.AddToSpriteManager(spriteManager);

            // CheckBox for VSync toggle
            CheckBox vsyncCheckBox = new CheckBox(new Vector2(100, 150), "VSync");
            vsyncCheckBox.OnStateChanged += (isSelected) =>
            {
                gameWindow.ToggleVSync();
            };
            vsyncCheckBox.AddToSpriteManager(spriteManager);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
            cursorSprite.Update();
        }
    }
}
