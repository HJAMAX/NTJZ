
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneHandler : MonoBehaviour
{
    /// <summary>
    /// 进度条
    /// </summary>
    [SerializeField]
    private UISlider progressBar;

    /// <summary>
    /// 读取时最少的等待时间
    /// </summary>
    [SerializeField]
    private float progressRate;

    void Start()
    {
        IEnumerator<WaitForSeconds> loading = Loading();
        StartCoroutine(loading);
    }

    private IEnumerator<WaitForSeconds> Loading()
    {
        SceneArgs e = new SceneArgs(SceneManager.GetActiveScene().buildIndex);
        Game.Instance.EventBus.SendEvents("exit_scene", e);

        AsyncOperation op = SceneManager.LoadSceneAsync(GameData.nextSceneIndex, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        float displayProgress = 0.0f;
        while(op.progress < 0.9f)
        {
            if (displayProgress < 0.9f)
            {
                displayProgress += progressRate;
                progressBar.value = displayProgress;
            }
            yield return null;
        }

        while (displayProgress < 1)
        {
            displayProgress += progressRate;
            progressBar.value = displayProgress;
            yield return null;
        }

        op.allowSceneActivation = true;
    }
}
