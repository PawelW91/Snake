using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnMouseScale : MonoBehaviour
{
public GameObject Panel;
public GameObject CloseButton;


public void PointerEnter()
{
    StartCoroutine(ScaleOverTime());
}

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
{
    yield return new WaitForSeconds(delay);
    SceneManager.LoadScene(sceneName);
}

    private IEnumerator ScaleOverTime()
{
    Vector3 startScale = transform.localScale;
    Vector3 targetScale = new Vector3(1.25f, 1.25f, 1);

    float currentTime = 0f;
    float scaleDuration = 0.1f;

    while (currentTime < scaleDuration)
    {
        currentTime += Time.deltaTime;
        transform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / scaleDuration);
        yield return null;
    }
}

    public void PointerExit()
{
    StopCoroutine(ScaleOverTime());
    StartCoroutine(ScaleOverTimeDown());
}

    private IEnumerator ScaleOverTimeDown()
{
    Vector3 startScale = transform.localScale;
    Vector3 targetScale = new Vector3(1f, 1f, 1);

    float currentTime = 0f;
    float scaleDuration = 0.1f;

    while (currentTime < scaleDuration)
    {
        currentTime += Time.deltaTime;
        transform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / scaleDuration);
        yield return null;
    }
}
    public void OnClickOPTIONS()
{   
    CloseButton.transform.localScale = Vector3.one;
    StartCoroutine(ScaleOverTimeOnClick());
    Panel.SetActive(!Panel.activeSelf);
}
    public void OnClickCloseOPTIONS()
{
    StartCoroutine(ScaleOverTimeOnClick());
    Panel.SetActive(false);
}

    public void OnClickSTART()
{   
    CloseButton.transform.localScale = Vector3.one;
    StartCoroutine(ScaleOverTimeOnClick());
    StartCoroutine(LoadSceneAfterDelay("Game_Map2(Scene_2)", 0.2f));
}

  public void OnClickEXITTOMENU()
{   
    CloseButton.transform.localScale = Vector3.one;
    StartCoroutine(ScaleOverTimeOnClick());
    SceneManager.LoadScene("Menu (Scene_0)");
}


    public void OnClickEXIT()
{
    StartCoroutine(ScaleOverTimeOnClick());
    Application.Quit();
}
    private AudioClip sound;
    public AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        source.PlayOneShot(sound);
    }

    private IEnumerator ScaleOverTimeOnClick()
{
    Vector3 startScale = transform.localScale;
    Vector3 targetScale = new Vector3(1.2f, 1.2f, 1);

    float currentTime = 0f;
    float scaleDuration = 0.1f;

    while (currentTime < scaleDuration)
    {
        currentTime += Time.deltaTime;
        transform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / scaleDuration);
        yield return null;
    }
}




















   /* 
   BEZ CZASUOW
   public void PointerEnter()
    {transform.localScale = new Vector2(1.25f, 1.25f);}

    public void PointExit()
    {transform.localScale = new Vector2(1f,1f);}

    public void OnClick()
    {transform.localScale = new Vector2(1.12f,1.12f);}
    */

}