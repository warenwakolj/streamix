using OpenTK.Graphics;
using OpenTK;
using osum.GameModes;
using osum.Graphics.Skins;
using osum.Graphics.Sprites;
using osum.Helpers;
using osum;
using osum.GameModes.MainMenu;

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
        background = new pSprite(TextureManager.Load("menu-background"), FieldTypes.Standard, OriginTypes.TopLeft,
                                 ClockTypes.Game, Vector2.Zero, 1, true, new Color4(0, 0, 0, 0.8f));
        spriteManager.Add(background);

        PauseButton PauseContinue = new PauseButton(OsuMode.Play, @"pause-continue");
        spriteManager.Add(PauseContinue);
        PauseContinue.SetPosition(new Vector2(180, 130));

        PauseButton PauseRetry = new PauseButton(OsuMode.Play, @"pause-retry");
        spriteManager.Add(PauseRetry);
        PauseRetry.SetPosition(new Vector2(180, 200));

        PauseButton PauseBack = new PauseButton(OsuMode.SongSelect, @"pause-back");
        spriteManager.Add(PauseBack);
        PauseBack.SetPosition(new Vector2(180, 270));

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


        base.Dispose();
    }
}
