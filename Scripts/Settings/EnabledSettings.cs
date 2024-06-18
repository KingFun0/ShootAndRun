using UnityEngine;
using UnityEngine.UI;

public class EnabledSettings : MonoBehaviour
{
    [SerializeField] private GameObject _enabledSettings;
    public void OnEnabledSettings()
    {
        Debug.LogWarning("Work");
        _enabledSettings.SetActive(true);
    }

    public void OnDisabledSettings()
    {
        Debug.LogWarning("Work");
        _enabledSettings.SetActive(false);
    }
}
