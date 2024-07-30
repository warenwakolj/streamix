using OpenTK;
using OpenTK.Graphics;
using osum;
using osum.GameModes;
using osum.Graphics.Skins;
using osum.Graphics.Sprites;
using osum.Helpers;
using osum.Audio;


public static class BackButton
{
    internal static pSprite CreateBackButton(OsuMode targetMode)
    {
        pSprite backButton = new pSprite(TextureManager.Load("menu-back"), FieldTypes.StandardSnapBottomLeft, OriginTypes.BottomLeft,
                                         ClockTypes.Game, Vector2.Zero, 1, true, new Color4(1, 1, 1, 0.4f));

        backButton.OnClick += delegate {
            AudioEngine.PlaySample(OsuSamples.MenuBack);
            backButton.UnbindAllEvents();
            Director.ChangeMode(targetMode);
        };

        return backButton;
    }
}
