using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainLobby : UIScene
{

    public void OnOtherMenuButtonClick()
    {
        // 1. 팝업 열기
        LoggerEx.Log("메뉴 모음!");
        //Managers.UI.OpenUI<UIMenuPopup>("MenuPopup");
    }
    
    public void OnToTitleButtonClick()
    {
        LoggerEx.Log("타이틀로!");
        Managers.Scene.LoadScene("Title");
    }
}
