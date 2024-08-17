using System;
using osum.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;

internal class UserCard : pSpriteCollection
{
    pSprite backingPlate;
    pText text;
    pText undertext;
    public delegate void StateChangedHandler(bool isSelected);
    public event StateChangedHandler OnStateChanged;

    internal UserCard(Vector2 position)
    {
        backingPlate = pSprite.FullscreenWhitePixel;
        backingPlate.Alpha = 1;
        backingPlate.AlwaysDraw = true;
        backingPlate.Colour = Color4.White;
        backingPlate.Scale = new Vector2(75, 75);
        backingPlate.DrawDepth = 0.8f;
        backingPlate.Position = position;
        SpriteCollection.Add(backingPlate);

        backingPlate.OnClick += delegate {

        };

        text = new pText("Guest", 9, position + new Vector2(50, -1), new Vector2(400, 40), 1, true, Color4.White, false);
        SpriteCollection.Add(text);
        undertext = new pText("Please click here to login", 6.5f, position + new Vector2(50, 13), new Vector2(400, 40), 1, true, Color4.White, false);
        SpriteCollection.Add(undertext);
    }


    internal void AddToSpriteManager(SpriteManager spriteManager)
    {
        spriteManager.Add(backingPlate);
        spriteManager.Add(undertext);
        spriteManager.Add(text);
    }
}
