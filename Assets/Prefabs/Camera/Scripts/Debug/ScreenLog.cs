using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Camera360
{
    [RequireComponent(typeof(Text))]
    public class ScreenLog : MonoBehaviour
    {

        static string logString;
        delegate void UpdateTextDelegate();
        static UpdateTextDelegate UpdateMethod;

        Text textComponent;

        // Use this for initialization
        void Awake()
        {
            textComponent = GetComponent<Text>();
            UpdateMethod = UpdateText;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void Write(string text)
        {
            logString = text;
            UpdateMethod();
        }

        public void UpdateText()
        {
            textComponent.text = logString;

        }
    }
}
