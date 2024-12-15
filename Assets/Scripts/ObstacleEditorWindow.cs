using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    private ObstacleData obstacleData;
    private bool[] obstacleGrid = new bool[100];

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI()
    {
        if (obstacleData == null)
        {
            if (GUILayout.Button("Create New Obstacle Data"))
            {
                obstacleData = CreateInstance<ObstacleData>();
                AssetDatabase.CreateAsset(obstacleData, "Assets/ObstacleData.asset");
                AssetDatabase.SaveAssets();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Obstacle Grid", EditorStyles.boldLabel);

            for (int y = 0; y < 10; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < 10; x++)
                {
                    int index = y * 10 + x;
                    obstacleGrid[index] = GUILayout.Toggle(obstacleGrid[index], "");
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Save Obstacles"))
            {
                obstacleData.obstacles = obstacleGrid;
                EditorUtility.SetDirty(obstacleData);
                AssetDatabase.SaveAssets();
            }
        }
    }

    private void OnEnable()
    {
        obstacleData = AssetDatabase.LoadAssetAtPath<ObstacleData>("Assets/ObstacleData.asset");
        if (obstacleData != null)
        {
            obstacleGrid = obstacleData.obstacles;
        }
    }
}