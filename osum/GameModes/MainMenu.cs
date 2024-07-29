using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osum.GameModes;
using osum.Graphics.Sprites;
using osum.Graphics.Skins;
using osum.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using osum.Audio;
using osum.Support;
using osum.Graphics;
using System.IO;

namespace osum.GameModes
{
    class MainMenu : GameMode
    {
        pSprite osuLogo;
        pSprite playButton;
        pSprite optionsButton;
        pSprite exitButton;

        List<pSprite> explosions = new List<pSprite>();

        internal override void Initialize()
        {
            menuBackground =
                new pSprite(TextureManager.Load(@"menu-background"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                            ClockTypes.Mode, Vector2.Zero, 0, true, Color.White);
            spriteManager.Add(menuBackground);

            osuLogo = new pSprite(TextureManager.Load(@"menu-osu"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(-120, 0), 1.0f, true, Color4.White);
            spriteManager.Add(osuLogo);

            // Adding play button
            playButton = new pSprite(TextureManager.Load(@"menu-button-play"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(80, -60), 1.0f, true, Color4.White);
            playButton.OnClick += playButton_OnClick;
            playButton.OnHover += playButton_OnHover;
            playButton.onHoverLost += playButton_OnHoverEnd;
            spriteManager.Add(playButton);

            // Adding options button
            optionsButton = new pSprite(TextureManager.Load(@"menu-button-options"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(80, 0), 1.0f, true, Color4.White);
            optionsButton.OnClick += optionsButton_OnClick;
            optionsButton.OnHover += optionsButton_OnHover;
            optionsButton.onHoverLost += optionsButton_OnHoverEnd;
            spriteManager.Add(optionsButton);

            // Adding exit button
            exitButton = new pSprite(TextureManager.Load(@"menu-button-exit"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(80, 60), 1.0f, true, Color4.White);
            exitButton.OnClick += exitButton_OnClick;
            exitButton.OnHover += exitButton_OnHover;
            exitButton.onHoverLost += exitButton_OnHoverEnd;
            spriteManager.Add(exitButton);
        }

        private void playButton_OnClick(object sender, EventArgs e)
        {
            if (!Director.IsTransitioning)
            {
                AudioEngine.PlaySample(OsuSamples.MenuHit);
                Director.ChangeMode(OsuMode.SongSelect, new FadeTransition());
            }
        }

        private void optionsButton_OnClick(object sender, EventArgs e)
        {
            if (!Director.IsTransitioning)
            {
                AudioEngine.PlaySample(OsuSamples.MenuHit);
                Director.ChangeMode(OsuMode.Options, new FadeTransition());
            }
        }

        private void exitButton_OnClick(object sender, EventArgs e)
        {
            if (!Director.IsTransitioning)
            {
                AudioEngine.PlaySample(OsuSamples.MenuHit);
                Environment.Exit(0);
            }
        }

        private pSprite menuBackground;

        private void playButton_OnHover(object sender, EventArgs e)
        {
            AudioEngine.PlaySample(OsuSamples.MenuClick);
            playButton.Texture = TextureManager.Load(@"menu-button-play-over");
        }

        private void playButton_OnHoverEnd(object sender, EventArgs e)
        {
            playButton.Texture = TextureManager.Load(@"menu-button-play");
        }

        private void optionsButton_OnHover(object sender, EventArgs e)
        {
            AudioEngine.PlaySample(OsuSamples.MenuClick);
            optionsButton.Texture = TextureManager.Load(@"menu-button-options-over");
        }

        private void optionsButton_OnHoverEnd(object sender, EventArgs e)
        {
            optionsButton.Texture = TextureManager.Load(@"menu-button-options");
        }

        private void exitButton_OnHover(object sender, EventArgs e)
        {
            AudioEngine.PlaySample(OsuSamples.MenuClick);
            exitButton.Texture = TextureManager.Load(@"menu-button-exit-over");
        }

        private void exitButton_OnHoverEnd(object sender, EventArgs e)
        {
            exitButton.Texture = TextureManager.Load(@"menu-button-exit");
        }

        public override void Update()
        {
            base.Update();
            explosions.ForEach(s =>
            {
                if (s.Transformations.Count == 0)
                    s.Transform(new TransformationBounce(Clock.Time, Clock.Time + 900, s.ScaleScalar, 0.1f, 2));
            });
        }

        public override void Draw()
        {
            base.Draw();

            if (!Director.IsTransitioning)
                osuLogo.ScaleScalar = 1 + AudioEngine.Music.CurrentVolume / 100;
        }
    }
}
