using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display3DData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    void Awake()
    {
        DontDestroyOnLoad(this);
        /*
         * Add event to update text
         * */
    }
    private void OnDestroy()
    {
        /*
         * Remove event to update text
         * */
    }
    void UpdateText(float dist, float spread)
    {
        text.text = 
            "Distance: " + dist.ToString("0.00") + 
            "\n"
            + "Spread: " + spread.ToString("0.00");
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetActiveState(!text.gameObject.activeSelf);
            }
        }
    }
    void SetActiveState(bool active)
    {
        text.gameObject.SetActive(active);
    }
}
