using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedText : MonoBehaviour
{
    [SerializeField] private float m_typingSpeed = 0.05f;
    private Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        StartCoroutine(TypeText(textComponent.text));
    }

    private IEnumerator TypeText(string textToType)
    {
        for (int i = 0; i <= textToType.Length; i++)
        {
            textComponent.text = textToType.Substring(0, i);
            yield return new WaitForSeconds(m_typingSpeed);
        }
    }
}
