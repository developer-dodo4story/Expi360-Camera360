using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraBoundsDisplay : EditorWindow
{
    /*
    private int width = 1;
    private int distance = 90;
    private Color color = Color.blue;

    [MenuItem("Window/Custom/Camera Bounds Display")]
    static void Init()
    {
        var window = GetWindow<CameraBoundsDisplay>();
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
    void OnSceneGUI(SceneView sceneView)
    {
    }
    void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ultimate camera separator", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Line width: ", EditorStyles.boldLabel, GUILayout.Width(120));
        width = EditorGUILayout.IntSlider(width, 0, 30, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Line distance: ", EditorStyles.boldLabel, GUILayout.Width(120));
        distance = EditorGUILayout.IntSlider(distance, 0, 30, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Line color: ", EditorStyles.boldLabel, GUILayout.Width(120));
        color = EditorGUILayout.ColorField(color, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

    }

    static float frequency = 600f;
    static float time = 0f;
    [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected)]
    private static void GizmosDrawer(Transform bounds, GizmoType aGizmoType)
    {
        time += Time.deltaTime;
        if(time > frequency)
        {
            time = 0f;
        }
        else
        {
            return;
        }

        if (Camera.allCameras.Length <= 1)
        {
            return;
        }
        if (Camera.allCameras.Length > 12)
        {
            return;
        }

        List<Vector3> prevPoints = new List<Vector3>();
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = (Camera)Camera.allCameras[i];

            var fovToRad = cam.fieldOfView * Mathf.Deg2Rad;
            var hFov = 2 * Mathf.Atan(Mathf.Tan(fovToRad / 2) * cam.aspect);
            var hFovDeg = Mathf.Rad2Deg * hFov;

            Vector3 bottom = cam.transform.position + (Quaternion.AngleAxis(hFovDeg / 2, Vector3.up)) * (Quaternion.AngleAxis(cam.fieldOfView / 2, cam.transform.right)) * cam.transform.forward * 100;
            Vector3 top = cam.transform.position + (Quaternion.AngleAxis(hFovDeg / 2, Vector3.up)) * (Quaternion.AngleAxis(-cam.fieldOfView / 2, cam.transform.right)) * cam.transform.forward * 100;
            Vector3 point = Vector3.Lerp(bottom, top, 0.5f);

            if (i < Camera.allCameras.Length / 2)
            {
                prevPoints.Add(point);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(Vector3.Lerp(point, prevPoints[i - Camera.allCameras.Length / 2], 0.5f), new Vector3(.5f, (top - bottom).magnitude, .5f));
            }


        }
        //}
    }
    
    */
}