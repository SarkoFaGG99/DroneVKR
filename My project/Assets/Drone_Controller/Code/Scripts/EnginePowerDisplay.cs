using MyDrone;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnginePowerDisplay : MonoBehaviour
{
    public TMP_Text LF_power;
    public TMP_Text RF_power;
    public TMP_Text LB_power;
    public TMP_Text RB_power;

    private List<IP_Drone_Engine> engines;

    // Start is called before the first frame update
    void Start()
    {
        // �������� ������ �� ���������
        engines = new List<IP_Drone_Engine>(FindObjectsOfType<IP_Drone_Engine>());

        // �������� ������� ��� ����������� �������� ����������
        DisplayEnginePowers();
    }

    // Update is called once per frame
    void Update()
    {
        // ��������� �������� ���������� ����� ���������� �� ��������, 
        // ��������� ����������� �������� � ������ �����
        DisplayEnginePowers();
    }

    void DisplayEnginePowers()
    {
        // ���������, ���������� �� ������� ���������� � ��������������� ��������� �������
        if (engines != null && engines.Count > 0 && LF_power != null && RF_power != null && LB_power != null && RB_power != null)
        {
            // ��������� ����� ������� ���������� �������, ��������� �������� ���������������� ���������
            LF_power.text = engines[1].currentPowerCoef.ToString("0.0");
            RF_power.text = engines[0].currentPowerCoef.ToString("0.0");
            LB_power.text = engines[3].currentPowerCoef.ToString("0.0");
            RB_power.text = engines[2].currentPowerCoef.ToString("0.0");
        }
        else
        {
            Debug.LogWarning("Some engines or power texts are not assigned in EnginePowerDisplay.");
        }
    }
}
