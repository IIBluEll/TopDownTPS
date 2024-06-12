using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HM_TopView.PlayerFOV
{
    [CustomEditor(typeof(PlayerFOV))]
    public class PlayerFOVEditor : Editor
    {
        private void OnSceneGUI()
        {
            PlayerFOV fov = (PlayerFOV)target;
 
            Handles.color = Color.green;
            Handles.DrawWireArc(fov.transform.position,Vector3.up,Vector3.forward, 360, fov.viewRadius);

            Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
            Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
            
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
            
            Handles.color = Color.red;

            foreach (var visibleTarget in fov.visibleTarget)
            {
                Handles.DrawLine(fov.transform.position, visibleTarget.position);
            }
        }
    }
}

