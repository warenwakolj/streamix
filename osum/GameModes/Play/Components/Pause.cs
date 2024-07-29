using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using osum.Graphics.Skins;
using osum.Graphics.Sprites;
using osum.Helpers;

namespace osum.GameModes
{
    public class PauseMenu : GameMode
    {
        private pSprite resumeButton;
        private pSprite OptionsButton;
        private pSprite background;

        public PauseMenu()
        {
            Initialize();
        }

        internal override void Initialize()
        {
            // Create a semi-transparent background for the pause menu
            background = new pSprite(TextureManager.Load("menu-background"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                     ClockTypes.Game, Vector2.Zero, 1, true, new Color4(0, 0, 0, 0.8f));
            spriteManager.Add(background);

            // Create the resume button
            resumeButton = new pSprite(TextureManager.Load("pause-continue"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                       ClockTypes.Game, new Vector2(0, -50), 1, true, Color4.White);
            resumeButton.Scale = new Vector2(0.5f, 0.5f);
            resumeButton.OnClick += delegate
            {
                Director.ChangeMode(OsuMode.Play);
            };
            spriteManager.Add(resumeButton);

            // Create the song select button
            OptionsButton = new pSprite(TextureManager.Load("pause-back"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                           ClockTypes.Game, new Vector2(0, 50), 1, true, Color4.White);
            OptionsButton.Scale = new Vector2(0.5f, 0.5f);
            OptionsButton.OnClick += delegate
            {
                Director.ChangeMode(OsuMode.Ranking);
            };
            spriteManager.Add(OptionsButton);
        }

        public override void Update()
        {
            // Any updates related to the pause menu can be handled here
            base.Update();
        }

        public override void Draw()
        {
            // Draw the pause menu elements
            base.Draw();
        }

        public override void Dispose()
        {
            // Clean up resources
            resumeButton.UnbindAllEvents();
            OptionsButton.UnbindAllEvents();

            base.Dispose();
        }
    }
}
