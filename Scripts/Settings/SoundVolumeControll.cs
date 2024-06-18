using UnityEngine;
using UnityEngine.UI;
public class SoundVolumeControll : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private Slider slider;

    [Header("Tags")]
    [SerializeField] private string sliderTag;

    [Header("Keys")]
    [SerializeField] private string saveVolumeKey;

    [Header("Parameters")]
    [SerializeField] private float volume;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(saveVolumeKey))
        {
            this.volume = PlayerPrefs.GetFloat(saveVolumeKey);
            this.audio.volume = this.volume;

            GameObject sliderObj = GameObject.FindWithTag(sliderTag);
            if (sliderObj != null)
            {
                this.slider = sliderObj.GetComponent<Slider>();
                this.slider.value = this.volume;
            }
        }
        else
        {
            this.volume = 0.5f;
            PlayerPrefs.SetFloat(saveVolumeKey, this.volume);
            this.audio.volume = this.volume;
        }
    }
    private void LateUpdate()
    {
        GameObject sliderObj = GameObject.FindWithTag(sliderTag);
        if(sliderObj != null)
        {
            this.slider = sliderObj.GetComponent<Slider>();
            this.volume = slider.value;

            if(this.audio.volume != this.volume)
            {
                PlayerPrefs.SetFloat(saveVolumeKey, this.volume);
            }
        }
        this.audio.volume = this.volume;
    }
}
