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

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            if (isSelected != value)
            {
                isSelected = value;
                if (isSelected)
                    Select();
                else
                    Deselect();

                OnStateChanged?.Invoke(isSelected);
            }
        }
    }

    public delegate void StateChangedHandler(bool isSelected);
    public event StateChangedHandler OnStateChanged;

    internal CheckBox(Vector2 position, string textContent)
    {
        backingPlate = pSprite.FullscreenWhitePixel;
        backingPlate.Alpha = 1;
        backingPlate.AlwaysDraw = true;
        backingPlate.Colour = Color4.OrangeRed;
        backingPlate.Scale = new Vector2(35, 35);
        backingPlate.DrawDepth = 0.8f;
        backingPlate.Position = position;
        SpriteCollection.Add(backingPlate);

        backingPlate.OnClick += delegate {
            IsSelected = !IsSelected;  // Toggle selection
        };

        backingPlate.HandleClickOnUp = true;

        text = new pText(textContent, 10, position + new Vector2(28, 3), new Vector2(400, 40), 1, true, Color4.White, false);
        SpriteCollection.Add(text);
    }

    private void Select()
    {
        backingPlate.Colour = Color4.Orange;
    }

    private void Deselect()
    {
        backingPlate.Colour = Color4.OrangeRed;
    }

    internal void AddToSpriteManager(SpriteManager spriteManager)
    {
        spriteManager.Add(backingPlate);
        spriteManager.Add(text);
    }
}
