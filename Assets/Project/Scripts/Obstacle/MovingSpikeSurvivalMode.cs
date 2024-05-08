using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikeSurvivalMode : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5;
    private void Start()
    {
        
    }
    void Update()
    {
        if(PlayerPrefs.GetInt("hasPause") == 0)
        {
            transform.Translate(new Vector3(0, 1, 0) * movingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(0, -5, 0);
        }
    }
}
