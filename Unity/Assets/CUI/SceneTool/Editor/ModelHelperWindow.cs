using UnityEngine;
using UnityEditor;

namespace ADWorkSpace
{
    /// <summary>
    /// 模型工具
    /// </summary>
    public class ModelHelperWindow : EditorWindow
    {
        public static bool IsDrawMeshRendererFrame = true;

        private ModelHelperWindow()
        {
            this.titleContent = new GUIContent("模型工具");
        }

        [MenuItem("COMMON/模型工具")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<ModelHelperWindow>();
        }

        private Material mat_Original;
        private Material mat_New;

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            //场景替换材质
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("场景替换材质");
            mat_Original = EditorGUILayout.ObjectField("原材质", mat_Original, typeof(Material), true) as Material;
            mat_New = EditorGUILayout.ObjectField("新材质", mat_New, typeof(Material), true) as Material;
            if (GUILayout.Button("替换材质"))
            {
                ReplaceSceneMaterial();
            }
            GUILayout.EndVertical();

            //找到丢失材质的物体
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("找到丢失材质的物体");
            if (GUILayout.Button("选取"))
            {
                SelectMissMaterial();
            }
            GUILayout.EndVertical();

            //找到丢失模型的物体
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("找到丢失模型的物体");
            if (GUILayout.Button("选取"))
            {
                SelectMissMesh();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("显示/隐藏模型尺寸外框");
            IsDrawMeshRendererFrame = GUILayout.Toggle(IsDrawMeshRendererFrame, "显示/隐藏");
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        private void ReplaceSceneMaterial()
        {
            if (!mat_Original || !mat_New)
            {
                Debug.LogError("模型工具 : 请正确选择 原材质 和 新材质");
                return;
            }
            Renderer[] renderers = Resources.FindObjectsOfTypeAll<Renderer>();
            foreach (var renderer in renderers)
            {
                if (renderer.gameObject.scene.isLoaded)
                {
                    if (renderer.sharedMaterials.Length == 1)
                    {
                        if (renderer.sharedMaterial == mat_Original)
                        {
                            renderer.sharedMaterial = mat_New;
                            Debug.Log("材质工具 : 已替换 : " + renderer.gameObject.scene.name + "-" + renderer.name);
                        }
                    }
                    else if (renderer.sharedMaterials.Length > 1)
                    {
                        Material[] mats = renderer.sharedMaterials;
                        for (int i = 0; i < mats.Length; i++)
                        {
                            if (mats[i] == mat_Original)
                            {
                                mats[i] = mat_New;
                                Debug.Log("材质工具 : 已替换 : " + renderer.gameObject.scene.name + "-" + renderer.name);
                            }
                        }
                        renderer.sharedMaterials = mats;
                    }
                }
            }
        }
        private void SelectMissMaterial()
        {
            Renderer[] rendererComponentes = Resources.FindObjectsOfTypeAll<Renderer>();
            foreach (var rendererComponente in rendererComponentes)
            {
                if (rendererComponente.gameObject.scene.isLoaded)
                {
                    if (rendererComponente.sharedMaterials.Length == 1)
                    {
                        if (rendererComponente.sharedMaterial == null)
                        {
                            EditorGUIUtility.PingObject(rendererComponente.gameObject);
                            Selection.objects = new Object[] { rendererComponente.gameObject };
                            return;
                        }
                    }
                    else if (rendererComponente.sharedMaterials.Length > 1)
                    {
                        Material[] mats = rendererComponente.sharedMaterials;
                        bool miss = false;
                        for (int i = 0; i < mats.Length; i++)
                        {
                            if (mats[i] == null)
                            {
                                miss = true;
                            }
                        }
                        if (miss)
                        {
                            EditorGUIUtility.PingObject(rendererComponente.gameObject);
                            Selection.objects = new Object[] { rendererComponente.gameObject };
                            return;
                        }
                    }
                }
            }
            Debug.LogError("模型工具 : 未找到找到丢失材质的物体");
        }
        private void SelectMissMesh()
        {
            MeshFilter[] meshComponentes = Resources.FindObjectsOfTypeAll<MeshFilter>();
            foreach (var meshComponent in meshComponentes)
            {
                if (meshComponent.gameObject.scene.isLoaded)
                {
                    if (meshComponent.sharedMesh == null)
                    {
                        EditorGUIUtility.PingObject(meshComponent.gameObject);
                        Selection.objects = new Object[] { meshComponent.gameObject };
                        return;
                    }
                }
            }
            Debug.LogError("模型工具 : 未找到找到丢失模型的物体");
        }
    }
}