///Developed By Mahdi Rahmati
///https://github.com/MahdiRahmatii/LanguageMan

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using ToArabic = LanToArabic.LanToArabic;

[CreateAssetMenu(fileName = "LanguageMan List", menuName = "LanguageMan/Create LanguageMan List (You can have just one)")]
public class LanguageMan : ScriptableObject
{
    #region Properties
    public List<string> Languages;
    public GroupProperty[] WordGroups;

    [Serializable]
    public struct GroupProperty
    {
        public string GroupName;
        public WordProperties[] GroupWords;
    }

    [Serializable]
    public struct WordProperties
    {
        public string Key;
        public string[] Words;
    }

    private static LanguageMan m_instance;
    private static int m_globalIndex = 0;
    private static List<string> m_languages;
    private static Dictionary<string, string[]> m_dictionary;
    #endregion

    #region Singelton
    public static LanguageMan Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = Resources.Load<LanguageMan>("LanguageMan List");
            return m_instance;
        }
    }
    #endregion

    #region Initialization
    /// <summary>
    /// Initializes the translation system with optional parameters.
    /// </summary>
    /// <param name="defaultLanguageIndex">Optional. The index of the default language for translation. Defaults to null.</param>
    /// <param name="defaultLanguageLabel">Optional. The label of the default language for translation. Defaults to null.</param>
    /// <remarks>
    /// <para>If both <paramref name="defaultLanguageIndex"/> and <paramref name="defaultLanguageLabel"/> are provided,
    /// the method will prioritize <paramref name="defaultLanguageIndex"/>.</para>
    /// <para>If the user changes the language in the game, the selected language will be saved.
    /// The next time the game is opened, the user will see their last selected language instead of the default language.</para>
    /// </remarks>
    public static void Initialize(int? defaultLanguageIndex = null, string defaultLanguageLabel = null)
    {
        m_languages = Instance.Languages;
        m_dictionary = new Dictionary<string, string[]>();
        List<WordProperties> ws = new List<WordProperties>();

        foreach (GroupProperty gp in Instance.WordGroups)
            foreach (WordProperties wp in gp.GroupWords)
                ws.Add(wp);

        foreach (WordProperties w in ws)
            m_dictionary.Add(w.Key, w.Words);

        if (PlayerPrefs.HasKey("global.LanguageMan"))
            defaultLanguageIndex = PlayerPrefs.GetInt("global.LanguageMan");

        if (defaultLanguageIndex != null && defaultLanguageLabel != null)
            Debug.LogWarning("[<b>LanguageMan</b>] : Both defaultLanguageIndex and defaultLanguageLabel provided. Prioritizing defaultLanguageIndex.");

        if (defaultLanguageIndex.HasValue)
            SetLanguageIndex(defaultLanguageIndex.Value);
        else if (!String.IsNullOrEmpty(defaultLanguageLabel))
            SetLanguageLabel(defaultLanguageLabel);
    }
    #endregion

    #region SetLanguage
    public static void SetLanguageLabel(string languageLabel) => SetLanguageIndex(m_languages.IndexOf(languageLabel));
    public static int GetLanguageIndex() => PlayerPrefs.GetInt("global.LanguageMan");
    public static void SetLanguageIndex(int languageIndex)
    {
        m_globalIndex = languageIndex;
        PlayerPrefs.SetInt("global.LanguageMan", languageIndex);
    }
    #endregion

    #region Translation
    /// <summary>
    /// Translates a given key to the specified language or uses the default language.
    /// </summary>
    /// <param name="key">The key to be translated. (will be searched in the list of keys)</param>
    /// <param name="specialLanguageIndex">Optional. The index of a special language to use for translation. Defaults to null.</param>
    /// <param name="specialLanguageLabel">Optional. The label of a special language to use for translation. Defaults to null.</param>
    /// <returns>The translated string.</returns>
    public static string Translate(string Key, int? specialLanguageIndex = null, string specialLanguageLabel = null)
    {
        int index = specialLanguageIndex.HasValue ? specialLanguageIndex.Value : m_globalIndex;

        try
        {
            if (!String.IsNullOrEmpty(specialLanguageLabel))
            {
                int i = m_languages.IndexOf(specialLanguageLabel);
                return ToArabic.Fix(m_dictionary[Key][i]);
            }
            else
                return ToArabic.Fix(m_dictionary[Key][index]);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[<b>LanguageMan</b>] : (<b>{Key}</b>) Have not value!\n" + ex);
            return Key;
        }
    }
    #endregion

    #region Import
    public static void Import(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LanguageMan));

        using (TextReader reader = new StreamReader(filePath))
        {
           // LanguageMan.Instance = (LanguageMan)serializer.Deserialize(reader);
        }
    }
    #endregion

    #region Export
    public void Export(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LanguageMan));

        using (TextWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, this);
        }
    }
    #endregion
}