using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyDrone
{
    public class AutoPilotStatusText : MonoBehaviour
    {
        public TMP_Text statusText; // ������ �� ��������� ������

        private IP_Drone_Controller droneController; // ������ �� ���������� �����

        private void Start()
        {
            // �������� ������ �� ���������� �����
            droneController = GetComponent<IP_Drone_Controller>();

            // ������������� �� ������� ��������� ������ ����������
            droneController.OnAutoPilotChanged += UpdateTextVisibility;

            // ���������� �������� �����
            statusText.enabled = false;
        }

        private void UpdateTextVisibility(bool isAutoPilot)
        {
            // ���������� ��� ������������ ����� � ����������� �� ������ ����������
            statusText.enabled = isAutoPilot;
        }

        private void OnDestroy()
        {
            // ������������ �� ������� ��� ����������� �������
            droneController.OnAutoPilotChanged -= UpdateTextVisibility;
        }
    }
}
