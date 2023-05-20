using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera360
{
    public class CameraSettingsConfig
    {
        public static CameraState currentState = CameraState.TopBottom;
        public static bool initialCameraSetup = false;
        public static Game3DState currentGameState;
        public static float cameraScale;
    }
}