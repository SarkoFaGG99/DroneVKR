using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public Rigidbody RB;
    private float minWindSpeed = 1.0f; // ����������� �������� �����
    private float maxWindSpeed = 5.0f; // ������������ �������� �����
    private float minChangeInterval = 2.0f; // ����������� �������� ����� ����������� �����
    private float maxChangeInterval = 5.0f; // ������������ �������� ����� ����������� �����
    private float smoothness = 5.0f; // ��������� ��������� ����������� �����

    private Vector3 windDirection; // ����������� �����
    [SerializeField] private float windSpeed; // ������� �������� �����
    [SerializeField] private float changeInterval; // ������� �������� ����� ����������� �����

    private bool isWindActive = true; // ���� ���������� �����

    void Start()
    {
        // ��������� �������� ��� ����� ����������� �����
        StartCoroutine(ChangeWindCoroutine());
    }

    void Update()
    {
        // ��������� ���� ����� � Rigidbody, ������ ���� ����� �������
        if (isWindActive) RB.AddForce(windDirection * windSpeed, ForceMode.Force);

        // ��������� ������� ������� V ��� ������������ �����
        if (Input.GetKeyDown(KeyCode.V)) ToggleWind();
    }

    // �������� ��� ��������� ����������� � �������� �����
    [SerializeField]
    IEnumerator ChangeWindCoroutine()
    {
        while (true)
        {
            // ��������� ������ ���� ����� �������
            if (isWindActive)
            {
                // ���������� ��������� �������� ��� ���� x � z
                float x = Random.Range(-1.0f, 1.0f);
                float z = Random.Range(-1.0f, 1.0f);

                // ������� ����� ������ ��� ����������� �����
                Vector3 newWindDirection = new Vector3(x, 0, z).normalized;

                // ���������� ��������� �������� �����
                windSpeed = Random.Range(minWindSpeed, maxWindSpeed);

                // ���������� ��������� �������� ��� ��������� ����� �����������
                changeInterval = Random.Range(minChangeInterval, maxChangeInterval);

                // ��������� ������� ��������� ����������� �����
                float elapsedTime = 0;
                while (elapsedTime < smoothness)
                {
                    windDirection = Vector3.Lerp(windDirection, newWindDirection, elapsedTime / smoothness);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                windDirection = newWindDirection; // ������������� ��������� ����������� �����

                // ���� �������� �������� ����� ��������� ���������� ����������� � ��������
                yield return new WaitForSeconds(changeInterval - smoothness); // �������� smoothness, ����� �������������� ����� �������� ��������� �����������
            }
            else
            {
                yield return null; // ���� ����� ���������, ������ ���� ���������� �����
            }
        }
    }

    // ������� ��� ������������ ��������� �����
    private void ToggleWind()
    {
        isWindActive = !isWindActive; // ����������� ��������� �����
    }
}
