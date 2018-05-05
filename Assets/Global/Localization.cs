using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using Assets.Serialization;
using System.IO;
using Assets.Other;

namespace Assets.Global
{
    public class Localization
    {
        MyDictionary<Language, MyDictionary<LocalizationContext, Dictionary<string, string>>> locs;
        MyDictionary<Language, MyDictionary<LocalizationContext, HashSet<string>>> notFound = new MyDictionary<Language, MyDictionary<LocalizationContext, HashSet<string>>>();

        public Language lang { get; set; }
        public LocalizationContext context { get; set; }
        public bool dontLocalize { get; set; }

        public static readonly LocalizationContext globalContext = new LocalizationContext("Loc");


        public static readonly Localization instance = new Localization();

        static Localization()
        {
            instance = new Localization((Language)Enum.Parse(typeof(Language), GameConfig.Get("localization", Language.English.ToString())), globalContext);
        }

        public string GetString(Language lang, LocalizationContext context, string key)
        {
            if (dontLocalize)
                if (this.context == context)
                    return key;

            try
            {
                return locs[lang][context][key];
            }
            catch
            {
                try {
                    LoadLang(lang, context);
                    return locs[lang][context][key];
                }
                catch
                {
                }
            }

            notFound[lang][context].Add(key);
            SaveNotFound();
            return "%" + key + "%";
        }

        public string GetString(LocalizationContext context, string key)
        {
            return GetString(lang, context, key);
        }

        public string GetString(string key)
        {
            return GetString(lang, context, key);
        }
        public static string GetGlobalString(string key)
        {
            return instance.GetString(globalContext, key);
        }
        void LoadLang(Language lang, LocalizationContext context)
        {
            Dictionary<string, string> strings =
                GameResources.Load(context.full + @"\" + context.last + "-" + lang.ToString() + ".loc", sr =>
                    new CompactSerializer().Deserialize<Dictionary<string, string>>(sr, typeof(DictionaryBinder<string, string>))
                );

            locs[lang][context] = strings;
        }

        void SaveNotFound()
        {
            foreach (var l in notFound.ByElements())
                foreach (var c in l.value.ByElements())
                    GameResources.Save(c.key.full + @"\NotFoundLoacalization-" + c.key.last + "-" + l.key + ".txt", c.value.ToList(), () => true);
        }

        public Localization(Language lang, LocalizationContext context)
        {
            locs = new MyDictionary<Language, MyDictionary<LocalizationContext, Dictionary<string, string>>>();

            this.lang = lang;
            this.context = context;
        }
        public Localization()
        {
            locs = new MyDictionary<Language, MyDictionary<LocalizationContext, Dictionary<string, string>>>();

        }
    }

    public struct LocalizationContext
    {
        string[] context;

        public string last { get { return context[context.Length - 1]; } }

        public string full { get { return string.Join(@"\", context); } }

        public LocalizationContext(params string[] context)
        {
            this.context = context;
        }

        public static implicit operator LocalizationContext(string[] context)
        {
            return new LocalizationContext(context);
        }
        public static implicit operator LocalizationContext(string context)
        {
            return new LocalizationContext(context);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is LocalizationContext))
                return false;

            return this == (LocalizationContext)obj;
        }

        public override int GetHashCode()
        {
            int r = context.Length;
            foreach (var c in context)
                r += c.GetHashCode();

            return r;
        }

        public static bool operator == (LocalizationContext left, LocalizationContext right)
        {
            if (left.context.Length != right.context.Length)
                return false;

            for (int i = 0; i < left.context.Length; i++)
                if (left.context[i] != right.context[i])
                    return false;

            return true;
        }
        public static bool operator !=(LocalizationContext left, LocalizationContext right)
        {
            if (left.context.Length == right.context.Length)
                return false;

            for (int i = 0; i < left.context.Length; i++)
                if (left.context[i] == right.context[i])
                    return false;

            return true;
        }
    }
}
