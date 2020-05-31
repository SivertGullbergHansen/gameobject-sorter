using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EditModeFunctions : EditorWindow
{
    public static Transform Source;

    private static List<GameObject> _unsortedObjects = new List<GameObject>();
    public static float Rows = 8;
    public static float Distance = 5;
    public static float ColumnDistance = 5;
    private static float _column = 0;
    private static List<GameObject> _sortedObjects = new List<GameObject>();
    private static int _index = 0;

    public bool ShowTutorial = false;

    [MenuItem("Tools/Sivert/GameObject Sorter")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<EditModeFunctions>("GameObject Sorter");
        window.maxSize = new Vector2(600, 600);
        window.Show();
    }

    private void OnGUI()
    {
        //ShowWindow();
        ScalingWindow(0);
    }

    public static void SortObjectsInEditor()
    {
        _unsortedObjects.Clear();
        _sortedObjects.Clear();
        _index = 0;
        _column = 0;

        Debug.Log("started finding children...");
        foreach (Transform child in Source)
        {
            if (child.parent == Source)
            {
                _unsortedObjects.Add(child.gameObject);
            }
        }
        Debug.Log("finished finding children.");

        Rows -= 1;
        Debug.Log("started sorting children...");
        _sortedObjects = _unsortedObjects.OrderBy(go => go.name).ToList();
        Debug.Log("finished sorting children.");

        Debug.Log("started sorting...");
        for (int i = 0; i < _sortedObjects.Count; i++)
        {
            if (_index < Rows && _index >= 0)
            {
                _sortedObjects[i].transform.position = new Vector3(_column, 0, Distance * (_index + 1) - Distance);
                _index += 1;
            }
            else if (_index == Rows)
            {
                _sortedObjects[i].transform.position = new Vector3(_column, 0, Distance * (_index + 1) - Distance);
                _index = 0;
                _column += ColumnDistance;
            }
        }

        Rows += 1;

        Debug.Log("Finished sorting " + _sortedObjects.Count + " objects!");
    }

    private void ScalingWindow(int windowID)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.wordWrap = true;

        ShowTutorial = EditorGUI.Foldout(new Rect(3, 3, position.width - 6, 15), ShowTutorial, "View Tutorial");
        if (ShowTutorial)
        {
            GUILayout.Space(24);
            GUILayout.Label("<size=14>1. Place all the game-objects you want to sort inside a new game-object (or simply select all your objects, right click and press <i>Group</i>). Position does not matter.</size>", style);
            GUILayout.Space(2);
            GUILayout.Label("<size=14>2. Select the new game-object you created and assign it to the box below.</size>", style);
            GUILayout.Space(2);
            GUILayout.Label("<size=14>3. Specify the number of rows and distance in units below. All of these values must be bigger than 0. Feel free to use decimals if you have very small game-objects :)</size>", style);
            GUILayout.Space(2);
            GUILayout.Label("<size=14>4. Press <i>Sort Objects</i> and view the results.</size>", style);
        }

        GUILayout.Space(24);
        GUILayout.Label("<color=yellow><size=14>Select the parent\n of the game-objects you want to sort.</size></color>", style);
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        Source = EditorGUILayout.ObjectField(Source, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(2);
        GUILayout.Label("<color=yellow><size=14>Insert amount of rows <i>(default: 8)</i></size></color>", style);
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        Rows = EditorGUILayout.FloatField(Rows);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(2);
        GUILayout.Label("<color=yellow><size=14>Insert distance in units between rows<i>(default: 5)</i></size></color>", style);
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        Distance = EditorGUILayout.FloatField(Distance);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(2);
        GUILayout.Label("<color=yellow><size=14>Insert distance in units between columns<i>(default: 5)</i></size></color>", style);
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        ColumnDistance = EditorGUILayout.FloatField(ColumnDistance);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);

        if (GUILayout.Button("Sort Objects"))
        {
            if (Source != null && Rows > 0)
            {
                SortObjectsInEditor();
            }
            else if (Rows <= 0 || ColumnDistance <= 0 || Distance <= 0)
            {
                Debug.LogWarning("Rows can't be less than 1.");
            }
            else
            {
                Debug.LogWarning("You forgot to set a game object before sorting.");
            }
        }
    }
}