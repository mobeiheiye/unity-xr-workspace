using UnityEngine;
using System.Collections;

namespace CUI.Shape
{
    [ExecuteInEditMode]
    public class ThreeDCurveMesh : MonoBehaviour
    {
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] private Transform startAnchor;
        [SerializeField] private Transform endAnchor;

        void Start()
        {

        }

        void Update()
        {
            if (startAnchor && endAnchor)
            {
                Curve.DrawSimpleCurve(startAnchor.position,
                    startAnchor.up,
                    endAnchor.position,
                    endAnchor.up,
                    meshFilter.sharedMesh,
                    meshFilter.mesh);
            }
        }
    }
}