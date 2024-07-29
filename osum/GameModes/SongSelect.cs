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
using osum.GameModes.SongSelect;
using osum.Audio;

namespace osum
{
    public class SongSelect : GameMode
    {
        pSprite backButton;
        pSprite SongSelectTop;
        public SongSelect() : base()
        {
        }

        static List<Beatmap> availableMaps;

        internal override void Initialize()
        {
            InitializeBeatmaps();
			
			InputManager.OnMove += InputManager_OnMove;
            SongSelectTop = new pSprite(TextureManager.Load("songselect-top"), FieldTypes.Standard, OriginTypes.TopLeft,
                         ClockTypes.Game, Vector2.Zero, 1, true, new Color4(1, 1, 1, 1f));
            SongSelectTop.Scale = new Vector2(1f, 1f);


            backButton = new pSprite(TextureManager.Load("menu-back"), FieldTypes.StandardSnapBottomLeft, OriginTypes.BottomLeft,
                                     ClockTypes.Game, Vector2.Zero, 1, true, new Color4(1, 1, 1, 1f));
            backButton.Scale = new Vector2(1f, 1f);

            backButton.OnClick += delegate {
                AudioEngine.PlaySample(OsuSamples.MenuBack);
                backButton.UnbindAllEvents();
                Director.ChangeMode(OsuMode.MainMenu);
            };
            spriteManager.Add(backButton);
            spriteManager.Add(SongSelectTop);
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

                foreach (string file in reader.Package == null ? Directory.GetFiles(s,"*.osu") : reader.Package.MapFiles)
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
		
        public override void Draw()
        {
			base.Draw();
        }

        public override void Update()
        {
            base.Update();
			
			if (Director.PendingMode == OsuMode.Unknown)
			{
				Vector2 pos = new Vector2(320,-10 + offset);
				foreach (BeatmapPanel p in panels)
				{
					p.MoveTo(pos);
					pos.Y += 60;
				}
			}
        }
    }
}

