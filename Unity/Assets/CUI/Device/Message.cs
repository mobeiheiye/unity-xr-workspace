using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace CUI.Device
{
    public class Message
    {
        //[DllImport("__Internal")] public static extern void HelloString();

        [Serializable]
        public enum MessageType
        {
            InApp,
            Container,
            ServerPort
        }

        public struct MessageLog
        {
            public MessageType Type;
            public string Address;
            public string Head;
            public string Body;
        }

        public static void Send(MessageType type, string address, string head, string body)
        {
            switch (type)
            {
                case MessageType.InApp:
                    GameObject target = GameObject.Find(address);
                    if (target)
                    {
                        target.SendMessage(head, body);
                    }
                    break;
                case MessageType.Container:
#if UNITY_WEBGL
                    //和HTML通信
                    //Application.ExternalCall();
#elif UNITY_ANDROID
                    //和安卓原生通信


#elif UNITY_IOS
                    //和IOS原生通信


#endif
                    break;
                case MessageType.ServerPort:

                    break;
            }

        }



    }

    public enum ContainerType
    {
        Web,
        App
    }
    public enum ViewType
    {
        Orbit,
        Wander,
    }

    public struct DeviceOption
    {

    }
    public struct ContainerOption
    {
        public ContainerType type;

    }
}