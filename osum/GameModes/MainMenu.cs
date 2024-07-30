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
using osum.GameModes.MainMenu;
using osum.GameplayElements.Beatmaps;

namespace osum
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

            MenuButton ButtonPlay = new MenuButton("Play", OsuMode.SongSelect);
            spriteManager.Add(ButtonPlay);
            ButtonPlay.SetPosition(new Vector2(200, 120));
            MenuButton ButtonOptions = new MenuButton("Options", OsuMode.Options);
            spriteManager.Add(ButtonOptions);
            ButtonOptions.SetPosition(new Vector2(200, 180));
            MenuButton ButtonQuit = new MenuButton("Quit", OsuMode.SongSelect);
            spriteManager.Add(ButtonQuit);
            ButtonQuit.SetPosition(new Vector2(200, 240));


            osuLogo = new pSprite(TextureManager.Load(@"menu-osu"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(-120, 0), 1.0f, true, Color4.White);
            spriteManager.Add(osuLogo);


        }



        private pSprite menuBackground;

   

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
