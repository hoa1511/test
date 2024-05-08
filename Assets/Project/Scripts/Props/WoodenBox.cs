using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicMeshCutter;

public class WoodenBox : CutAbleObject
{
    [SerializeField] private AudioClip boxCrackSFX; 
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }
    public override void CutObject(CutterBehaviour cutterBehaviour)
    {
        PlaySound(boxCrackSFX);
        if(gameObject.GetComponent<MeshTarget>())
        {
            cutterBehaviour.Cut(gameObject.GetComponent<MeshTarget>(), cutterBehaviour.gameObject.transform.position, cutterBehaviour.gameObject.transform.forward); 
        }
    }

}
