using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicMeshCutter;

public class Arrow : WeaponBase, ICutable
{
    [SerializeField] private float arrowSpeed = 10;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        MoveForward();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            Destroy(this.gameObject);
        }

        if(other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);
        }
    }

    private void MoveForward()
    {
        transform.Translate(new Vector3(0, 0 ,1) * Time.deltaTime * arrowSpeed);
    }

    public void CutObject(CutterBehaviour cutterBehaviour)
    {
        if(gameObject.GetComponent<MeshTarget>())
        {
            cutterBehaviour.Cut(gameObject.GetComponent<MeshTarget>(), cutterBehaviour.gameObject.transform.position, cutterBehaviour.gameObject.transform.forward); 
        }
    }
}
