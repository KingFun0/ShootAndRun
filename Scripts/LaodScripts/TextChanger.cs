using System.Collections;
using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; 
    public string[] messages; 
    private int currentMessageIndex = 0; 

    void Start()
    {
        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeText()
    {
        while (true)
        {
            textMeshPro.text = messages[currentMessageIndex];
            currentMessageIndex = (currentMessageIndex + 1) % messages.Length; 
            yield return new WaitForSeconds(2); 
        }
    }
}
