using UnityEngine;
using System.Collections;
using Assets.Global;
using Assets.Other;

public class SettingsViewBehaviour : SlideBehaviour {

    [SerializeField]
    TableView table;

    SettingsModel settings;

    public override void OnSlide(params object[] args)
    {

        settings = new SettingsModel();

        table.Clear();

        table.SetModel<ValueOption>(
            vo => vo.optionName,
            vo => vo
            );

        table.AddRange(new ValueOption[]
        {
            new ValueOption() { optionName = "Language", value = settings.language, setAction = v => settings.language = (Language)v },
            new ValueOption() { optionName = "Sound", value = settings.sound, setAction = v => settings.sound = (bool)v },

        });
        GetComponent<ThemeListener>().UpdateTheme();
    }

    public void Apply()
    {
        settings.Apply();
        settings = null;


        GameState.GoToMainMenu(); // reload scene
    }

    public void Cancel()
    {
        settings = null;
        screenSlider.MoveTo("MainMenu");
    }

}

public class SettingsModel
{
    public Language language;
    public bool sound;

    public SettingsModel()
    {
        language = Localization.instance.lang;
        sound = bool.Parse(GameConfig.Get("Sound", "true"));
    }

    public void Apply()
    {
        Localization.instance.lang = language;

        GameConfig.Set("localization", language.ToString());
        GameConfig.Set("Sound", sound.ToString());
        GameConfig.Save();
    }

}
