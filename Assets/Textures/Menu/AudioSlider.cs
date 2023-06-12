using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider audioSlider;

    void Start()
    {
        audioSlider.value = PlayerPrefs.GetFloat("Volume", 1);
        AudioListener.volume = audioSlider.value;
    }

    public void SetVolume()
    {
        AudioListener.volume = audioSlider.value;
        PlayerPrefs.SetFloat("Volume", audioSlider.value);
    }
}
