using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonVisible : MonoBehaviour
{
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;
    public Button button;

    private void Start()
    {
        button.interactable = false;
        inputField1.onValueChanged.AddListener(OnInputFieldValueChanged);
        inputField2.onValueChanged.AddListener(OnInputFieldValueChanged);
        inputField3.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private void OnInputFieldValueChanged(string text)
    {
        if (!string.IsNullOrEmpty(inputField1.text) && !string.IsNullOrEmpty(inputField2.text) && !string.IsNullOrEmpty(inputField3.text))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
