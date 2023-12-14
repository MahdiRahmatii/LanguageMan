using UnityEngine;

public class LanguageManInisializer : MonoBehaviour
{
    [SerializeField] private string m_languageAtStart;

    private void Awake()
    {
        LanguageMan.Initialize(defaultLanguageLabel: m_languageAtStart);
    }
}