using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIRotate : MonoBehaviour
{
    private Transform parent;
    private Quaternion rotation;

    protected virtual void Start()
    {
    }

    protected virtual void Awake()
    {
        rotation = transform.rotation;
        parent = transform.parent;
    }
    protected virtual void LateUpdate()
    {
        RotateUI();
    }

    protected virtual void Update()
    {
        transform.position = new Vector3(parent.position.x, parent.position.y, parent.position.z);
    }

    protected virtual void RotateUI()
    {
        transform.rotation = rotation;
    }
}
