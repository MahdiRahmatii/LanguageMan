using UnityEngine;
using UnityEngine.UI;

public class LanguageManText : MonoBehaviour
{
    Text m_text;

    private void Start()
    {
        m_text = GetComponent<Text>();
        OnEnable();
    }

    private void OnEnable()
    {
        if (m_text)
            m_text.text = LanguageMan.Translate(m_text.text);
    }
}