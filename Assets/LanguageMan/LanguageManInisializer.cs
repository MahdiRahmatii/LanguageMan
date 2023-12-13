using UnityEngine;

public class LanguageManInisializer : MonoBehaviour
{
    [SerializeField] private string m_globalLanguage;
    private void Awake()
    {
        LanguageMan.Initialize();
        LanguageMan.SetGlobalLanguage(m_globalLanguage);
    }
}