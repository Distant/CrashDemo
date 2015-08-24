using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GemPhysical))]
public class GemPositioneHandle : Editor {
		void OnSceneGUI () {
		((GemPhysical)target).endPos = Handles.PositionHandle(((GemPhysical)target).endPos, 
			                                    Quaternion.identity);
			if (GUI.changed)
				EditorUtility.SetDirty (target);
		}
	}