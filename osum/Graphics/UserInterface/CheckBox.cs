using System;
using osum.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;

internal class CheckBox : pSpriteCollection
{
    pSprite backingPlate;
    pText text;
    bool isHovered = false;
    bool isSelected = false;

    public delegate void StateChangedHandler(bool isSelected);
    public event StateChangedHandler OnStateChanged;

    internal CheckBox(Vector2 position, string textContent)
    {
        backingPlate = pSprite.FullscreenWhitePixel;
        backingPlate.Alpha = 1;
        backingPlate.AlwaysDraw = true;
        backingPlate.Colour = Color4.OrangeRed;
        backingPlate.Scale = new Vector2(40, 40);
        backingPlate.DrawDepth = 0.8f;
        backingPlate.Position = position;
        SpriteCollection.Add(backingPlate);

        backingPlate.OnClick += delegate {
            if (isSelected)
            {
                Deselect();
            }
            else
            {
                Select();
            }

            OnStateChanged?.Invoke(isSelected);
        };

        backingPlate.HandleClickOnUp = true;

        text = new pText(textContent, 10, position + new Vector2(50, 0), new Vector2(400, 40), 1, true, Color4.White, false);
        SpriteCollection.Add(text);
    }

    private void Select()
    {
        isSelected = true;
        backingPlate.Colour = Color4.Orange;
    }

    private void Deselect()
    {
        isSelected = false;
        backingPlate.Colour = Color4.OrangeRed;
    }

    internal void AddToSpriteManager(SpriteManager spriteManager)
    {
        spriteManager.Add(backingPlate);
        spriteManager.Add(text);
    }
}
