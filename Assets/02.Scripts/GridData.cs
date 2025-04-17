using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSetting", menuName = "Scriptable Objects/GridSetting")]
public class GridData : ScriptableObject
{
    public bool[] grid = new bool[49];

    
    [Header("질병 태그")]
    public SymptomTag Tag;
}


[CustomEditor(typeof(GridData))]
public class GridDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GridData gridData = (GridData)target;
        int gridSize = 7;

        GUILayoutOption width = GUILayout.Width(20);
        GUILayoutOption height = GUILayout.Height(20);

        EditorGUILayout.LabelField("비활성 그리드", EditorStyles.boldLabel);
        for (int y = 0; y < gridSize; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * 7 + x;
                gridData.grid[index] = GUILayout.Toggle(gridData.grid[index], "",width,height);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridData);
        }
    }
}