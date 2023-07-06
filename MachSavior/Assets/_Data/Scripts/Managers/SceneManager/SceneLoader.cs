using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using static UnityEngine.ParticleSystem;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private AsyncOperation sceneLoading;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panelLoadScreenDone;
    [SerializeField] private Slider sliderLoad;
    [SerializeField] private TextMeshProUGUI loadText;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float lerpSpeed = 1f;

    [SerializeField] bool loading = false;
    [SerializeField] private bool isLoadAnimationDone = false;

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
        panelLoadScreenDone.SetActive(false);
    }

    void Update()
    {
        ChangeScene();
    }

    void ChangeScene()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Load(0);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            Load(1);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            Load(2);
        }
    }

    public void Load(int sceneToLoad)
    {
        sceneLoading = SceneManager.LoadSceneAsync(sceneToLoad);
        loading = true;
        panel.SetActive(true);
        StartCoroutine(Loading());
        StartCoroutine(LoadingBar());
    }
    
    private IEnumerator Loading()
    {
        PlayerInputManager.Instance.IsInputAllowed = false;
        PlayerInputManager.Instance.IsPauseAllowed = false;
        while (loading)
        {
            float loadingNumber = sceneLoading.progress * 100;
            loadText.text = loadingNumber + "%";

            if (sceneLoading.progress == 1)
            {
                loading = false;
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
    
    private IEnumerator LoadingBar()
    {
        lerpSpeed = 0;
        isLoadAnimationDone = false;
        while (lerpSpeed < 1)
        {
            sliderLoad.value = curve.Evaluate(lerpSpeed);
            lerpSpeed += Time.deltaTime * Random.Range(0.1f, 0.3f);
            yield return null;
        }

        isLoadAnimationDone = true;

        if (!loading)
        {
            panelLoadScreenDone.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            panelLoadScreenDone.SetActive(true);
            panel.SetActive(false);
            panelLoadScreenDone.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0f), 1f);
            yield return new WaitForSecondsRealtime(1.3f);
            panelLoadScreenDone.SetActive(false);
            PlayerInputManager.Instance.IsInputAllowed = true;
            PlayerInputManager.Instance.IsPauseAllowed = true;
        }          
    }
}
