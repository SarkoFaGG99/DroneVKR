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
        // Получаем ссылки на двигатели
        engines = new List<IP_Drone_Engine>(FindObjectsOfType<IP_Drone_Engine>());

        // Вызываем функцию для отображения мощности двигателей
        DisplayEnginePowers();
    }

    // Update is called once per frame
    void Update()
    {
        // Поскольку мощность двигателей может изменяться со временем, 
        // обновляем отображение мощности в каждом кадре
        DisplayEnginePowers();
    }

    void DisplayEnginePowers()
    {
        // Проверяем, существуют ли объекты двигателей и соответствующие текстовые объекты
        if (engines != null && engines.Count > 0 && LF_power != null && RF_power != null && LB_power != null && RB_power != null)
        {
            // Обновляем текст каждого текстового объекта, отображая мощность соответствующего двигателя
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
