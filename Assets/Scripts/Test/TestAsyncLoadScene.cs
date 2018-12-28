
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestAsyncLoadScene : EventHandler
{
    public override void HandleEvent(params object[] data)
    {
        IEnumerator<WaitForSeconds> test = Test();
        StartCoroutine(test);
    }

    private IEnumerator<WaitForSeconds> Test()
    {
        //SceneArgs e = new SceneArgs(1);
        //Game.Instance.EventBus.SendEvents("exit_scene", e);

        AsyncOperation op = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        op.allowSceneActivation = false;
        Debug.Log(op.progress.ToString());
        //while(op.progress < 0.9f)
        //{
        //  progressBar.value = op.progress;
        yield return new WaitForSeconds(3f);
        //}

        op.allowSceneActivation = true;
    }
}
