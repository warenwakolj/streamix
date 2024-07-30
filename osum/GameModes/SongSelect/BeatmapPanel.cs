using System;
using osum.Graphics.Sprites;
using osum.GameplayElements.Beatmaps;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using osum.Helpers;
using osum;  // Ensure this is included to access SongSelect class

namespace osum.GameModes.SongSelect
{
    internal class BeatmapPanel : pSpriteCollection
    {
        public Beatmap Beatmap { get; private set; }

        pSprite backingPlate;
        pSprite text;
        Vector2 originalPosition;
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
            backingPlate.Scale.X *= 0.5f;
            backingPlate.DrawDepth = 0.1f;
            SpriteCollection.Add(backingPlate);

            originalPosition = backingPlate.Position;

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

        private void Select()
        {
            isSelected = true;
            currentlySelectedPanel = this;

            backingPlate.Colour = Color4.Orange;

          
           // SongSelect.UpdateMetadataText(Path.GetFileNameWithoutExtension(Beatmap.BeatmapFilename));
        }

        private void Deselect()
        {
            isSelected = false;

            backingPlate.Colour = Color4.OrangeRed;
        }
    }
}
