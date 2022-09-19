using System.Collections.Generic;
using UnityEngine;

namespace CUI.Device
{
    /// <summary>
    /// 运行时Loger
    /// </summary>
    internal class RunningLoger : MonoBehaviour
    {
        private struct Log
        {
            public string message;
            public string stackTrace;
            public LogType type;
        }

        #region Inspector Settings  
        public bool restrictLogCount = false;
        public int maxLogs = 1000;
        #endregion

        private readonly List<Log> logs = new List<Log>();
        private Vector2 scrollPosition;
        private bool visible;
        private bool collapse;
        private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };
        private const string windowTitle = "Console";
        private const int margin = 20;
        private static readonly GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        private static readonly GUIContent closeLabel = new GUIContent("Close", "Close Panel.");
        private static readonly GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
        private readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
        private Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
        private static readonly int wakeUpCount = 7;
        private static readonly float wakeUpTime = 2;

        private int clickCount = 0;
        private float timer = 0;

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void Show()
        {
            if (clickCount == 0)
            {
                timer = 0;
            }
            if (timer < wakeUpTime)
            {
                clickCount++;
            }
            if (clickCount >= wakeUpCount && timer <= wakeUpTime)
            {
                visible = true;
                timer = 0;
                clickCount = 0;
            }
        }

        private void Update()
        {
            if (clickCount != 0)
            {
                timer += Time.deltaTime;
            }
            if (timer > wakeUpTime)
            {
                clickCount = 0;
                timer = 0;
            }
        }

        private void OnGUI()
        {
            if (!visible)
            {
                return;
            }
            windowRect = GUILayout.Window(111111, windowRect, DrawConsoleWindow, windowTitle);
        }

        /// <summary>  
        /// Displays a window that lists the recorded logs.  
        /// </summary>  
        /// <param name="windowID">Window ID.</param>  
        private void DrawConsoleWindow(int windowID)
        {
            DrawLogsList();
            DrawToolbar();

            // Allow the window to be dragged by its title bar.  
            GUI.DragWindow(titleBarRect);
        }

        /// <summary>  
        /// Displays a scrollable list of logs.  
        /// </summary>  
        private void DrawLogsList()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // Iterate through the recorded logs.  
            for (var i = 0; i < logs.Count; i++)
            {
                var log = logs[i];

                // Combine identical messages if collapse option is chosen.  
                if (collapse && i > 0)
                {
                    var previousMessage = logs[i - 1].message;

                    if (log.message == previousMessage)
                    {
                        continue;
                    }
                }

                GUI.contentColor = logTypeColors[log.type];

                GUIStyle _style = new GUIStyle();
                _style.fontSize = 100;

                GUILayout.Label(log.message + "::::::::::" + log.stackTrace);
            }

            GUILayout.EndScrollView();

            // Ensure GUI colour is reset before drawing other components.  
            GUI.contentColor = Color.white;
        }

        /// <summary>  
        /// Displays options for filtering and changing the logs list.  
        /// </summary>  
        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(clearLabel))
            {
                logs.Clear();
            }
            if (GUILayout.Button(closeLabel))
            {
                visible = false;
            }

            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        /// <summary>  
        /// Records a log from the log callback.  
        /// </summary>  
        /// <param name="message">Message.</param>  
        /// <param name="stackTrace">Trace of where the message came from.</param>  
        /// <param name="type">Type of message (error, exception, warning, assert).</param>  
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            logs.Add(new Log
            {
                message = message,
                stackTrace = stackTrace,
                type = type,
            });

            TrimExcessLogs();
        }

        /// <summary>  
        /// Removes old logs that exceed the maximum number allowed.  
        /// </summary>  
        private void TrimExcessLogs()
        {
            if (!restrictLogCount)
            {
                return;
            }

            var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);

            if (amountToRemove == 0)
            {
                return;
            }

            logs.RemoveRange(0, amountToRemove);
        }
    }
}