using UnityEngine;

public class GameManager : BaseManager
{
    static GameManager instance;

    static PoolManager poolManager;
    static ResourceManager resourceManager;
    static DataManager dataManager;
    static SceneManager sceneManager;
    static AudioManager audioManager;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static DataManager Data { get { return dataManager; } }
    public static SceneManager Scene { get { return sceneManager; } }
    public static AudioManager Audio { get { return audioManager; } }

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        Initialize();
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public override void Initialize()
    {
        base.Initialize();

        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resourceManager = resourceObj.AddComponent<ResourceManager>();
        resourceManager.Initialize();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();
        poolManager.Initialize();

        GameObject dataObj = new GameObject();
        dataObj.name = "DataManager";
        dataObj.transform.parent = transform;
        dataManager = dataObj.AddComponent<DataManager>();
        dataManager.Initialize();

        GameObject sceneObj = new GameObject();
        sceneObj.name = "SceneManager";
        sceneObj.transform.parent = transform;
        sceneManager = sceneObj.AddComponent<SceneManager>();
        sceneManager.Initialize();

        GameObject audioObj = new GameObject();
        audioObj.name = "AudioManager";
        audioObj.transform.parent = transform;
        audioManager = audioObj.AddComponent<AudioManager>();
        audioManager.Initialize();
    }
}