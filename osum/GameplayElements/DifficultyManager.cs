using System;

namespace osum.GameplayElements
{
    internal static class DifficultyManager
    {
        public static float HitObjectRadiusDefault { get { return 64 * GameBase.SpriteRatioToWindowBase; } }

        /// <summary>
        /// Radius of hitObjects in a gamefield.
        /// </summary>
        private static float hitObjectRadius = 48 * GameBase.SpriteRatioToWindowBase;
        public static float HitObjectRadius
        {
            get { return hitObjectRadius; }
            private set { hitObjectRadius = value; }
        }

        public static float HitObjectSizeModifier = 1f;

        public static int SliderVelocity = 300;

        internal static int PreEmpt { get { return 1000; } }
        internal static int PreEmptSnakeStart { get { return 1000; } }
        internal static int PreEmptSnakeEnd { get { return 500; } }
        internal static int HitWindow50 { get { return 150; } }
        internal static int HitWindow100 { get { return 100; } }
        internal static int HitWindow300 { get { return 50; } }
        internal static int FadeIn { get { return 400; } }
        internal static int FadeOut { get { return 300; } }
        internal static int SpinnerRotationRatio { get { return 5; } }
        internal static int DistanceBetweenTicks { get { return 30; } }

        internal static int FollowLineDistance = 32;
        internal static int FollowLinePreEmpt = 800;

        /// <summary>
        /// Set the hit object radius based on the CS from the beatmap
        /// </summary>
        /// <param name="circleSize">The circle size value</param>
        public static void SetHitObjectRadius(float circleSize)
        {
            HitObjectRadius = 54.4f - 4.48f * circleSize;
            Console.WriteLine($"HitObjectRadius set to: {HitObjectRadius}");
        }
    }
}
