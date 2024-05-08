using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicMeshCutter;

public interface ICutable
{
    public void CutObject(CutterBehaviour cutterBehaviour);
}