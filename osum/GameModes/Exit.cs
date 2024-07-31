using System;
using osum.GameplayElements.Scoring;
using osum.Graphics.Sprites;
using osum.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace osum.GameModes
{
    public class Exit : GameMode
    {

        internal override void Initialize()
        {
            Environment.Exit(0);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
