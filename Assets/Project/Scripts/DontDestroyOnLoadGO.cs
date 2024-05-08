using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadGO : MonoBehaviour
{
private static DontDestroyOnLoadGO instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

}
