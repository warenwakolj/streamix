using OpenTK.Graphics;
using OpenTK;
using osum.Audio;
using osum.GameModes;
using osum.GameplayElements.Beatmaps;
using osum.GameplayElements;
using osum.Graphics.Skins;
using osum.Graphics.Sprites;
using osum.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System;
using osum.GameModes.MainMenu;
using osum.Support;
using System.Text.RegularExpressions;

namespace osum
{
    class MainMenu : GameMode
    {
        private pSprite osuLogo;
        private CursorSprite cursorSprite;
        private List<Beatmap> availableMaps;
        private const string BEATMAP_DIRECTORY = "Beatmaps";
        internal static pText InfoText;
        private Beatmap currentlyPlayingBeatmap;

        public static Beatmap SelectedBeatmap { get; private set; }

        private int osuLogoClickCount = 0;

        internal override void Initialize()
        {
            InitializeBeatmaps();

            InfoText = new pText("Loading...", 10, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(InfoText);

            PlayRandomBeatmap();

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

                        availableMaps.Add(beatmap);
                    }
                }
            }
        }

        private void PlayRandomBeatmap()
        {
            if (availableMaps == null || availableMaps.Count == 0) return;

            Random random = new Random();
            Beatmap randomBeatmap = availableMaps[random.Next(availableMaps.Count)];
            HitObjectManager hitObjectManager = new HitObjectManager(randomBeatmap);
            hitObjectManager.LoadFile();

            string audioFilename = randomBeatmap.AudioFilename;

            if (!string.IsNullOrEmpty(audioFilename))
            {
                string audioFilePath = Path.Combine(randomBeatmap.ContainerFilename, audioFilename);
                byte[] audioData = File.ReadAllBytes(audioFilePath);
                if (audioData != null)
                {

                    string beatmapFilePath = Path.Combine(randomBeatmap.ContainerFilename, randomBeatmap.BeatmapFilename);
                    string[] beatmapLines = File.ReadAllLines(beatmapFilePath);
                    int previewTime = 0;

                    foreach (string line in beatmapLines)
                    {
                        if (line.StartsWith("PreviewTime"))
                        {
                            previewTime = int.Parse(line.Split(':')[1].Trim());
                            break;
                        }
                    }

                    AudioEngine.Music.Load(audioData);
                    AudioEngine.Music.Play();

                    if (previewTime > 0)
                    {
                        AudioEngine.Music.SetCurrentTime(previewTime / 1000.0);
                    }

                    string filename = Path.GetFileNameWithoutExtension(randomBeatmap.BeatmapFilename);
                    Regex regex = new Regex(@"(.*) - (.*) \((.*)\) \[(.*)\]");
                    Match match = regex.Match(filename);
                    string artist = match.Groups[1].Value;
                    string songName = match.Groups[2].Value;

                    InfoText.Text = $"{artist} - {songName}";

                    currentlyPlayingBeatmap = randomBeatmap;
                    SelectedBeatmap = randomBeatmap;
                }
            }
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

        public Beatmap GetCurrentlyPlayingBeatmap()
        {
            return currentlyPlayingBeatmap;
        }
    }
}
