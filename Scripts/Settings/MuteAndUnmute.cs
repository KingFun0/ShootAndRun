using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MuteAndUnmute : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text buttonText;
    public string soundOnText = "Выключить";
    public string soundOffText = "Включить";

    private bool isMuted = false;
    private const string MutePreferenceKey = "IsMuted";

    void Start()
    {
        GameObject audioSourceObject = GameObject.FindGameObjectWithTag("BG_MUSIC_CREATED");
        if (audioSourceObject != null)
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("Object with tag 'BG_MUSIC_CREATED' not found.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on object with tag 'BG_MUSIC_CREATED'.");
        }
        else
        {
            if (audioSource == null || buttonText == null)
            {
                Debug.LogError("AudioSource or Text component not assigned!");
            }
            isMuted = PlayerPrefs.GetInt(MutePreferenceKey, 0) == 1;
            audioSource.mute = isMuted;
            buttonText.text = isMuted ? soundOffText : soundOnText;
        }
    }

    public void ToggleAudioAndText()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        buttonText.text = isMuted ? soundOffText : soundOnText;
        PlayerPrefs.SetInt(MutePreferenceKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
