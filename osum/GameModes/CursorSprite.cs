﻿using OpenTK;
using OpenTK.Graphics;
using osum.Graphics.Sprites;
using osum.Graphics;
using osum.Graphics.Skins;
using osum.Helpers;
using System;

namespace osum
{
    public class CursorSprite
    {
        private pSprite cursor;

        public CursorSprite()
        {
            cursor = new pSprite(TextureManager.Load("cursor"), FieldTypes.StandardSnapTopLeft, OriginTypes.Centre, ClockTypes.Mode, Vector2.Zero, 19, true, Color4.White);

            InputManager.OnDown += OnClick;
            InputManager.OnUp += OnClickLost;
        }

        internal void AddToSpriteManager(SpriteManager spriteManager)
        {
            spriteManager.Add(cursor);
        }

        private void OnClick(InputSource source, TrackingPoint point)
        {
            cursor.ScaleTo(1.1f, 100, EasingTypes.In);
        }

        private void OnClickLost(InputSource source, TrackingPoint point)
        {
            cursor.ScaleTo(1f, 100, EasingTypes.In);
        }

        public void Update()
        {
            Vector2 mousePosition = InputManager.MainPointerPosition;
            cursor.Position = mousePosition;
        }

        public void Draw()
        {
            cursor.Draw();
        }
    }

    public class Game
    {
        private CursorSprite cursorSprite;
        private SpriteManager spriteManager;

        public Game()
        {
            spriteManager = new SpriteManager();
            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);
        }

        public void Run()
        {
            while (true)
            {
                cursorSprite.Update();
                InputManager.Update();

                spriteManager.Draw();
            }
        }
    }
}
