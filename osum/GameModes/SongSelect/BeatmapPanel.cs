using System;
using osum.Graphics.Sprites;
using osum.GameplayElements.Beatmaps;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using osum.Helpers;

namespace osum.GameModes.SongSelect
{
    internal class BeatmapPanel : pSpriteCollection
    {
        Beatmap beatmap;
        pSprite backingPlate;
        pSprite text;
        Vector2 originalPosition;
        bool isHovered = false;

        internal BeatmapPanel(Beatmap beatmap)
        {
            backingPlate = pSprite.FullscreenWhitePixel;
            backingPlate.Alpha = 1;
            backingPlate.AlwaysDraw = true;
            backingPlate.Colour = Color4.OrangeRed;
            backingPlate.Scale.Y = 80;
            backingPlate.Scale.X *= 0.5f;
            backingPlate.DrawDepth = 0.1f;
            SpriteCollection.Add(backingPlate);

            this.beatmap = beatmap;

            originalPosition = backingPlate.Position;

            backingPlate.OnClick += delegate {
                backingPlate.MoveTo(backingPlate.Position - new Vector2(50, 0), 600);
                backingPlate.Transform(new Transformation(TransformationType.VectorScale, backingPlate.Scale,
                                                          new Vector2(backingPlate.Scale.X * 1.2f, backingPlate.Scale.Y),
                                                          backingPlate.ClockingNow, backingPlate.ClockingNow + 600));
                backingPlate.UnbindAllEvents();

                Player.SetBeatmap(beatmap);
                Director.ChangeMode(OsuMode.Play);
            };

            backingPlate.HandleClickOnUp = true;

            backingPlate.OnHover += delegate {
                if (!isHovered)
                {
                    isHovered = true;
                    backingPlate.MoveTo(originalPosition - new Vector2(300, 0), 600);
                }
            };

            backingPlate.OnHoverLost += delegate {
                if (isHovered)
                {
                    isHovered = false;
                    backingPlate.MoveTo(originalPosition, 600);
                }
            };

            text = new pText(Path.GetFileNameWithoutExtension(beatmap.BeatmapFilename), 10, Vector2.Zero, Vector2.Zero, 1, true, Color4.White, false);
            SpriteCollection.Add(text);
        }

        internal void MoveTo(Vector2 location)
        {
            text.MoveTo(location + new Vector2(10, 10), 400); 
            backingPlate.MoveTo(location, 400);

            originalPosition = location;
        }
    }
}
