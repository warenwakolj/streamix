using OpenTK.Graphics;
using OpenTK;
using osum.Audio;
using osum.GameModes;
using osum.Graphics.Skins;
using osum.Graphics.Sprites;
using osum.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System;
using osum.GameModes.MainMenu;
using osum.Support;
using System.Text.RegularExpressions;
using OpenTK.Platform;
using osum.GameplayElements.Beatmaps;
using osum.GameplayElements;

namespace osum
{
    class MainMenu : GameMode
    {
        private pSprite osuLogo;
        private pSprite menuNp;
        private CursorSprite cursorSprite;
        internal static pText InfoText;
        private MenuMusicManager MenuMusicManager;

        public static Beatmap SelectedBeatmap { get; private set; }

        private int osuLogoClickCount = 0;

        internal override void Initialize()
        {
            MenuMusicManager = new MenuMusicManager();

            InfoText = new pText("Loading...", 10, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false)
            {
                Field = FieldTypes.StandardSnapRight,
                Origin = OriginTypes.TopRight,
                Position = new Vector2(-10, 0)
            };
            spriteManager.Add(InfoText);

            Vector2 textSize = InfoText.MeasureText();


            menuNp = new pSprite(TextureManager.Load(@"menu-np"), FieldTypes.StandardSnapRight, OriginTypes.TopRight, ClockTypes.Mode,
                                 new Vector2(textSize.X - 30, 0),
                                 1f, true, Color4.White);
            spriteManager.Add(menuNp);


            UserCard UserCard = new UserCard(new Vector2(4, 4), "Guest");
            UserCard.AddToSpriteManager(spriteManager);

            MenuCopyright MenuCopyright = new MenuCopyright();
            spriteManager.Add(MenuCopyright);

            menuBackground = new pSprite(TextureManager.Load(@"menu-background"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                 ClockTypes.Mode, Vector2.Zero, 0, true, Color.White);
            spriteManager.Add(menuBackground);

            osuLogo = new pSprite(TextureManager.Load(@"menu-osu"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(0, -20), 1f, true, Color4.White);
            spriteManager.Add(osuLogo);

            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);

            osuLogo.OnClick += delegate {
                DateTime currentTime = DateTime.Now;

                if (osuLogoClickCount == 0)
                {
                    osuLogoClickCount++;
                    AudioEngine.PlaySample(OsuSamples.MenuHit);
                    MoveTo(new Vector2(-120, -20));

                    MenuButton ButtonPlay = new MenuButton(OsuMode.SongSelect, @"menu-button-play");
                    spriteManager.Add(ButtonPlay);
                    ButtonPlay.SetPosition(new Vector2(220, 120));

                    MenuButton ButtonOptions = new MenuButton(OsuMode.Options, @"menu-button-options");
                    spriteManager.Add(ButtonOptions);
                    ButtonOptions.SetPosition(new Vector2(220, 180));

                    MenuButton ButtonQuit = new MenuButton(OsuMode.Exit, @"menu-button-exit");
                    spriteManager.Add(ButtonQuit);
                    ButtonQuit.SetPosition(new Vector2(220, 240));
                }
            };

            MenuMusicManager.PlayRandomBeatmap(InfoText);
        }

        internal void MoveTo(Vector2 location)
        {
            osuLogo.MoveTo(location, 400, EasingTypes.In);
        }

        public override void Update()
        {
            updateBeat();
            cursorSprite.Update();
            base.Update();
        }

        private pSprite menuBackground;

        public override void Draw()
        {
            base.Draw();

            osuLogo.ScaleScalar = 1 + AudioEngine.Music.CurrentVolume / 90;
        }

        int lastBgmBeat = 0;
        int offset = 0;
        float between_beats = 375 / 6f;

        private void updateBeat()
        {
            int newBeat = (int)((Clock.AudioTime - offset) / between_beats);
            if (lastBgmBeat != newBeat)
            {
                if (newBeat % 4 == 0)
                {
                    osuLogo.ScaleScalar *= 0.975f;
                    osuLogo.ScaleTo(1, 500, EasingTypes.In);
                }

                lastBgmBeat = newBeat;
            }
        }
    }
}
