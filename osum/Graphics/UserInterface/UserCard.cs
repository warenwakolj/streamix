using System;
using osum.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;

internal class UserCard : pSpriteCollection
{
    pSprite backingPlate;
    pText text;
    pText undertext;

    public static string Username { get; private set; } = "Guest";

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
// soon
        };


        text = new pText(Username, 9, position + new Vector2(50, -1), new Vector2(400, 40), 1, true, Color4.White, false);
        SpriteCollection.Add(text);
        undertext = new pText("osu game ", 6.5f, position + new Vector2(50, 13), new Vector2(400, 40), 1, true, Color4.White, false);
        SpriteCollection.Add(undertext);
    }

    public void RefreshUsername()
    {
        text.Text = Username;  
        undertext.Text = "Welcome to osu!";
    }

    internal void AddToSpriteManager(SpriteManager spriteManager)
    {
        spriteManager.Add(backingPlate);
        spriteManager.Add(undertext);
        spriteManager.Add(text);
    }

    public static void SetUsername(string username)
    {
        Username = username;
        RefreshAllUserCards();
    }

    public static event Action OnUsernameChanged;

    private static void RefreshAllUserCards()
    {
        OnUsernameChanged?.Invoke(); 
    }
}
