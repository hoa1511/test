using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        Manager.Add("UIScene");
        Time.timeScale = 0;
    }
}
