using System;
using osum.GameModes;
using OpenTK.Graphics.ES11;
using osum.GameplayElements;
using OpenTK;
using osum.Helpers;
using System.IO;
using osum.Graphics.Skins;
using osum.GameplayElements.Beatmaps;
using System.Collections.Generic;
using osum.Graphics.Sprites;
using OpenTK.Graphics;
using osum.GameModes;
using osum.Audio;

namespace osum
{
    public class SongSelect : GameMode
    {
        pSprite SongSelectTop;
        pSprite SongSelectBottom;
        private CursorSprite cursorSprite;
        private Beatmap selectedBeatmap;


        public SongSelect() : base()
        {
        }

        static List<Beatmap> availableMaps;
        internal static pText MetadataText;


        internal override void Initialize()
        {
            AudioEngine.Music.Stop();
            InitializeBeatmaps();
			
			InputManager.OnMove += InputManager_OnMove;
            SongSelectTop = new pSprite(TextureManager.Load("songselect-top"), FieldTypes.Standard, OriginTypes.TopLeft,
                         ClockTypes.Game, Vector2.Zero, 2, true, new Color4(1, 1, 1, 1f));
            SongSelectTop.Scale = new Vector2(1f, 1f);

            InputManager.OnMove += InputManager_OnMove;
            SongSelectBottom = new pSprite(TextureManager.Load("songselect-bottom"), FieldTypes.StandardSnapBottomLeft, OriginTypes.BottomLeft,
                         ClockTypes.Game, Vector2.Zero, 2, true, new Color4(1, 1, 1, 1f));
            SongSelectBottom.Scale = new Vector2(10f, 1f);


            pSprite backButton = BackButton.CreateBackButton(OsuMode.MainMenu);
            spriteManager.Add(backButton);
            spriteManager.Add(SongSelectTop);
            spriteManager.Add(SongSelectBottom);

            MetadataText = new pText("Artist - title (mapped by name)", 10, Vector2.Zero, new Vector2(0, 0), 3, true, Color4.White, false);
            spriteManager.Add(MetadataText);

            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);

            HighlightSelectedBeatmap();

        }

        public SongSelect(Beatmap selectedBeatmap) : base()
        {
            this.selectedBeatmap = selectedBeatmap;
        }

        private void HighlightSelectedBeatmap()
        {
            foreach (var panel in panels)
            {
                if (panel.Beatmap == selectedBeatmap)
                {
                    panel.Select(); 
                    break;
                }
            }
        }

        public static void UpdateMetadataText(string text)
        {
            if (MetadataText != null)
            {
                MetadataText.Text = text;
            }
        }

        void InputManager_OnMove(InputSource source, TrackingPoint trackingPoint)
        {
            if (InputManager.IsPressed)
                offset += trackingPoint.WindowDelta.Y;	
        }



        public override void Dispose()
		{
			base.Dispose();
			
			InputManager.OnMove -= InputManager_OnMove;
		}
		
		const string BEATMAP_DIRECTORY = "Beatmaps";
		
		List<BeatmapPanel> panels = new List<BeatmapPanel>();

        private void InitializeBeatmaps()
        {
            availableMaps = new List<Beatmap>();

            if (Directory.Exists(BEATMAP_DIRECTORY))
                foreach (string s in Directory.GetDirectories(BEATMAP_DIRECTORY))
                {
                    Beatmap reader = new Beatmap(s);

                    foreach (string file in reader.Package == null ? Directory.GetFiles(s, "*.osu") : reader.Package.MapFiles)
                    {
                        Beatmap b = new Beatmap(s);
                        b.BeatmapFilename = Path.GetFileName(file);

                        BeatmapPanel panel = new BeatmapPanel(b);
                        spriteManager.Add(panel);

                        availableMaps.Add(b);
                        panels.Add(panel);
                    }
                }
        
			
#if IPHONE
			string docs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			
			foreach (string s in Directory.GetFiles(docs,"*.osu"))
            {
				Beatmap b = new Beatmap(docs);
                b.BeatmapFilename = Path.GetFileName(s);
				
				BeatmapPanel panel = new BeatmapPanel(b);
				spriteManager.Add(panel);

                availableMaps.Add(b);
				panels.Add(panel);
			}
#endif
        }
		
		float offset;

        private void PlaySelectedSong(Beatmap beatmap)
        {
            if (beatmap != null && !string.IsNullOrEmpty(beatmap.AudioFilename))
            {
                string audioFilePath = Path.Combine(beatmap.ContainerFilename, beatmap.AudioFilename);
                if (File.Exists(audioFilePath))
                {
                    byte[] audioData = File.ReadAllBytes(audioFilePath);
                    if (audioData != null)
                    {
                        AudioEngine.Music.Load(audioData);
                        AudioEngine.Music.Play();
                    }
                }
            }
        }

        public override void Draw()
        {
			base.Draw();
        }

        public override void Update()
        {
            base.Update();
            cursorSprite.Update();

            if (Director.PendingMode == OsuMode.Unknown)
            {
                Vector2 pos = new Vector2(320, -10 + offset);
                foreach (BeatmapPanel p in panels)
                {
                    p.MoveTo(pos);
                    pos.Y += 60;
                }
            }
        }
    }


}

