using UnityEngine;
using UnityEngine.UI;

public class LanguageManDropdownLanguages : MonoBehaviour
{
    private int m_language;

    private void Start()
    {
        Dropdown dp = GetComponent<Dropdown>();
        dp.options.Clear();

        foreach (string l in LanguageMan.Instance.Languages)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = l;
            dp.options.Add(od);
        }

        SetLanguageIndex(LanguageMan.GetLanguageIndex());
        dp.value = m_language;
        dp.RefreshShownValue();
    }

    public void SetLanguageIndex(int l)
    {
        m_language = l;
    }

    public void Apply()
    {
        LanguageMan.SetLanguageIndex(m_language);
    }
}