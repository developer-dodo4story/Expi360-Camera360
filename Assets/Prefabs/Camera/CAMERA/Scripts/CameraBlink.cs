using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Camera360
{
    public class CameraBlink : MonoBehaviour
    {

        
        #region Settings
        [SerializeField]
        [Tooltip("Speed of manual changes")]
        public float tweakSpeed = 0.02f;
        [SerializeField]
        [Tooltip("Initial distance of the convergence point")]
        public float distance = 200f;
        [SerializeField]
        [Tooltip("How often does a blink occur")]
        public float refreshRate = 120;
        [SerializeField]
        [Tooltip("Turn on to enable left and right camera blinking")]
        public bool blinkingEnabled = false;
        [SerializeField]
        [Tooltip("Turn on to enable manual camera controls")]
        public bool controlManually = true;
        [SerializeField]
        [Tooltip("Turn off to disable all 3D settings")]
        public bool is3DEnabled = true;
        #endregion



        #region CameraStates
        [Space(10)]
        [Tooltip("Add custom 3D settings states to change between")]
        [SerializeField] private List<Game3DState> states = new List<Game3DState>();
        #endregion

        #region Cameras
        [Tooltip("Add left cameras here")]
        public List<Transform> leftCameras = new List<Transform>();
        [Tooltip("Add right cameras here")]
        public List<Transform> rightCameras = new List<Transform>();
        #endregion


        [Space(10)]
        #region PrivateProperties
        private Game3DState state;
        private Game3DState lastState;
        private Vector3 initialLookCenterLocalPosition = Vector3.zero;
        private Vector3 initialCameraLocalPosition = Vector3.zero;
        private float tweakedSpread;
        private float tweakedCurve;
        private List<Transform> lookCenters = new List<Transform>();
        bool rightcams = false;
        #endregion

        #region Text
        [SerializeField]
        private Transform debugCanvas;
        [SerializeField]
        private Text spreadText;
        [SerializeField]
        private Text curveText;
        #endregion
        
        void Start()
        {
            InitialCameraSetup();
            SubscribeAllStates();
            InitialStateChange();
        }
        void SubscribeAllStates()
        {
            foreach(Game3DState state in states)
            {
                System.Action act = null;
                act += delegate { ChangeState(state); };
                state.Subscribe(act);
            }
        }
        void InitialStateChange()
        {
            foreach(Game3DState st in states)
            {
                if(st.invokeMethod == InvokeMethodType.GameStarted)
                {
                    ChangeState(st);
                }
            }
        }
        private void OnDestroy()
        {
            foreach (Game3DState state in states)
            {
                System.Action act = null;
                act += delegate { ChangeState(state); };
                state.Unsubscribe(act);
            }
        }
        private void OnEnable()
        {
           StartCoroutine(ChangeStateAfterFrame());
        }
        IEnumerator ChangeStateAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            ChangeStateAfterEnable();
        }
        void InitialCameraSetup()
        {
            StartCoroutine(BlinkCamsSave());
            Application.targetFrameRate = (int)refreshRate;
            var tmpGameObject = new GameObject();
            for (int i = 0; i < leftCameras.Count; i++)
            {
                Transform newLook = Instantiate(tmpGameObject, leftCameras[i].parent).transform;
                newLook.position += (rightCameras[i].position - leftCameras[i].position) * 0.5f;
                newLook.position += (leftCameras[i].forward * distance + rightCameras[i].forward * distance) * 0.5f;
                lookCenters.Add(newLook);
            }
            initialLookCenterLocalPosition = lookCenters[0].localPosition;
            initialCameraLocalPosition = leftCameras[0].localPosition;
            Destroy(tmpGameObject);
        }
        void ChangeState(Game3DState state)
        {
            CameraSettingsConfig.currentGameState = state;
            if (!is3DEnabled)
            {
                return;
            }
            if(gameObject.activeSelf)
            {
                StartCoroutine(ChangeStateDelayed(state));
            }
        }
        void ChangeStateToGameplay()
        {
            ChangeState(states[1]);
        }
        void ChangeStateToEnd()
        {
            ChangeState(states[2]);
        }
        void ChangeStateAfterEnable()
        {
            if (CameraSettingsConfig.currentGameState == null)
            {
                return;
            }
            SetValuesForState(CameraSettingsConfig.currentGameState);
        }
        IEnumerator ChangeStateDelayed(Game3DState state)
        {
            if(gameObject.activeSelf)
            {
                yield return new WaitForSeconds(state.changeDelay);
            }
            SetValuesForState(state);
        }
        void SetValuesForState(Game3DState newState)
        {
            ResetScale(newState);

            tweakedCurve = newState.convergencePointDistance;
            tweakedSpread = newState.cameraSpread;
            Debug.Log("Changed " + newState.convergencePointDistance + " " + newState.cameraSpread);
            for (int i = 0; i < lookCenters.Count; i++)
            {
                SetCameraCenter(i, newState);
                SetCameraSpread(i, newState);
            }

            DownscaleChildren(newState);
            lastState = state;
            state = newState;
        }
        void ResetScale(Game3DState newState)
        {
        }
        void DownscaleChildren(Game3DState newState)
        {
        }
        void SetCameraCenter(int index, Game3DState newState)
        {
            Vector3 newPos = Vector3.Lerp(leftCameras[index].position, rightCameras[index].position, 0.5f);
            Vector3 movementVector = leftCameras[index].localPosition == Vector3.zero ? leftCameras[index].right : (rightCameras[index].position - leftCameras[index].position).normalized;
            lookCenters[index].position = newPos + Vector3.Cross(movementVector, Vector3.up)  * newState.convergencePointDistance;
        }
        void SetCameraSpread(int index, Game3DState newState)
        {
            Vector3 movementVector = leftCameras[index].localPosition == Vector3.zero ? leftCameras[index].right : (rightCameras[index].position - leftCameras[index].position).normalized;
            
            leftCameras[index].localPosition = Vector3.zero;
            leftCameras[index].position += movementVector * newState.cameraSpread;


            rightCameras[index].localPosition = Vector3.zero;
            rightCameras[index].position -= movementVector * newState.cameraSpread;
        }
        private void Update()
        {
            TweakCamera();
            ShowDebugText();
        }
        void ShowDebugText()
        {
            if (spreadText && curveText)
            {
                //spreadText.text = "3DSpread: " + ((int)(tweakedSpread * 100) / 100);
                spreadText.text = "3DSpread: " + tweakedSpread;
                //curveText.text = "3DCurve: " + ((int)(tweakedCurve * 100) / 100);
                curveText.text = "3DCurve: " + tweakedCurve;
            }
        }
        void TweakCamera()
        {
            /*
             * Manual camera controls
             * */

            if (Input.GetKeyDown(KeyCode.O) && debugCanvas != null)
            {
                debugCanvas.gameObject.SetActive(!debugCanvas.gameObject.activeSelf);
            }
            if (!controlManually)
            {
                return;
            }
            if (Input.GetKey(KeyCode.A))
            {
                foreach (Transform tr in leftCameras)
                {
                    tr.position += tr.right * tweakSpeed;
                }
                foreach (Transform tr in rightCameras)
                {
                    tr.position -= tr.right * tweakSpeed;
                }
                tweakedSpread += tweakSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                foreach (Transform tr in leftCameras)
                {
                    tr.position -= tr.right * tweakSpeed;
                }
                foreach (Transform tr in rightCameras)
                {
                    tr.position += tr.right * tweakSpeed;
                }
                tweakedSpread -= tweakSpeed;

            }
            if (Input.GetKey(KeyCode.W))
            {
                for (int i = 0; i < lookCenters.Count; i++)
                {
                    lookCenters[i].position = Vector3.MoveTowards(lookCenters[i].position, Vector3.Lerp(leftCameras[i].position, rightCameras[i].position, 0.5f), -tweakSpeed * 20);
                }
                tweakedCurve += tweakSpeed * 20;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                ChangeState(states[1]);
            }
            if (Input.GetKey(KeyCode.S))
            {
                bool tooClose = false;
                for (int i = 0; i < lookCenters.Count; i++)
                {
                    Vector3 newPos = Vector3.MoveTowards(lookCenters[i].position, Vector3.Lerp(leftCameras[i].position, rightCameras[i].position, 0.5f), tweakSpeed * 20);
                    if ((newPos - Vector3.Lerp(leftCameras[i].position, rightCameras[i].position, 0.5f)).magnitude>2)
                    {
                        lookCenters[i].position = newPos;
                    }
                    else
                    {
                        tooClose = true;
                    }
                }
                if(tooClose)
                {
                    return;
                }
                tweakedCurve -= tweakSpeed * 20;
            }
            if (Input.GetKey(KeyCode.Z))
            {
                refreshRate -= tweakSpeed * 10;
                if (refreshRate < 1)
                {
                    refreshRate = 1;
                }
            }
            if (Input.GetKey(KeyCode.C))
            {
                refreshRate += tweakSpeed * 10;
            }


            for (int i = 0; i < leftCameras.Count; i++)
            {
                leftCameras[i].LookAt(lookCenters[i]);
                rightCameras[i].LookAt(lookCenters[i]);
            }

        }
        IEnumerator BlinkCams()
        {
            while (blinkingEnabled)
            {

                if (rightcams)
                {

                    foreach (Transform cam in leftCameras)
                    {
                        cam.gameObject.SetActive(true);
                    }
                    foreach (Transform cam in rightCameras)
                    {
                        cam.gameObject.SetActive(false);
                    }
                }
                else
                {
                    foreach (Transform cam in leftCameras)
                    {
                        cam.gameObject.SetActive(false);
                    }
                    foreach (Transform cam in rightCameras)
                    {
                        cam.gameObject.SetActive(true);
                    }
                }
                rightcams = !rightcams;
                yield return new WaitForSeconds(1 / refreshRate);
            }
        }

        public void ResetBlink()
        {
            StartCoroutine(BlinkCamsSave());
        }

        IEnumerator BlinkCamsSave()
        {
            while (blinkingEnabled)
            {
                rightcams = Time.time % ((1 / refreshRate) * 2f) > (1 / refreshRate);

                if (rightcams)
                {

                    foreach (Transform cam in leftCameras)
                    {
                        cam.gameObject.SetActive(true);
                    }
                    foreach (Transform cam in rightCameras)
                    {
                        cam.gameObject.SetActive(false);
                    }
                }
                else
                {
                    foreach (Transform cam in leftCameras)
                    {
                        cam.gameObject.SetActive(false);
                    }
                    foreach (Transform cam in rightCameras)
                    {
                        cam.gameObject.SetActive(true);
                    }
                }

                yield return new WaitForSeconds((1 / refreshRate) * 0.5f);
            }
        }

    }
}
