using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GUILabel : MonoBehaviour
{
    [SerializeField] private List<LabelBlock> labelBlocks;
    
    private void OnGUI()
    {
        foreach (var labelBlock in labelBlocks)
        {
            labelBlock.Draw();
        }
    }
}

[Serializable]
public class TextBlock
{
    public string Text;
    public VarBase<float> Float;
    public VarBase<int> Int;
    public VarBase<Vector3> Vector3;

    public void Draw(Rect position)
    {
        string varText = "";
        if (Float != null) { varText = Float.Value.ToString(); }
        if (Int != null) { varText = Int.Value.ToString(); }
        if (Vector3 != null) { varText = Vector3.Value.ToString(); }
        GUI.Label(position, $"{Text}: {varText}");
    }
}

[Serializable]
public class LabelBlock
{
    public Rect Position;
    public string Header;
    public float Offset;
    public List<TextBlock> TextBlocks;
    public void Draw()
    {
        GUI.Label(Position, $"<b>{Header}</b>");

        for (int i = 0; i < TextBlocks.Count; i++)
        {
            Rect pos = new Rect(new Vector2(Position.x, Position.y + Offset * (i+1)),
                new Vector2(Position.width, Position.height));
            TextBlocks[i].Draw(pos);
        }
    }
}

#if UNITY_EDITOR
// IngredientDrawer
[CustomPropertyDrawer(typeof(TextBlock))]
public class IngredientDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate rects
        var textRect = new Rect(position.x, position.y + 10, 100, position.height);
        
        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.PropertyField(textRect, property.FindPropertyRelative("Text"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
#endif