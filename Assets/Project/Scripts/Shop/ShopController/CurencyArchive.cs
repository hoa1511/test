using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurencyArchive : MonoBehaviour
{
    [Header("Gem")]
        [SerializeField] private Gem gem;
    
    public Gem GetGem{get => gem;}
    public static CurencyArchive Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(this);
        }
    }

}
