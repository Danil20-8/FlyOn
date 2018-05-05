using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
namespace Assets.Models
{
    public class ColorThemeObserver
    {
        event UnityAction<ColorTheme> themeChanged;
        public ColorTheme colorTheme { get; private set; }

        public ColorThemeObserver(ColorTheme theme)
        {
            SetTheme(theme);
        }

        public void SetTheme(ColorTheme theme)
        {
            colorTheme = theme;
            if (themeChanged != null)
                themeChanged(colorTheme);
        }
        public void AddListener(UnityAction<ColorTheme> listener)
        {

            themeChanged += listener;
            listener(colorTheme);
        }
    }
}
