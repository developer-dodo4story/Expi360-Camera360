using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera360
{
    public class CustomResolution : MonoBehaviour
    {

        public int screenWidth = 11520;
        public int screenHeight = 1080;
        public int preferedRefreshRate = 60;


        // Use this for initialization
        void Start()
        {
            Screen.SetResolution(screenWidth, screenHeight, true, preferedRefreshRate);
        }
    }
}
