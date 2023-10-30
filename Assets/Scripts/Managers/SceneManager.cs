using System.Collections;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : BaseManager
{
    BaseScene curScene;
    LoadingUI loadingUI;

    public bool ReadyToPlay { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        loadingUI = GameManager.Pool.GetUI<LoadingUI>("UI/LoadingUI");
        GameManager.Pool.ReleaseUI(loadingUI);
    }

    public BaseScene CurScene
    {
        get
        {
            if (!curScene)
                curScene = GameObject.FindObjectOfType<BaseScene>();

            return curScene;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        ReadyToPlay = false;
        GameManager.Pool.GetUI(loadingUI);
        yield return new WaitForSeconds(1f);
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            yield return null;
        }

        if (CurScene)
        {
            CurScene.LoadAsync();
            while (CurScene.Progress < 1f)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        GameManager.Pool.ReleaseUI(loadingUI);
        ReadyToPlay = true;
    }
}