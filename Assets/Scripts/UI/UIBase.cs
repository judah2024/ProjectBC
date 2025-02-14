using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public bool IsTransitioning { get; protected set; }
    
    public virtual void OnOpen()
    {
    }

    public virtual void OnClose()
    {
    }
}
