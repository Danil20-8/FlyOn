using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Models
{
    public class ColorTheme
    {
        public readonly Color BackColor;
        public readonly Color ContentColor;
        public readonly Color ItemColor;

        public ColorTheme(string path)
        {
            BackColor = new Color(.4f, .4f, .8f);
            ContentColor = new Color(.15f, .4f, .8f);
            ItemColor = new Color(.8f, .8f, .2f);
        }
    }
}
