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
        public static void SetHitWindows(float overallDifficulty)
        {
            int hitWindow300 = (int)(80 - 6 * overallDifficulty);
            int hitWindow100 = (int)(140 - 8 * overallDifficulty);
            int hitWindow50 = (int)(200 - 10 * overallDifficulty);

            Console.WriteLine($"HitWindow300 set to: {hitWindow300}");
            Console.WriteLine($"HitWindow100 set to: {hitWindow100}");
            Console.WriteLine($"HitWindow50 set to: {hitWindow50}");

            _hitWindow300 = hitWindow300;
            _hitWindow100 = hitWindow100;
            _hitWindow50 = hitWindow50;
        }

        private static int _hitWindow300 = 50;
        public static int HitWindow300 { get { return _hitWindow300; } }

        private static int _hitWindow100 = 100;
        public static int HitWindow100 { get { return _hitWindow100; } }

        private static int _hitWindow50 = 150;
        public static int HitWindow50 { get { return _hitWindow50; } }

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
