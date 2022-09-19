using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResistanceTest
{
    public class ResistanceTestController : MonoBehaviour
    {
        [SerializeField] private Slider slider_StartHeight;
        [SerializeField] private Text txt_Height;

        [SerializeField] private Button btn_AddTest;
        [SerializeField] private Button btn_RemoveTest;

        [SerializeField] private Button btn_StartTest;
        [SerializeField] private Button btn_RestTest;

        [SerializeField] private Transform testRoot;
        [SerializeField] private ResistanceTestBody testPrefab;
        [SerializeField] private float testBodyOffsetX = 20;

        private List<ResistanceTestBody> bodyList = new List<ResistanceTestBody>();

        private float startHeight = 30;

        private void Start()
        {
            if (btn_AddTest)
            {
                btn_AddTest.onClick.AddListener(delegate ()
                {
                    ResistanceTestBody testBody = Instantiate(testPrefab, testRoot);
                    bodyList.Add(testBody);
                    int testIndex = bodyList.IndexOf(testBody);
                    testBody.name = testPrefab.name + ":" + (testIndex + 1).ToString();
                    ReSetPosition();
                    testBody.Init();
                });
            }
            if (btn_RemoveTest)
            {
                btn_RemoveTest.onClick.AddListener(delegate ()
                {
                    if (bodyList.Count > 0)
                    {
                        ResistanceTestBody testBody = bodyList[bodyList.Count - 1];
                        bodyList.Remove(testBody);
                        DestroyImmediate(testBody.gameObject);
                        ReSetPosition();
                    }
                });
            }

            if (slider_StartHeight)
            {
                slider_StartHeight.value = (startHeight - 10) / 90;
                slider_StartHeight.onValueChanged.AddListener(delegate (float value)
                {
                    startHeight = value;
                    ReSetPosition();
                    txt_Height.text = value.ToString();
                });
            }

            if (btn_RestTest)
            {
                btn_RestTest.onClick.AddListener(delegate ()
                {
                    InitTest();
                });
            }
            if (btn_StartTest)
            {
                btn_StartTest.onClick.AddListener(delegate ()
                {
                    StartTest();
                });
            }
        }

        private void ReSetPosition()
        {
            for (int i = 0; i < bodyList.Count; i++)
            {
                bodyList[i].transform.position = new Vector3((bodyList.Count % 2 == 0) ? (i - bodyList.Count / 2 + 0.5f) * testBodyOffsetX : (i - bodyList.Count / 2) * testBodyOffsetX, startHeight, 0);
            }
        }
        private void InitTest()
        {
            foreach (var item in bodyList)
            {
                item.StopTest();
                Vector3 startPos = item.transform.position;
                startPos.y = startHeight;
                item.transform.position = startPos;
                item.Init();
            }
        }
        private void StartTest()
        {
            foreach (var item in bodyList)
            {
                item.StartTest();
            }
        }
    }
}