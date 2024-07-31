using OpenTK;
using OpenTK.Graphics;
using osum.Graphics.Sprites;
using osum.Graphics;
using osum.Graphics.Skins;
using osum.Helpers;

namespace osum
{
    public class CursorSprite
    {
        private pSprite cursor;

        public CursorSprite()
        {
            cursor = new pSprite(TextureManager.Load("cursor"), FieldTypes.StandardSnapTopLeft, OriginTypes.TopLeft, ClockTypes.Mode, Vector2.Zero, 19, true, Color4.White);
        }

        internal void AddToSpriteManager(SpriteManager spriteManager)
        {
            spriteManager.Add(cursor);
        }

        public void Update()
        {
            Vector2 mousePosition = InputManager.MainPointerPosition;
            cursor.Position = new Vector2(mousePosition.X - cursor.Width / 4, mousePosition.Y - cursor.Height / 4);
        }

        public void Draw()
        {
            cursor.Draw();
        }

    }
}
