using MyDrone;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float batteryLevel = 100f;
    public float dischargingLevel = 0.05f;
    public TMP_Text BatteryText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            batteryLevel = 101f;
        }
    }
    private void FixedUpdate()
    {
        BatteryUpdate();
    }
    void BatteryUpdate()
    {
        foreach (IP_Drone_Engine item in FindObjectsOfType<IP_Drone_Engine>())
        {
            if (item.checkOffOnEngine == false) return;
        }
        BatteryText.text = "" + (int)(batteryLevel) + "%";
        if (batteryLevel > 0)
        {
            batteryLevel -= dischargingLevel;
        }
        else
        {
            foreach (IP_Drone_Engine item in FindObjectsOfType<IP_Drone_Engine>())
            {
                item.SwitchEngineOffOn();
            }
        }

    }
}
