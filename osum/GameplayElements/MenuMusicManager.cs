using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using osum.Audio;
using osum.GameModes;
using osum.GameplayElements;
using osum.GameplayElements.Beatmaps;
using osum.Graphics.Sprites;

namespace osum
{
    public class MenuMusicManager
    {
        private const string BEATMAP_DIRECTORY = "Songs";
        private List<Beatmap> availableMaps;
        private Beatmap currentlyPlayingBeatmap;
        private static MenuMusicManager instance;

        public static MenuMusicManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuMusicManager();
                }
                return instance;
            }
        }

        public void Update()
        {
            if (Director.CurrentOsuMode == OsuMode.MainMenu && currentlyPlayingBeatmap == null)
            {
                PlayRandomBeatmap(MainMenu.InfoText);
            }
        }

        private MenuMusicManager()
        {
            InitializeBeatmaps();
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

        internal void PlayRandomBeatmap(pText infoText)
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

                    infoText.Text = $"{artist} - {songName}";

                    currentlyPlayingBeatmap = randomBeatmap;
                }
            }
        }

        public void PlayBeatmap(Beatmap beatmap)
        {
            if (beatmap == null) return;

            HitObjectManager hitObjectManager = new HitObjectManager(beatmap);
            hitObjectManager.LoadFile();

            string audioFilename = beatmap.AudioFilename;

            if (!string.IsNullOrEmpty(audioFilename))
            {
                string audioFilePath = Path.Combine(beatmap.ContainerFilename, audioFilename);
                byte[] audioData = File.ReadAllBytes(audioFilePath);
                if (audioData != null)
                {
                    Console.WriteLine("Audio data loaded successfully");
                    string beatmapFilePath = Path.Combine(beatmap.ContainerFilename, beatmap.BeatmapFilename);
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
                    AudioEngine.Music.Stop();
                    AudioEngine.Music.Load(audioData);
                    AudioEngine.Music.Play();

                    if (previewTime > 0)
                    {
                        AudioEngine.Music.SetCurrentTime(previewTime / 1000.0);
                    }

                    currentlyPlayingBeatmap = beatmap;
                }
            }
        }


        public Beatmap GetCurrentlyPlayingBeatmap()
        {
            return currentlyPlayingBeatmap;
        }
    }
}
