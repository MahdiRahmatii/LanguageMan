using UnityEngine;
using UnityEngine.UI;

public class LanguageManText : MonoBehaviour
{
    private void Start()
    {
        Text txt = GetComponent<Text>();
        txt.text = LanguageMan.GetWord(txt.text);
    }
}