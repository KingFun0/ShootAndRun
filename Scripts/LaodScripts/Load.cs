using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Slider loadSlider;
    public float progressUpdateInterval = 0.1f;
    private static int sceneID;
    public float loadingTime = 5f;

    public static void SetSceneID(int id)
    {
        sceneID = id;
    }

    private void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    IEnumerator LoadingSequence()
    {
        yield return StartCoroutine(UpdateSlider(0.25f));
        yield return StartCoroutine(UpdateSlider(0.50f));
        yield return StartCoroutine(UpdateSlider(0.75f));
        yield return StartCoroutine(UpdateSlider(1f));

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(LoadNextScene());
    }

    IEnumerator UpdateSlider(float targetValue)
    {
        float timer = 0f;
        while (timer < progressUpdateInterval)
        {
            timer += Time.deltaTime;
            loadSlider.value = Mathf.Lerp(loadSlider.value, targetValue, timer / progressUpdateInterval);
            yield return null;
        }
        loadSlider.value = targetValue;
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneID);
        while (!oper.isDone)
        {
            yield return null;
        }
    }
}
