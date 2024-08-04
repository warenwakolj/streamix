using OpenTK.Graphics;
using OpenTK;
using osum.GameplayElements.Beatmaps;
using osum.Graphics.Sprites;
using System.IO;
using System.Text.RegularExpressions;
using osum.Audio;
using osum.GameModes;
using osum;
using System;
using System.Timers;
using System.Collections.Generic;

internal class Dropdown : pSpriteCollection
{
    pSprite backingPlate;
    pSprite outlinePlate; 
    pText text;
    bool isHovered = false;
    bool isSelected = false;
    bool optionsVisible = false;

    List<DropdownOption> options;

    internal Dropdown(string displayText, Vector2 position)
    {
        outlinePlate = pSprite.FullscreenWhitePixel;
        outlinePlate.Alpha = 1;
        outlinePlate.AlwaysDraw = true;
        outlinePlate.Colour = Color4.White; 
        outlinePlate.Scale.Y = 34;          
        outlinePlate.Scale.X = 254;
        outlinePlate.DrawDepth = 5;
        outlinePlate.Position = position - new Vector2(1, 1);  

        SpriteCollection.Add(outlinePlate);

        backingPlate = pSprite.FullscreenWhitePixel;
        backingPlate.Alpha = 1;
        backingPlate.AlwaysDraw = true;
        backingPlate.Colour = Color4.Black;
        backingPlate.Scale.Y = 30;
        backingPlate.Scale.X = 250;
        backingPlate.DrawDepth = 6;
        backingPlate.Position = position;

        SpriteCollection.Add(backingPlate);

        backingPlate.HandleClickOnUp = true;
        backingPlate.OnClick += (sender, e) => ToggleOptions();
        backingPlate.OnHover += (sender, e) => OnHover();
        backingPlate.OnHoverLost += (sender, e) => OnHoverLost();

        text = new pText(displayText, 10, Vector2.Zero, new Vector2(0, 0), 7, true, Color4.White, false);
        text.Position = position + new Vector2(10, 0);
        text.TextBold = true;
        SpriteCollection.Add(text);

        options = new List<DropdownOption>();
    }

    private void OnHover()
    {
        if (!isSelected)
        {
            backingPlate.Colour = Color4.Gray;
        }
    }

    private void OnHoverLost()
    {
        if (!isSelected)
        {
            backingPlate.Colour = Color4.Black;
        }
    }

    public void AddOption(string displayText, Action onClick)
    {
        Vector2 optionPosition = backingPlate.Position + new Vector2(0, (options.Count + 1) * 20);
        DropdownOption option = new DropdownOption(displayText, optionPosition, () =>
        {
            SetMainText(displayText);
            onClick?.Invoke();
        }, HideOptions);
        options.Add(option);

        option.SpriteCollection.ForEach(sprite => sprite.Alpha = 0);
        option.SpriteCollection.ForEach(sprite => SpriteCollection.Add(sprite));
    }

    private void ToggleOptions()
    {
        optionsVisible = !optionsVisible;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].SetVisibility(optionsVisible, i * 50);
        }
    }

    private void HideOptions()
    {
        optionsVisible = false;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].SetVisibility(false, i * 50);
        }
    }

    public void Select()
    {
        isSelected = true;
        backingPlate.Colour = Color4.Gray;
    }

    private void Deselect()
    {
        isSelected = false;
        backingPlate.Colour = Color4.Black;
    }

    private void SetMainText(string displayText)
    {
        text.Text = displayText;
    }
}

internal class DropdownOption : pSpriteCollection
{
    pSprite backingPlate;
    pSprite outlinePlate; 
    pText text;
    Action onHideOptions;
    bool isVisible; 

    internal DropdownOption(string displayText, Vector2 position, Action onClick, Action hideOptions)
    {
        onHideOptions = hideOptions;

        outlinePlate = pSprite.FullscreenWhitePixel;
        outlinePlate.Alpha = 1;
        outlinePlate.AlwaysDraw = true;
        outlinePlate.Colour = Color4.White;
        outlinePlate.Scale.Y = 34;
        outlinePlate.DrawDepth = 5;
        outlinePlate.Scale.X = 254;
        outlinePlate.Position = position - new Vector2(1, 1);

        SpriteCollection.Add(outlinePlate);

        backingPlate = pSprite.FullscreenWhitePixel;
        backingPlate.Alpha = 1;
        backingPlate.AlwaysDraw = true;
        backingPlate.Colour = Color4.Black;
        backingPlate.Scale.Y = 30;
        backingPlate.Scale.X = 250;
        backingPlate.DrawDepth = 6;
        backingPlate.Position = position;

        SpriteCollection.Add(backingPlate);

        backingPlate.OnClick += (sender, e) =>
        {
            if (isVisible) 
            {
                onClick?.Invoke();
                onHideOptions?.Invoke();
            }
        };
        backingPlate.OnHover += (sender, e) => OnHover();
        backingPlate.OnHoverLost += (sender, e) => OnHoverLost();

        text = new pText(displayText, 10, Vector2.Zero, new Vector2(0, 0), 7, true, Color4.White, false);
        text.Position = position + new Vector2(10, 0);
        SpriteCollection.Add(text);
    }

    private void OnHover()
    {
        backingPlate.Colour = Color4.Gray;
    }

    private void OnHoverLost()
    {
        backingPlate.Colour = Color4.Black;
    }

    public void SetVisibility(bool visible, int delay)
    {
        isVisible = visible;
        if (visible)
        {
            float targetAlpha = 1;
            float duration = 50;

            if (delay > 0)
            {
                Timer timer = new Timer();
                timer.Interval = delay;
                timer.AutoReset = false;
                timer.Elapsed += (sender, e) =>
                {
                    timer.Dispose();
                    FadeTo(targetAlpha, duration);
                };
                timer.Start();
            }
            else
            {
                FadeTo(targetAlpha, duration);
            }
        }
        else
        {
            SpriteCollection.ForEach(sprite => sprite.Alpha = 0);
        }
    }

    private void FadeTo(float targetAlpha, float duration)
    {
        float initialAlpha = SpriteCollection[0].Alpha;
        float alphaChange = targetAlpha - initialAlpha;
        float elapsedTime = 0;

        Timer fadeTimer = new Timer();
        fadeTimer.Interval = 16;
        fadeTimer.Elapsed += (sender, e) => {
            elapsedTime += 16;
            float progress = Math.Min(elapsedTime / duration, 1);
            float newAlpha = initialAlpha + alphaChange * progress;
            SpriteCollection.ForEach(sprite => sprite.Alpha = newAlpha);

            if (progress >= 1)
            {
                fadeTimer.Stop();
                fadeTimer.Dispose();
            }
        };
        fadeTimer.Start();
    }
}
