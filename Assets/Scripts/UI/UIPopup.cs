using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIBase
{
    public override void OnOpen()
    {
        gameObject.SetActive(true);
        
        base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
        
        gameObject.SetActive(false);
    }
}
