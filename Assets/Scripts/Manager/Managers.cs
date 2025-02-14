using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private const string MANAGER_PATH = "Manager";
    
    private static Managers _instance;
    public static Managers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Managers>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("Managers");
                    _instance = go.AddComponent<Managers>();
                }
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private static UIManager _ui;
    private static SceneLoader _scene;
    
    public static UIManager UI => GetManager(ref _ui);
    public static SceneLoader Scene => GetManager(ref _scene);

    private void Init()
    {
        UI?.Init();
        Scene?.Init();
    }

    private static T GetManager<T>(ref T manager) where T : Component
    {
        if (_instance == null)
        {
            Instance.Init();
        }
        
        if (manager != null)
            return manager;
        
        // 1. Scene에서 찾아오기
        manager = FindObjectOfType<T>();
        if (manager != null)
        {
            manager.transform.SetParent(_instance.transform);
            return manager;
        }

        // 2. Resources에서 로드
        string path = $"{MANAGER_PATH}/{typeof(T)}";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            GameObject go = Instantiate(prefab, _instance.transform);
            go.name = typeof(T).Name;
            manager = go.GetComponent<T>();
            if (manager == null)
            {
                Destroy(go);
            }
        }

        // 3. 실패시 로그 출력
        if (manager == null)
        {
            LoggerEx.LogError("Resources에 해당 매니저가 없어요.");
        }

        return manager;
    }
}
