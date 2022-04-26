using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class CurveRef : BaseRef<AnimationCurve, CurveVar>
{

}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CurveRef))]
public class CurveRefDrawer : BaseRefDrawer<AnimationCurve>
{

}
#endif