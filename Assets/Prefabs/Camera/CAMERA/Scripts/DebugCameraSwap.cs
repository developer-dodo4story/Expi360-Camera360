using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

namespace Camera360
{
    public class DebugCameraSwap : MonoBehaviour
    {
        [SerializeField]
        private List<SwappableCamera> cameras;
        [SerializeField]
        private CameraState initialCameraState;

        private Dictionary<CameraState, CameraBlink> cameraDictionary;
        [SerializeField] private bool disablePostProcessing;

        void Start()
        {
            CreateDictionary();
            InitialCameraSetup();
            SetPostProcessing();
        }

        void Update()
        {
            CameraToggle();
        }

        private void SetPostProcessing()
        {
            foreach (PostProcessingBehaviour ppb in GetComponentsInChildren<PostProcessingBehaviour>())
            {
                ppb.enabled = !disablePostProcessing;
            }
        }

        void CameraToggle()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeCamera(CameraState.Blink);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeCamera(CameraState.SideBySide);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeCamera(CameraState.TopBottom);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeCamera(CameraState.Debug);
            }

            SetPostProcessing();
        }
        void CreateDictionary()
        {
            cameraDictionary = new Dictionary<CameraState, CameraBlink>();
            foreach(SwappableCamera cam in cameras)
            {
                cameraDictionary.Add(cam.state, cam.camera);
            }
        }
        void ChangeCamera(CameraState state)
        {
            foreach(CameraBlink cam in cameraDictionary.Values)
            {
                cam.gameObject.SetActive(false);
            }
            cameraDictionary[state].gameObject.SetActive(true);
            cameraDictionary[state].ResetBlink();
            CameraSettingsConfig.currentState = state;
        }


        void InitialCameraSetup()
        {
            if(!CameraSettingsConfig.initialCameraSetup)
            {
#if UNITY_STANDALONE
                ChangeCamera(initialCameraState);
#endif
#if UNITY_EDITOR
                ChangeCamera(CameraState.Debug);
#endif
                CameraSettingsConfig.initialCameraSetup = true;
            }
            else
            {
                ChangeCamera(CameraSettingsConfig.currentState);
            }

        }
    }
}
