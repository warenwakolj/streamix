using System;

namespace osum.GameplayElements
{
    internal static class DifficultyManager
    {
        public static float HitObjectRadiusDefault { get { return 64 * GameBase.SpriteRatioToWindowBase; } }

        private static float hitObjectRadius = 48 * GameBase.SpriteRatioToWindowBase;
        public static float HitObjectRadius
        {
            get { return hitObjectRadius; }
            private set { hitObjectRadius = value; }
        }

        public static float HitObjectSizeModifier = 1f;

        public static int SliderVelocity = 300;

        private static int preEmpt = 1000;

        internal static int PreEmpt { get { return preEmpt; } }

        internal static int PreEmptSnakeStart { get { return 1000; } }
        internal static int PreEmptSnakeEnd { get { return 500; } }
        internal static int HitWindow50 { get { return 150; } }
        internal static int HitWindow100 { get { return 100; } }
        internal static int HitWindow300 { get { return 50; } }
        internal static int FadeIn { get; private set; } = 400;
        internal static int FadeOut { get { return 300; } }
        internal static int SpinnerRotationRatio { get { return 5; } }
        internal static int DistanceBetweenTicks { get { return 30; } }

        internal static int FollowLineDistance = 32;
        internal static int FollowLinePreEmpt = 800;

        /// <summary>
        /// Set the hit object radius based on the CS from the beatmap.
        /// </summary>
        /// <param name="circleSize">The Circle Size value.</param>
        public static void SetHitObjectRadius(float circleSize)
        {
            HitObjectRadius = 54.4f - 4.48f * circleSize;
            Console.WriteLine($"HitObjectRadius set to: {HitObjectRadius}");
        }

        /// <summary>
        /// Set the preempt time and fade-in time based on the AR from the beatmap
        /// </summary>
        /// <param name="approachRate">The Approach Rate value.</param>
        public static void SetPreEmptAndFadeIn(float approachRate)
        {
            if (approachRate < 5)
            {
                preEmpt = (int)(1200 + 600 * (5 - approachRate) / 5);
                FadeIn = (int)(800 + 400 * (5 - approachRate) / 5);
            }
            else
            {
                preEmpt = (int)(1200 - 750 * (approachRate - 5) / 5);
                FadeIn = (int)(800 - 500 * (approachRate - 5) / 5);
            }

            Console.WriteLine($"PreEmpt set to: {preEmpt}, FadeIn set to: {FadeIn}");
        }
    }

}
