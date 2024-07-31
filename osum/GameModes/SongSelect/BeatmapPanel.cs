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
        Beatmap beatmap;

        pSprite backingPlate;
        pText text;

        internal BeatmapPanel(Beatmap beatmap)
        {
            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 1;
            backingPlate.AlwaysDraw = true;
            backingPlate.Colour = Color4.OrangeRed;
            backingPlate.Scale.Y = 80;
            backingPlate.DrawDepth = 0.8f;
            SpriteCollection.Add(backingPlate);

            this.beatmap = beatmap;

            backingPlate.OnClick += delegate {

                backingPlate.UnbindAllEvents();

                backingPlate.Colour = Color4.LightSkyBlue;

                Player.SetBeatmap(beatmap);
                Director.ChangeMode(OsuMode.Play);
            };

            backingPlate.HandleClickOnUp = true;

            backingPlate.OnHover += delegate { backingPlate.Colour = Color4.YellowGreen; };
            backingPlate.OnHoverLost += delegate { backingPlate.Colour = Color4.OrangeRed; };

            string filename = Path.GetFileNameWithoutExtension(beatmap.BeatmapFilename);

            Regex r = new Regex(@"(.*) - (.*) \((.*)\) \[(.*)\]");
            Match m = r.Match(filename);

            //song name 
            text = new pText(m.Groups[2].Value, 10, Vector2.Zero, new Vector2(400, 80), 1, true, Color4.White, false);
            text.Offset = new Vector2(10, 0);
            SpriteCollection.Add(text);


            //artist // mapper
            text = new pText(m.Groups[3].Value + " // " + m.Groups[1].Value, 6, Vector2.Zero, new Vector2(GameBase.WindowBaseSize.Width - 120, 60), 1, true, Color4.White, false);
            text.Offset = new Vector2(11, 15);
            SpriteCollection.Add(text);

            //difficulty
            text = new pText(m.Groups[4].Value, 8, Vector2.Zero, new Vector2(GameBase.WindowBaseSize.Width - 120, 60), 1, true, Color4.White, false);
            text.TextBold = true;
            text.Offset = new Vector2(10, 84);
            SpriteCollection.Add(text);
        }

        internal void MoveTo(Vector2 location)
        {
            SpriteCollection.ForEach(s => s.MoveTo(location, 150));
        }
    }
}

