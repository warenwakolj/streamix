using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using osum.GameModes;
using osum.Support;

namespace osum
{
    class GameWindowDesktop : GameWindow
    {
        private bool isFullscreen;
        private bool isVSyncEnabled;

        public GameWindowDesktop() : base(960, 640, GraphicsMode.Default, "osu!")
        {
            VSync = VSyncMode.Off;
            isFullscreen = false; 
            isVSyncEnabled = false; 
        }

        public void Run()
        {
            base.Run();
        }

        public void ToggleFullscreen()
        {
            if (isFullscreen)
            {
                WindowState = WindowState.Normal;
                DisplayDevice.Default.RestoreResolution();
            }
            else
            {
                WindowState = WindowState.Fullscreen;
            }
            isFullscreen = !isFullscreen;
        }

        public void ToggleVSync()
        {
            if (isVSyncEnabled)
            {
                VSync = VSyncMode.Off;
            }
            else
            {
                VSync = VSyncMode.On;
            }
            isVSyncEnabled = !isVSyncEnabled;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);

            GameBase.Instance.Initialize();
            KeyPress += new EventHandler<KeyPressEventArgs>(GameWindowDesktop_KeyPress);
        }

        void GameWindowDesktop_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'a':
                    Player.Autoplay = !Player.Autoplay;
                    break;
                case 'f':
                    ToggleFullscreen();
                    break;
                case 'v':
                    ToggleVSync();
                    break;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (Director.CurrentOsuMode != OsuMode.MainMenu)
            {
                e.Cancel = true;
                Director.ChangeMode(OsuMode.MainMenu, new FadeTransition(200, 400));
            }
            base.OnClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GameBase.Instance.SetupScreen();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GameBase.Instance.Draw(e);

            SwapBuffers();
        }
    }
}
