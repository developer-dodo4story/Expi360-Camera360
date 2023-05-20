using GameRoomServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera360
{
    public class CameraStrech : MonoBehaviour
    {
        Camera cam;
        public float height = 1f;
        public float width = 1f;

        // Use this for initialization
        void Start()
        {
            cam = GetComponent<Camera>();
            LoadConfigAndSetupValues();
        }

        private void LoadConfigAndSetupValues()
        {
            GameConfig config = null;
            if (SerializeHelper.LoadFromJSON("gameConfig", SerializeHelper.pathToProject, ref config))
            {
                height = config.heightStretch;
                width = config.widthStretch;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //stretch view//
            cam.ResetProjectionMatrix();
            var m = cam.projectionMatrix;

            m.m11 *= height;
            m.m00 *= width;
            cam.projectionMatrix = m;
        }
    }
}
