using System;
using osum.Graphics.Sprites;
using osum.GameplayElements.Beatmaps;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using osum.Helpers;
using System.Text.RegularExpressions;
namespace osum.GameModes.SongSelect
{
    internal class BeatmapPanel : pSpriteCollection
    {
        public Beatmap Beatmap { get; private set; }

        pSprite backingPlate;
        pText text;
        bool isHovered = false;
        bool isSelected = false;

        private static BeatmapPanel currentlySelectedPanel;

        internal BeatmapPanel(Beatmap beatmap)
        {

            Beatmap = beatmap;

            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 1;
            backingPlate.AlwaysDraw = true;
            backingPlate.Colour = Color4.OrangeRed;
            backingPlate.Scale.Y = 80;
            backingPlate.DrawDepth = 0.8f;
            SpriteCollection.Add(backingPlate);

      

            backingPlate.OnClick += delegate {
                if (isSelected)
                {
                    Player.SetBeatmap(beatmap);
                    Director.ChangeMode(OsuMode.Play);
                }
                else
                {
                    if (currentlySelectedPanel != null)
                    {
                        currentlySelectedPanel.Deselect();
                    }

                    Select();
                }
            };

          

            backingPlate.HandleClickOnUp = true;


            string filename = Path.GetFileNameWithoutExtension(beatmap.BeatmapFilename);

            Regex r = new Regex(@"(.*) - (.*) \((.*)\) \[(.*)\]");
            Match m = r.Match(filename);

            //song name 
            text = new pText(m.Groups[2].Value, 10, Vector2.Zero, new Vector2(400, 80), 1, true, Color4.White, false);
            text.Offset = new Vector2(10, 0);
            SpriteCollection.Add(text);

            //artist // mapper
            text = new pText(m.Groups[3].Value + " // " + m.Groups[1].Value, 6, Vector2.Zero, new Vector2(GameBase.WindowBaseSize.Width - 120, 60), 1, true, Color4.White, false);
            text.Offset = new Vector2(12, 15); 
            SpriteCollection.Add(text);


            //difficulty
            text = new pText(m.Groups[4].Value, 8, Vector2.Zero, new Vector2(GameBase.WindowBaseSize.Width - 120, 60), 1, true, Color4.White, false);
            text.TextBold = true;
            text.Offset = new Vector2(11, 25);
            SpriteCollection.Add(text);

        }

        private void Select()
        {
            isSelected = true;
            currentlySelectedPanel = this;

            backingPlate.Colour = Color4.Orange;

        }

        private void Deselect()
        {
            isSelected = false;

            backingPlate.Colour = Color4.OrangeRed;
        }
        internal void MoveTo(Vector2 location)
        {
            SpriteCollection.ForEach(s => s.MoveTo(location, 150));
        }
    }
}

