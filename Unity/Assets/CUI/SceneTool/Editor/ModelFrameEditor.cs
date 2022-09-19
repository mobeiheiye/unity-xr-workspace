using UnityEditor;
using UnityEngine;

namespace ADWorkSpace
{
    [CustomEditor(typeof(MeshRenderer))]
    class ModelSizeFrameEditor : Editor
    {
        void OnSceneGUI()
        {
            if (ModelHelperWindow.IsDrawMeshRendererFrame)
            {
                MeshRenderer meshRenderer = target as MeshRenderer;
                float halfX = meshRenderer.bounds.size.x / 2;
                float halfY = meshRenderer.bounds.size.y / 2;
                float halfZ = meshRenderer.bounds.size.z / 2;

                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.BoldAndItalic;

                style.normal.textColor = Color.red;
                Handles.Label(meshRenderer.bounds.center + new Vector3(0, -halfY, -halfZ),
                    "sizeX : " + meshRenderer.bounds.size.x.ToString(), style);
                Handles.color = Color.red;
                Handles.DrawWireDisc(meshRenderer.bounds.center + new Vector3(0, -halfY, -halfZ), Vector3.left, 0.05f);

                style.normal.textColor = Color.green;
                Handles.Label(meshRenderer.bounds.center + new Vector3(-halfX, 0, -halfZ),
                    "sizeY : " + meshRenderer.bounds.size.y.ToString(), style);
                Handles.color = Color.green;
                Handles.DrawWireDisc(meshRenderer.bounds.center + new Vector3(-halfX, 0, -halfZ), Vector3.up, 0.05f);

                style.normal.textColor = Color.blue;
                Handles.Label(meshRenderer.bounds.center + new Vector3(-halfX, -halfY, 0),
                    "sizeZ : " + meshRenderer.bounds.size.z.ToString(), style);
                Handles.color = Color.blue;
                Handles.DrawWireDisc(meshRenderer.bounds.center + new Vector3(-halfX, -halfY, 0), Vector3.forward, 0.05f);

                Handles.color = Color.gray;
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(halfX, -halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(-halfX, -halfY, -halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(-halfX, -halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(-halfX, -halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(-halfX, -halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(-halfX, halfY, -halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(-halfX, halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(halfX, halfY, -halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(halfX, halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(halfX, -halfY, -halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(-halfX, -halfY, halfZ),
                    meshRenderer.bounds.center + new Vector3(-halfX, halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(-halfX, halfY, halfZ),
                    meshRenderer.bounds.center + new Vector3(halfX, halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(halfX, halfY, halfZ),
                    meshRenderer.bounds.center + new Vector3(halfX, -halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(halfX, -halfY, halfZ),
                    meshRenderer.bounds.center + new Vector3(-halfX, -halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(halfX, halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(halfX, halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(-halfX, halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(-halfX, halfY, halfZ), 2);
                Handles.DrawLine(meshRenderer.bounds.center + new Vector3(halfX, -halfY, -halfZ),
                    meshRenderer.bounds.center + new Vector3(halfX, -halfY, halfZ), 2);


            }
        }
    }
}