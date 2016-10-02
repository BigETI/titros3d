using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Delete highscore"))
        {
            Highscore highscore = new Highscore("titros3d.db");
            highscore.deleteHighscore();
        }

        if (GUILayout.Button("Drop highscore table"))
        {
            Highscore highscore = new Highscore("titros3d.db");
            highscore.dropHighscore();
        }
    }
}
