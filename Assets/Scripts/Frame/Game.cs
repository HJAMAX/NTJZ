using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ObjPool))]
public class Game : Singleton<Game>
{
    [SerializeField]
    private bool loadSceneStart;

    public ObjPool ObjectPool = null;

    public EventBus EventBus = null;

    public ServerEngine serverEngine;

    /// <summary>
    /// 特殊情况返回起始场景
    /// </summary>
    /// <param name="code"></param>
    public void ReturnStartScene(int code = 0)
    {
        GameData.returnStartCode = code;
        LoadingScene(GameData.startIndex);
    }

    public void LoadingScene(int level)
    {
        GameData.nextSceneIndex = level;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventBus.Clear();
        
        EventController[] eventCtrls = FindObjectsOfType<EventController>();
        foreach (EventController eventCtrl in eventCtrls)
        {
            EventBus.RegisterController(eventCtrl);
        }
        //发布事件
        EventBus.SendEvents("init");
    }

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnEnable()
    {
        ObjectPool = GetComponent<ObjPool>();
        EventBus = GetComponent<EventBus>();
        serverEngine = GetComponent<ServerEngine>();
   
        EventController eventController = GetComponent<EventController>();
        eventController.RegisterHandler(GetComponent<ExitSceneHandler>());
        eventController.RegisterHandler(GetComponent<HttpEventHandler>());
        EventBus.RegisterController(eventController);
    
        DontDestroyOnLoad(this);

        if (loadSceneStart)
        {
            LoadingScene(GameData.startIndex);
        }
    }
}
