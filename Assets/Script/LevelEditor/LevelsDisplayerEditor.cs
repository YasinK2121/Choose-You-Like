using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelsDisplayer))]
public class LevelsDisplayerEditor : Editor
{
    private LevelData _current;
    private TrueObject _currentChildClass;
    private FalseObject _currentChildClass2;

    LevelsDisplayer _target;
    string ChildToggleText;
    string toggleText;

    public override void OnInspectorGUI()
    {
        GUI.backgroundColor = Color.white;
        _target = (LevelsDisplayer)target;

        if (GUILayout.Button("ADD NEW LEVEL"))
        {
            LevelData temp = new LevelData();
            _target.LevelAndStepList.Add(temp);
        }

        for (int i = 0; i < _target.LevelAndStepList.Count; i++)
        {
            _current = _target.LevelAndStepList[i];

            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("EXPAND LEVEL"))
            {
                _current.toggle = !_current.toggle;
            }

            if (_current.toggle)
            {
                toggleText = "LEVEL " + (i + 1).ToString() + "---> OPENED";
            }
            else
            {
                toggleText = "LEVEL " + (i + 1).ToString();
            }
            _current.toggle = EditorGUILayout.Foldout(_current.toggle, toggleText);

            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("REMOVE LEVEL"))
            {
                _target.LevelAndStepList.Remove(_current);
                break;
            }

            EditorGUILayout.EndHorizontal();


            if (_current.toggle)
            {
                GUI.backgroundColor = Color.white;

                if (GUILayout.Button("ADD NEW TRUE"))
                {
                    TrueObject temp = new TrueObject();
                    _current.trueObject.Add(temp);
                }

                for (int a = 0; a < _target.LevelAndStepList[i].trueObject.Count; a++)
                {
                    _currentChildClass = _target.LevelAndStepList[i].trueObject[a];

                    EditorGUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.yellow;


                    if (GUILayout.Button("EXPAND TRUE"))
                    {
                        _currentChildClass.toggle = !_currentChildClass.toggle;
                    }

                    if (_currentChildClass.toggle)
                    {
                        ChildToggleText = "TRUE " + (a + 1).ToString() + "---> OPENED";
                    }
                    else
                    {
                        ChildToggleText = "TRUE " + (a + 1).ToString();
                    }

                    _currentChildClass.toggle = EditorGUILayout.Foldout(_currentChildClass.toggle, ChildToggleText);
                    GUI.backgroundColor = Color.red;

                    if (GUILayout.Button("REMOVE TRUE"))
                    {
                        _target.LevelAndStepList[i].trueObject.Remove(_currentChildClass);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.backgroundColor = Color.white;

                    if (_currentChildClass.toggle)
                    {
                        DrawChildStep();
                        DrawChildIntField();
                    }
                }

                if (GUILayout.Button("ADD NEW FALSE"))
                {
                    FalseObject temp = new FalseObject();
                    _current.falseObject.Add(temp);
                }

                for (int a = 0; a < _target.LevelAndStepList[i].falseObject.Count; a++)
                {
                    _currentChildClass2 = _target.LevelAndStepList[i].falseObject[a];

                    EditorGUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.yellow;


                    if (GUILayout.Button("EXPAND FALSE"))
                    {
                        _currentChildClass2.toggle = !_currentChildClass2.toggle;
                    }

                    if (_currentChildClass2.toggle)
                    {
                        ChildToggleText = "FALSE " + (a + 1).ToString() + "---> OPENED";
                    }
                    else
                    {
                        ChildToggleText = "FALSE " + (a + 1).ToString();
                    }

                    _currentChildClass2.toggle = EditorGUILayout.Foldout(_currentChildClass2.toggle, ChildToggleText);
                    GUI.backgroundColor = Color.red;

                    if (GUILayout.Button("REMOVE FALSE"))
                    {
                        _target.LevelAndStepList[i].falseObject.Remove(_currentChildClass2);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.backgroundColor = Color.white;

                    if (_currentChildClass2.toggle)
                    {
                        DrawChildStep2();
                        DrawChildIntField2();
                    }
                }
            }
            _current = null;
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    public void DrawChildIntField()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index");
        _currentChildClass.gObjectCount = EditorGUILayout.IntField(_currentChildClass.gObjectCount);
        EditorGUILayout.EndHorizontal();
    }

    public void DrawChildStep()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Step");
        _currentChildClass.categories = (Categories)EditorGUILayout.EnumPopup(_currentChildClass.categories);
        EditorGUILayout.EndHorizontal();
    }

    public void DrawChildIntField2()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index");
        _currentChildClass2.gObjectCount = EditorGUILayout.IntField(_currentChildClass2.gObjectCount);
        EditorGUILayout.EndHorizontal();
    }

    public void DrawChildStep2()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Step");
        _currentChildClass2.categories = (Categories)EditorGUILayout.EnumPopup(_currentChildClass2.categories);
        EditorGUILayout.EndHorizontal();
    }
}
