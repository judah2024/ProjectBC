using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private void Start()
    {
        Managers.UI.OpenUI<UIMainLobby>("MainLobby");
    }
}
