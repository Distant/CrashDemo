using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraGraph))]
[CanEditMultipleObjects]
public class GraphEditor : Editor
{
    private SerializedObject mGraph;
    private CameraNode[] nodesList;

    public void OnEnable() {
        mGraph = new SerializedObject(target);
        nodesList = GameObject.FindObjectsOfType<CameraNode>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("Nodes", EditorStyles.boldLabel);

        for (int i = 0; i < nodesList.Length; i++) {
            EditorGUILayout.ObjectField(nodesList[i], typeof(CameraNode), true);
        }

        if (GUILayout.Button("Add New Node", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20))){
            AddNode();
            nodesList = GameObject.FindObjectsOfType<CameraNode>();
        }

        mGraph.ApplyModifiedProperties();
    }

    public void AddNode() {
        GameObject newObj = new GameObject();
        newObj.AddComponent<CameraNode>();
        newObj.transform.parent = GameObject.Find("CameraNodes").transform;
        newObj.name = "camera_node" + nodesList.Length;
    }
}