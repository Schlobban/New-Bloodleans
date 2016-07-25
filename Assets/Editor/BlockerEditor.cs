using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Blocker))]
public class BlockerEditor : Editor {

  void OnSceneGUI() {
    Blocker b = (target as Blocker);

    EditorGUI.BeginChangeCheck();
    Vector3 pos = Handles.FreeMoveHandle(b.unblockLocation, Quaternion.identity, 2, new Vector3(1, 1, 1), Handles.ArrowCap);
    if (EditorGUI.EndChangeCheck()) {
      Undo.RecordObject(target, "Unblock location");
      b.unblockLocation = pos;
    }
  }

}