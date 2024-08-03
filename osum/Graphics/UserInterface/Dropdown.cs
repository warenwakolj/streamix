using OpenTK.Graphics;
using OpenTK;
using osum.Graphics.Sprites;
using System;
using System.IO;
using osum;

internal class Dropdown : pSpriteCollection
{
    pSprite backingPlate;
    pText text;
    bool isHovered = false;
    bool isSelected = false;

    private static Dropdown currentlySelectedPanel;

    internal Dropdown(string displayText)
    {
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
                Console.WriteLine("Panel clicked while selected.");
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

        text = new pText(displayText, 10, Vector2.Zero, new Vector2(400, 80), 1, true, Color4.White, false);
        text.Offset = new Vector2(10, 0);
        SpriteCollection.Add(text);
    }

    public void Select()
    {
        Console.WriteLine("Panel selected");
        isSelected = true;
        currentlySelectedPanel = this;
        backingPlate.Colour = Color4.Orange;

        // Update metadata display with the displayText
        SongSelect.UpdateMetadataText(text.Text);
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
