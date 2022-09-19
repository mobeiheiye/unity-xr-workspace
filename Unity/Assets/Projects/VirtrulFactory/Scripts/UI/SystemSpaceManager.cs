using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VirtrulFactory.UI
{
    /// <summary>
    /// UI
    /// </summary>
    public sealed class SystemSpaceManager
    {
        private readonly static SystemSpaceManager instance = new SystemSpaceManager();
        public static SystemSpaceManager Instance
        {
            get
            {
                return instance;
            }
        }
        static SystemSpaceManager()
        {

        }
        private SystemSpaceManager()
        {

        }

    }

}