using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 로딩과 팝업 문제 : 로딩 오류시 나오는 팝업UI는 만든 구조를 유지한다. (Resources.Load 하지 말것)
// 해당 팝업은 1개만 존개하며, 확인시 Title로 즉시 이동하므로 상관없음

public class UIManager : MonoBehaviour
{
    private const string UI_PATH = "UI";
    
    private void Awake()
    {
        // 싱글턴인 Managers을 사용하여 단 하나만 가지도록 유지
        if (Managers.UI != this)
        {
            Destroy(gameObject);
        }
        
    }

    public Transform canvasTransform;
    public Image loadingImage;

    private Stack<UIBase> _uiStack = new Stack<UIBase>();
    private Stack<UIScene> _SceneStack = new Stack<UIScene>();
    private Dictionary<string, UIBase> _uiDict = new Dictionary<string, UIBase>();

    private bool _isUITransitioning = false;
    private Coroutine _coUITransition;

    public void Init()
    {
        canvasTransform.gameObject.SetActive(true);
        // 가장 처음 실행시 모든UI를 비활성화한 후, 순차적으로 활성화
        foreach (Transform child in canvasTransform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleBackInput();
    }

    private void HandleBackInput()
    {
        // TODO : 경고 UI 오픈?
        if (_isUITransitioning == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    private T GetUI<T>(string uiName) where T : UIBase
    {
        // 1. dict에 있으면 가져온다.
        if (_uiDict.TryGetValue(uiName, out UIBase ui))
        {
            return ui as T;
        }

        // 2. 씬 내에 있는가?
        var comp = canvasTransform.GetComponentInChildren<T>(true);
        if (comp == null)
        {
            // 3. 없으면 만들어서 캐싱한다.
            GameObject prefab = Resources.Load<GameObject>($"{UI_PATH}/{uiName}");
            if (prefab == null)
            {
                Debug.LogError($"UI Prefab not found: {uiName}");
                return null;
            }

            GameObject go = Instantiate(prefab, canvasTransform);
            go.name = uiName;
            int length = canvasTransform.childCount;
            go.transform.SetSiblingIndex(length - 1);
            
            comp = go.GetComponent<T>();
            if (comp == null)
            {
                Debug.LogError($"Component {typeof(T)} not found on prefab: {uiName}");
                Destroy(go);
                return null;
            }
        }
        
        _uiDict.Add(uiName, comp);
        return comp;
    }

    public void OpenUI<T>(string uiName) where T : UIBase
    {
        T newUI = GetUI<T>(uiName);
        if (newUI == null)
        {
            Debug.LogError($"UI not found: {uiName}");
            return;
        }

        StartUITransition(CoOpenUI(newUI));
    }

    public void CloseUI()
    {
        // UIScene는 반드시 1개 활성화 되어야 한다.
        // 이 상태에서 UI닫기를 시도하면 종료 팝업을 열어준다.
        if (_SceneStack.Count <= 1)
        {
            Debug.Log("종료?!");
            return;
        }

        StartUITransition(CoCloseUI());
    }

    void StartUITransition(IEnumerator coroutine)
    {
        // 코루틴 실행이 종료될 때, null로 초기화된다.
        if (_coUITransition != null)
        {
            return;
        }

        _coUITransition = StartCoroutine(coroutine);
    }

    IEnumerator CoOpenUI(UIBase ui)
    {
        _isUITransitioning = true;

        if (ui is UIScene scene)
        {
            if (_SceneStack.TryPeek(out UIScene curr))
            {
                curr.OnClose();
                yield return new WaitUntil(() => curr.IsTransitioning == false);
            }
            _SceneStack.Push(scene);
        }
        
        if (_SceneStack.Count > 0)
        {
            var current = _SceneStack.Peek();
            current.OnClose();
        }
        
        _uiStack.Push(ui);
        ui.OnOpen();
        yield return new WaitUntil(() => ui.IsTransitioning == false);
        
        _isUITransitioning = false;
        _coUITransition = null;
    }

    IEnumerator CoCloseUI()
    {
        _isUITransitioning = true;

        var ui = _uiStack.Pop();
        ui.OnClose();
        yield return new WaitUntil(() => ui.IsTransitioning == false);

        // 닫을 UI가 UIScene라면 이전 씬을 열어준다.
        if (ui is UIScene)
        {
            _SceneStack.Pop();
            var prev = _SceneStack.Peek();
            prev.OnOpen();
            yield return new WaitUntil(() => prev.IsTransitioning == false);
        }
        
        _isUITransitioning = false;
        _coUITransition = null;
    }

    public void Clear()
    {
        while (_uiStack.Count > 0)
        {
            var popup = _uiStack.Pop();
            popup.OnClose();
        }
        
        _uiStack.Clear();
        _SceneStack.Clear();
    }
}
