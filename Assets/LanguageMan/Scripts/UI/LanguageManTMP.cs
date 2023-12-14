using TMPro;
using UnityEngine;

public class LanguageManTMP : MonoBehaviour
{
    private void Start()
    {
        TMP_Text txt = GetComponent<TMP_Text>();
        txt.text = LanguageMan.Translate(txt.text);
    }
}