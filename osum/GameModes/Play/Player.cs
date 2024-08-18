//  Play.cs
//  Author: Dean Herbert <pe@ppy.sh>
//  Copyright (c) 2010 2010 Dean Herbert
using System;
using osum.GameplayElements;
using osum.GameplayElements.Beatmaps;
using osum.Helpers;
//using osu.Graphics.Renderers;
using osum.Graphics.Primitives;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using osum.Audio;
using osum.Graphics.Renderers;
using osum.GameplayElements.Scoring;
using osum.GameModes.Play.Components;
using osum.Graphics.Sprites;
using osum.Graphics.Skins;
using System.Linq;

using System.Text;
using OpenTK.Input;

namespace osum.GameModes
{
    public class Player : GameMode
    {
        HitObjectManager hitObjectManager;

        HealthBar healthBar;
        ScoreDisplay scoreDisplay;
        ComboCounter comboCounter;
        private CursorSprite cursorSprite;
        private bool isPaused = false;
        private double pausedTime;

        Score currentScore;

        static Beatmap Beatmap;
        public static bool Autoplay;

        public Player() : base()
        {
        }

        private void DisplayBeatmapDifficultySettings()
        {
            Console.WriteLine("Beatmap Difficulty Settings:");
            Console.WriteLine($"- Overall Difficulty (OD): {Beatmap.DifficultyOverall}");
            Console.WriteLine($"- Circle Size (CS): {Beatmap.DifficultyCircleSize}");
            Console.WriteLine($"- HP Drain Rate (HP): {Beatmap.DifficultyHpDrainRate}");
            Console.WriteLine($"- Approach Rate (AR): {Beatmap.DifficultyApproachRate}");
        }

        private void ApplyCircleSizeModifier()
        {
            float circleSize = Beatmap.DifficultyCircleSize;
            DifficultyManager.SetHitObjectRadius(circleSize);

            Console.WriteLine($"Applied Circle Size Modifier: CS={circleSize}, Radius={DifficultyManager.HitObjectRadius}");
        }




        void InputManager_OnDown(InputSource source, TrackingPoint point)
        {
            //pass on the event to hitObjectManager for handling.
            hitObjectManager.HandlePressAt(point);
        }

        private void ApplyPreEmptModifier()
        {
            float approachRate = Beatmap.DifficultyApproachRate;

            //dk if this actually works for actual ar0 diffs

            DifficultyManager.SetPreEmptAndFadeIn(approachRate);

            Console.WriteLine($"Applied Approach Rate Modifier: AR={approachRate}, PreEmpt={DifficultyManager.PreEmpt}, FadeIn={DifficultyManager.FadeIn}");
        }



        private void ApplyHitWindowModifier()
        {
            float overallDifficulty = Beatmap.DifficultyOverall;
            DifficultyManager.SetHitWindows(overallDifficulty);

            Console.WriteLine($"Applied Overall Difficulty Modifier: OD={overallDifficulty}, HitWindow300={DifficultyManager.HitWindow300}, HitWindow100={DifficultyManager.HitWindow100}, HitWindow50={DifficultyManager.HitWindow50}");
        }

        internal override void Initialize()
        {
            Console.WriteLine("Initializing Player mode.");

            InputManager.OnDown += new InputHandler(InputManager_OnDown);

            hitObjectManager = new HitObjectManager(Beatmap);
            hitObjectManager.OnScoreChanged += new ScoreChangeDelegate(hitObjectManager_OnScoreChanged);

            hitObjectManager.LoadFile();

            DisplayBeatmapDifficultySettings();

            ApplyCircleSizeModifier();
            ApplyPreEmptModifier();
            ApplyHitWindowModifier();

            healthBar = new HealthBar();
            scoreDisplay = new ScoreDisplay();
            comboCounter = new ComboCounter();
            currentScore = new Score();
            cursorSprite = new CursorSprite();
            cursorSprite.AddToSpriteManager(spriteManager);

            AudioEngine.Music.Load(Beatmap.GetFileBytes(Beatmap.AudioFilename));
            AudioEngine.Music.Play();
        }





        public override void Dispose()
        {
            Console.WriteLine("Disposing Player resources.");
            InputManager.OnDown -= new InputHandler(InputManager_OnDown);

            AudioEngine.Music.Stop();

            hitObjectManager.Dispose();
            healthBar.Dispose();
            scoreDisplay.Dispose();
            comboCounter.Dispose();

            base.Dispose();
        }

        void hitObjectManager_OnScoreChanged(ScoreChange change, HitObject hitObject)
        {
            //handle the score addition
            switch (change & ~ScoreChange.ComboAddition)
            {
                case ScoreChange.SpinnerBonus:
                    currentScore.totalScore += 1000;
                    break;
                case ScoreChange.SpinnerSpinPoints:
                    currentScore.totalScore += 500;
                    break;
                case ScoreChange.SpinnerSpin:
                    break;
                case ScoreChange.SliderRepeat:
                case ScoreChange.SliderEnd:
                    currentScore.totalScore += 30;
                    comboCounter.IncreaseCombo();
                    break;
                case ScoreChange.SliderTick:
                    currentScore.totalScore += 10;
                    comboCounter.IncreaseCombo();
                    break;
                case ScoreChange.Hit50:
                    currentScore.totalScore += 50;
                    currentScore.count50++;
                    comboCounter.IncreaseCombo();
                    break;
                case ScoreChange.Hit100:
                    currentScore.totalScore += 100;
                    currentScore.count100++;
                    comboCounter.IncreaseCombo();
                    break;
                case ScoreChange.Hit300:
                    currentScore.totalScore += 300;
                    currentScore.count300++;
                    comboCounter.IncreaseCombo();
                    break;
                case ScoreChange.Miss:
                    currentScore.countMiss++;
                    comboCounter.SetCombo(0);
                    break;
            }

            //then handle the hp addition
            switch (change)
            {
                case ScoreChange.Miss:
                    healthBar.ReduceCurrentHp(20);
                    break;
                default:
                    healthBar.IncreaseCurrentHp(5);
                    break;
            }

            scoreDisplay.SetScore(currentScore.totalScore);
            scoreDisplay.SetAccuracy(currentScore.accuracy * 100);
        }

        public void Pause()
        {
            if (isPaused) return;

            isPaused = true;
            pausedTime = AudioEngine.Music.CurrentTime;
            AudioEngine.Music.Pause();
        }

        public void Resume()
        {
            if (!isPaused) return;

            isPaused = false;
            AudioEngine.Music.SetCurrentTime(pausedTime);
            AudioEngine.Music.Play();
        }

        public override void Update()
        {
            if (hitObjectManager.AllNotesHit)
            {
                Ranking.RankableScore = currentScore;

                Director.ChangeMode(OsuMode.Ranking);
                return;
            }

            hitObjectManager.Update();
            cursorSprite.Update();
            healthBar.Update();
            scoreDisplay.Update();
            comboCounter.Update();

            if (isPaused)
            {
                var keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Key.Escape))
                {
                    Director.ChangeMode(OsuMode.Play);
                }

                return;
            }

            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.Escape))
            {
                Director.ChangeMode(OsuMode.Pause);
            }

            base.Update();
        }

        public override void Draw()
        {
            hitObjectManager.Draw();

            scoreDisplay.Draw();
            healthBar.Draw();
            comboCounter.Draw();

            base.Draw();
        }

        internal static void SetBeatmap(Beatmap beatmap)
        {
            Beatmap = beatmap;
        }
    }
}
