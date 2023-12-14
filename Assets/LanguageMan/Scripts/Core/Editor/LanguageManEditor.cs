#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageMan))]
public class LanguageManEditor : Editor
{
    private SerializedProperty languagesProperty;
    private SerializedProperty wordsGroupProperty;
    private GUIContent importContent, exportContent;

    [MenuItem("Assets/Open LanguageMan List")]
    public static void OpenLanguageMan()
    {
        LanguageMan languageMan = LanguageMan.Instance;
        Selection.activeObject = languageMan;
    }

    private void OnEnable()
    {
        languagesProperty = serializedObject.FindProperty("Languages");
        wordsGroupProperty = serializedObject.FindProperty("WordGroups");

        Texture2D importIcon = EditorGUIUtility.FindTexture("import");
        Texture2D exportIcon = EditorGUIUtility.FindTexture("SaveAs");

        importContent = new GUIContent(" Import from Excel", importIcon);
        exportContent = new GUIContent(" Export to Excel", exportIcon);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        #region Header
        GUI.color = Color.green;
        GUILayout.BeginVertical("Box");
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = 30;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Add space to the left
        GUILayout.Label("LanguageMan", headerStyle, GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace(); // Add space to the right
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        GUILayout.EndVertical();
        GUILayout.Space(10);
        #endregion

        /* Rect rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true), GUILayout.Height(1));
   Handles.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.width + 10, rect.y));
   GUILayout.Space(10);


   #region Import & Export
   GUI.color = Color.white;
   GUILayout.BeginHorizontal("Box");
   if (GUILayout.Button(importContent, GUILayout.Height(30)))
       ImportFromExcel();
   if (GUILayout.Button(exportContent, GUILayout.Height(30)))
       ExportToExcel();
   GUILayout.EndHorizontal();
   GUILayout.Space(10);
   #endregion
   */

        Rect rect1 = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true), GUILayout.Height(1));
        Handles.DrawLine(new Vector2(rect1.x, rect1.y), new Vector2(rect1.width + 10, rect1.y));
        GUILayout.Space(10);

        #region Languages
        EditorGUILayout.PropertyField(languagesProperty, true);
        GUILayout.Space(10);
        #endregion

        Rect rect2 = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true), GUILayout.Height(1));
        Handles.DrawLine(new Vector2(rect2.x, rect2.y), new Vector2(rect2.width + 10, rect2.y));
        GUILayout.Space(10);

        #region Words
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(wordsGroupProperty, true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);
        #endregion

        serializedObject.ApplyModifiedProperties();
    }

    private void ImportFromExcel()
    {
        LanguageMan languageMan = (LanguageMan)target;

        string filePath = EditorUtility.OpenFilePanel("Select Language Text File", "", "xlsx");

        if (!string.IsNullOrEmpty(filePath))
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            EditorUtility.SetDirty(languageMan);
            EditorUtility.DisplayDialog("Import Successful", $"The file has been imported from {filePath}", "OK");
        }
    }

    private void ExportToExcel()
    {
        LanguageMan languageMan = (LanguageMan)target;

        string filePath = EditorUtility.SaveFilePanel("Save Language Text File", "", $"{Application.productName}_language_export", "xlsx");

        if (!string.IsNullOrEmpty(filePath))
        {
            languageMan.Export(filePath);
            EditorUtility.SetDirty(languageMan);
            EditorUtility.DisplayDialog("Export Successful", $"The file has been saved to {filePath}", "OK");
        }
    }
}
#endif