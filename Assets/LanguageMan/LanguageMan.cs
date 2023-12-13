///Developed By BlueAge Studio

using System;
using System.Collections.Generic;
using UnityEngine;
using ToPer = Persian.ToPersian;

[CreateAssetMenu(fileName = "LanguageMan List", menuName = "BlueAge/Create LanguageMan List (You can have just one)")]
public class LanguageMan : ScriptableObject
{
    public List<string> Languages;
    public GropuProperty[] WordsGroup;

    [Header("Import from file")]
    public TextAsset TextFile;

    private static LanguageMan m_instance;
    private static int m_globalIndex = 0;
    private static string m_globalLanguage;
    private static List<string> m_languages;
    private static Dictionary<string, string[]> m_dictionary;

    [Serializable]
    public struct GropuProperty
    {
        public string Name;
        public WordProperty[] WordsProperty;
    }

    [Serializable]
    public struct WordProperty
    {
        public string Key;
        public string[] Words;
    }

    public static void Initialize()
    {
        m_languages = Instance.Languages;
        m_dictionary = new Dictionary<string, string[]>();
        List<WordProperty> ws = new List<WordProperty>();

        foreach (GropuProperty gp in Instance.WordsGroup)
        {
            foreach (WordProperty wp in gp.WordsProperty)
                ws.Add(wp);
        }

        foreach (WordProperty w in ws)
            m_dictionary.Add(w.Key, w.Words);
    }

    public static void SetGlobalIndex(int index)
    {
        m_globalIndex = index;
    }

    public static void SetGlobalLanguage(string language)
    {
        m_globalLanguage = language;
    }

    public static string GetWord(string Key, int? Index = null, string Language = null)
    {
        int index = Index.HasValue ? Index.Value : m_globalIndex;
        string language = Language != null ? Language : m_globalLanguage;

        try
        {
            if (!String.IsNullOrEmpty(language))
            {
                int i = m_languages.IndexOf(language);
                return ToPer.Fix(m_dictionary[Key][i]);
            }
            else
                return ToPer.Fix(m_dictionary[Key][index]);
        }
        catch (Exception ex)
        {
            Debug.LogError($"-> [<b>LanguageMan</b>] : (<b>{Key}</b>) Have not value!\n" + ex);
            return Key;
        }
    }

    public static LanguageMan Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = Resources.Load<LanguageMan>("LanguageMan List");
            return m_instance;
        }
    }

    [ContextMenu("[ IMPORT FROM FILE ]")]
    public void ImportFromFile()
    {
        if (TextFile == null)
        {
            Debug.Log("-> [LanguageMan] : Text File is empty");
            return;
        }
    }
}