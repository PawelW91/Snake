using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopUp : MonoBehaviour
{
    public Image overlay;
    public float Czas=1.5f;
    public Slider audioSlider;

    void Start()
{
    overlay.gameObject.SetActive(true);
    overlay.canvasRenderer.SetAlpha(1.0f);
    overlay.CrossFadeAlpha(0.0f, Czas, false);
    Invoke("DeactivateOverlay", Czas);

    audioSlider.value = PlayerPrefs.GetFloat("Volume", 1);
    AudioListener.volume = audioSlider.value;
}

void DeactivateOverlay()
{
    overlay.gameObject.SetActive(false);
}
}
