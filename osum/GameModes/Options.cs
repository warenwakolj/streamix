using System;
using osum.GameplayElements.Scoring;
using osum.Graphics.Sprites;
using osum.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using osum.Graphics.Skins;
using System.Drawing;

namespace osum.GameModes
{
    public class Options : GameMode
    {
        internal static Score RankableScore;
        internal static pText Text;
        private pSprite menuBackground;
        pSprite HeaderBg;

        private CursorSprite cursorSprite;

        private GameWindowDesktop gameWindow; 
        private pSpriteCollection SpriteCollection;

        List<pSprite> fillSprites = new List<pSprite>();

        float actualSpriteScaleX;

        public Options()
        {
            gameWindow = new GameWindowDesktop();
        }

        internal override void Initialize()
        {
            HeaderBg = pSprite.FullscreenWhitePixel;
            HeaderBg.Alpha = 1;
            HeaderBg.AlwaysDraw = true;
            HeaderBg.Colour = Color4.Black;
            HeaderBg.Scale = new Vector2(2000, 105);
            HeaderBg.DrawDepth = 0.8f;
            spriteManager.Add(HeaderBg);

            menuBackground = new pSprite(TextureManager.Load(@"menu-background"), FieldTypes.StandardSnapBottomLeft, OriginTypes.BottomLeft,
                                         ClockTypes.Game, Vector2.Zero, 0, true, new Color4(1, 1, 1, 0.5f));
            spriteManager.Add(menuBackground);

            pSprite backButton = BackButton.CreateBackButton(OsuMode.MainMenu);
            spriteManager.Add(backButton);

            Text = new pText("Options", 20, Vector2.Zero, new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(Text);

            Vector2 optionsTextSize = Text.MeasureText();

            Text = new pText("Change the way osu! behaves", 11, new Vector2(0, optionsTextSize.Y - 20), new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(Text);

            Text = new pText("Renderer", 17, new Vector2(100, 100), new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(Text);

            Text = new pText("Input", 17, new Vector2(400, 100), new Vector2(0, 0), 1, true, Color4.White, false);
            spriteManager.Add(Text);

            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);

            CheckBox fullscreenCheckBox = new CheckBox(new Vector2(10, 150), "Fullscreen");
            fullscreenCheckBox.OnStateChanged += (isSelected) =>
            {
                gameWindow.ToggleFullscreen();
            };
            fullscreenCheckBox.AddToSpriteManager(spriteManager);

            CheckBox vsyncCheckBox = new CheckBox(new Vector2(150, 150), "VSync");
            vsyncCheckBox.OnStateChanged += (isSelected) =>
            {
                gameWindow.ToggleVSync();
            };
            vsyncCheckBox.AddToSpriteManager(spriteManager);

            CheckBox cursorCheckBox = new CheckBox(new Vector2(300, 150), "Show windows cursor");
            cursorCheckBox.OnStateChanged += (isSelected) =>
            {
                gameWindow.ToggleCursor();
            };
            cursorCheckBox.AddToSpriteManager(spriteManager);
            CheckBox oldScorebarCheckBox;
            oldScorebarCheckBox = new CheckBox(new Vector2(450, 150), "Old Scorebar Style");
            oldScorebarCheckBox.IsSelected = SettingsManager.GetSetting<bool>("OldScorebarStyle");
            oldScorebarCheckBox.OnStateChanged += (isSelected) =>
            {
                SettingsManager.SaveSetting("OldScorebarStyle", isSelected);
            };

            oldScorebarCheckBox.AddToSpriteManager(spriteManager);

            OptionsButton loginButton = new OptionsButton("Login", new Vector2(100, 300), new Vector2(200, 50), Color4.DarkSlateGray);
            loginButton.OnClick += () =>
            {
                ShowLoginForm();
            };
            loginButton.SpriteCollection.ForEach(sprite => spriteManager.Add(sprite));


            AddDropdown();

        }

        private void ShowLoginForm()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new LoginForm());
        }


        private void AddDropdown()
        {
            Dropdown dropdown = new Dropdown("Select an option", new Vector2(100, 200));
            dropdown.AddOption("Option 1", () => { /* action for option */ });
            dropdown.AddOption("Option 2", () => { });
            dropdown.AddOption("Option 3", () => { });
            dropdown.SpriteCollection.ForEach(sprite => spriteManager.Add(sprite));
        }


        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
            cursorSprite.Update();
        }

     

    }
}
