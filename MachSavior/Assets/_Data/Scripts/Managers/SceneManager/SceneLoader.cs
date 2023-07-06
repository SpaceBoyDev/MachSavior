using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private AsyncOperation sceneLoading;

    public GameObject panel;
    public Slider sliderLoad;
    public TextMeshProUGUI loadText;

    public bool loading = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        panel.SetActive(false);
    }


    public void Load(int sceneToLoad)
    {
        sceneLoading = SceneManager.LoadSceneAsync(sceneToLoad);
        loading = true;
        panel.SetActive(true);
        panel.GetComponent<Image>().DOColor(new Color(0.3490566f, 0.3490566f, 0.3490566f, 1f), 0.01f);
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        while (loading)
        {
            sliderLoad.value = sceneLoading.progress;
            float loadingNumber = sceneLoading.progress * 100;
            loadText.text = loadingNumber + "%";
            //yield return new WaitForSeconds(1);

            if (sceneLoading.progress == 1)
            {
                loading = false;
                panel.GetComponent<Image>().DOColor(new Color(0.9103928f, 1f, 0f, 0f), 0.5f);

                yield return new WaitForSeconds(0.5f);
                panel.SetActive(false);
            }

            yield return null;
        }
    }
}
