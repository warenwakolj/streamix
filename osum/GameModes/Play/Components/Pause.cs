using OpenTK.Graphics;
using OpenTK;
using osum.GameModes;
using osum.Graphics.Skins;
using osum.Graphics.Sprites;
using osum.Helpers;
using osum;

public class PauseMenu : GameMode
{
    private pSprite resumeButton;
    private pSprite OptionsButton;
    private pSprite background;
    private CursorSprite cursorSprite;

    public PauseMenu()
    {
        Initialize();
    }

    internal override void Initialize()
    {
        background = new pSprite(TextureManager.Load("menu-background"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                 ClockTypes.Game, Vector2.Zero, 1, true, new Color4(0, 0, 0, 0.8f));
        spriteManager.Add(background);

        resumeButton = new pSprite(TextureManager.Load("pause-continue"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                   ClockTypes.Game, new Vector2(0, -50), 1, true, Color4.White);
        resumeButton.Scale = new Vector2(0.5f, 0.5f);
        resumeButton.OnClick += delegate
        {
            var player = Director.CurrentMode as Player;
            if (player != null)
            {
                player.Resume();
            }
            Director.ChangeMode(OsuMode.Play);
        };
        spriteManager.Add(resumeButton);

        OptionsButton = new pSprite(TextureManager.Load("pause-back"), FieldTypes.StandardSnapCentre, OriginTypes.Centre,
                                       ClockTypes.Game, new Vector2(0, 50), 1, true, Color4.White);
        OptionsButton.Scale = new Vector2(0.5f, 0.5f);
        OptionsButton.OnClick += delegate
        {
            Director.ChangeMode(OsuMode.SongSelect);
        };
        spriteManager.Add(OptionsButton);
        cursorSprite = new CursorSprite();
        cursorSprite.AddToSpriteManager(spriteManager);
    }

    public override void Update()
    {
        base.Update();
        cursorSprite.Update();
    }

    public override void Draw()
    {
        base.Draw();
    }

    public override void Dispose()
    {
        resumeButton.UnbindAllEvents();
        OptionsButton.UnbindAllEvents();

        base.Dispose();
    }
}
