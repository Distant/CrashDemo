using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraNode))]
[CanEditMultipleObjects]
public class NodeEditor : Editor
{
    private SerializedObject mNode;

    public void OnEnable()
    {
        mNode = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        if (GUILayout.Button("Create Node", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20))){
            AddNode();
        }

        mNode.ApplyModifiedProperties();
    }

    public void AddNode()
    {
        GameObject newNode = new GameObject();
        newNode.AddComponent<CameraNode>();
        newNode.transform.parent = GameObject.Find("CameraNodes").transform;
        newNode.name = "camera_node" + (GameObject.FindObjectsOfType<CameraNode>().Length - 1) ;
        newNode.transform.position = ((CameraNode)mNode.targetObject).gameObject.transform.position;
        newNode.transform.rotation = ((CameraNode)mNode.targetObject).gameObject.transform.rotation;

        GameObject newEdge = new GameObject();
        newEdge.AddComponent<CameraEdge>();
        newEdge.transform.parent = GameObject.Find("CameraEdges").transform;
        newEdge.name = "camera_edge" + GameObject.FindObjectsOfType<CameraEdge>().Length;

        newNode.GetComponent<CameraNode>().AddEdge(newEdge.GetComponent<CameraEdge>());
        ((CameraNode)mNode.targetObject).AddEdge(newEdge.GetComponent<CameraEdge>());
        newEdge.GetComponent<CameraEdge>().SetNodes((CameraNode)mNode.targetObject, newNode.GetComponent<CameraNode>());
    }
}