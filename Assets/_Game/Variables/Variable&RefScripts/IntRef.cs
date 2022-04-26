using System;
using UnityEditor;

[Serializable]
public class IntRef : BaseRef<int, IntVar>
{

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(IntRef))]
public class IntRefDrawer : BaseRefDrawer<int>
{

}
#endif
