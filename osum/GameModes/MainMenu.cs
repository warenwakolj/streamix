﻿using System;
using System.Collections.Generic;
using System.IO;
using osum.GameplayElements.Beatmaps;
using osum.Graphics.Sprites;
using osum.Helpers;
using OpenTK;
using OpenTK.Graphics;
using osum.Audio;
using osum.Support;
using osum.Graphics;
using osum.GameModes.MainMenu;
using osum.GameModes;
using osum.Graphics.Skins;
using System.Drawing;

namespace osum
{
    class MainMenu : GameMode
    {
        private pSprite osuLogo;
        private CursorSprite cursorSprite; 
        private List<Beatmap> availableMaps;
        private const string BEATMAP_DIRECTORY = "Beatmaps";

        internal override void Initialize()
        {
            InitializeBeatmaps();

            PlayRandomBeatmap();

            menuBackground = new pSprite(TextureManager.Load(@"menu-background"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                            ClockTypes.Mode, Vector2.Zero, 0, true, Color.White);
            spriteManager.Add(menuBackground);

            MenuButton ButtonPlay = new MenuButton("Play", OsuMode.SongSelect);
            spriteManager.Add(ButtonPlay);
            ButtonPlay.SetPosition(new Vector2(200, 120));

            MenuButton ButtonOptions = new MenuButton("Options", OsuMode.Options);
            spriteManager.Add(ButtonOptions);
            ButtonOptions.SetPosition(new Vector2(200, 180));

            MenuButton ButtonQuit = new MenuButton("Quit", OsuMode.Play);
            spriteManager.Add(ButtonQuit);
            ButtonQuit.SetPosition(new Vector2(200, 240));

            osuLogo = new pSprite(TextureManager.Load(@"menu-osu"), FieldTypes.StandardSnapCentre, OriginTypes.Centre, ClockTypes.Mode, new Vector2(-120, 0), 1.0f, true, Color4.White);
            spriteManager.Add(osuLogo);

            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);
        }

        private void InitializeBeatmaps()
        {
            availableMaps = new List<Beatmap>();

            if (Directory.Exists(BEATMAP_DIRECTORY))
            {
                foreach (string directory in Directory.GetDirectories(BEATMAP_DIRECTORY))
                {
                    foreach (string file in Directory.GetFiles(directory, "*.osu"))
                    {
                        Beatmap beatmap = new Beatmap(directory)
                        {
                            BeatmapFilename = Path.GetFileName(file)
                        };
                        string audioFile = LocateAudioFile(directory);
                        if (!string.IsNullOrEmpty(audioFile))
                        {
                            beatmap.AudioFilename = Path.GetFileName(audioFile);
                        }

                        availableMaps.Add(beatmap);
                    }
                }
            }
        }

        private string LocateAudioFile(string directory)
        {
            string[] audioExtensions = { ".mp3", ".wav", ".ogg" };
            foreach (string extension in audioExtensions)
            {
                string[] files = Directory.GetFiles(directory, "*" + extension);
                if (files.Length > 0)
                {
                    return files[0];
                }
            }
            return null;
        }

        private void PlayRandomBeatmap()
        {
            if (availableMaps == null || availableMaps.Count == 0) return;

            Random random = new Random();
            Beatmap randomBeatmap = availableMaps[random.Next(availableMaps.Count)];
            string audioFilename = randomBeatmap.AudioFilename;

            if (!string.IsNullOrEmpty(audioFilename))
            {
                string audioFilePath = Path.Combine(randomBeatmap.ContainerFilename, audioFilename);
                byte[] audioData = File.ReadAllBytes(audioFilePath);
                if (audioData != null)
                {
                    AudioEngine.Music.Load(audioData);
                    AudioEngine.Music.Play();
                }
            }
        }

        private pSprite menuBackground;

        public override void Update()
        {
            base.Update();

            cursorSprite.Update();
        }

        public override void Draw()
        {

            base.Draw();
        }
    }
}
