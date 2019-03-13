using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private Image loadingBar;

    private void Awake()
    {
        loadingBar = GetComponent<Image>();

        if(loadingBar != null)
        {
            loadingBar.type = Image.Type.Filled;
            loadingBar.fillMethod = Image.FillMethod.Horizontal;
            loadingBar.fillAmount = 0;
        }
    }

    public void LoadLevel(int sceneIndex) => StartCoroutine(LoadingScene(sceneIndex));

    private IEnumerator LoadingScene(int sceneIndex)
    {
        AsyncOperation levelLoadingOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!levelLoadingOperation.isDone)
        {
            loadingBar.fillAmount = levelLoadingOperation.progress;
            yield return null;
        }

    }
}
