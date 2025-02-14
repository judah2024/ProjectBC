using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    private void Start()
    {
        Managers.UI.OpenUI<UITitle>("TitleScene");
    }
}
