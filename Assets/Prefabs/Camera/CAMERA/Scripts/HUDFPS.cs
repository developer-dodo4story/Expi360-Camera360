using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Camera360
{
    public class HUDFPS : MonoBehaviour
    {
        public float updateInterval = 0.5F;

        private float accum = 0;
        private int frames = 0;
        private float timeleft;

        private Text text;

        void Start()
        {
            Application.targetFrameRate = 90;

            text = GetComponent<Text>();
            timeleft = updateInterval;
        }

        void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0)
            {
                float fps = accum / frames;
                string format = System.String.Format("{0:F2}", fps);

                if (fps > 60)
                    text.text = "<color=green>" + format + "</color>";
                else if (fps > 30)
                    text.text = "<color=yellow>" + format + "</color>";
                else
                    text.text = "<color=red>" + format + "</color>";


                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}