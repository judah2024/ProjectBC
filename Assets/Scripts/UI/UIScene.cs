using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene : UIBase
{
    public override void OnOpen()
    {
        IsTransitioning = true;
        gameObject.SetActive(true);
        
        base.OnOpen();
        
        IsTransitioning = false;
    }

    public override void OnClose()
    {
        IsTransitioning = true;

        base.OnClose();
        
        gameObject.SetActive(false);
        
        IsTransitioning = false;
    }
}
