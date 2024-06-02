using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyDrone
{
    public class AutoPilotStatusText : MonoBehaviour
    {
        public TMP_Text statusText; // Ссылка на компонент текста

        private IP_Drone_Controller droneController; // Ссылка на контроллер дрона

        private void Start()
        {
            // Получаем ссылку на контроллер дрона
            droneController = GetComponent<IP_Drone_Controller>();

            // Подписываемся на событие изменения режима автопилота
            droneController.OnAutoPilotChanged += UpdateTextVisibility;

            // Изначально скрываем текст
            statusText.enabled = false;
        }

        private void UpdateTextVisibility(bool isAutoPilot)
        {
            // Активируем или деактивируем текст в зависимости от режима автопилота
            statusText.enabled = isAutoPilot;
        }

        private void OnDestroy()
        {
            // Отписываемся от события при уничтожении объекта
            droneController.OnAutoPilotChanged -= UpdateTextVisibility;
        }
    }
}
