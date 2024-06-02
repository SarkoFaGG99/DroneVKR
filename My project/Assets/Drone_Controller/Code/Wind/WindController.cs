using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public Rigidbody RB;
    private float minWindSpeed = 1.0f; // Минимальная скорость ветра
    private float maxWindSpeed = 5.0f; // Максимальная скорость ветра
    private float minChangeInterval = 2.0f; // Минимальный интервал смены направления ветра
    private float maxChangeInterval = 5.0f; // Максимальный интервал смены направления ветра
    private float smoothness = 5.0f; // Плавность изменения направления ветра

    private Vector3 windDirection; // Направление ветра
    [SerializeField] private float windSpeed; // Текущая скорость ветра
    [SerializeField] private float changeInterval; // Текущий интервал смены направления ветра

    private bool isWindActive = true; // Флаг активности ветра

    void Start()
    {
        // Запускаем корутину для смены направления ветра
        StartCoroutine(ChangeWindCoroutine());
    }

    void Update()
    {
        // Применяем силу ветра к Rigidbody, только если ветер активен
        if (isWindActive) RB.AddForce(windDirection * windSpeed, ForceMode.Force);

        // Проверяем нажатие клавиши V для переключения ветра
        if (Input.GetKeyDown(KeyCode.V)) ToggleWind();
    }

    // Корутина для изменения направления и скорости ветра
    [SerializeField]
    IEnumerator ChangeWindCoroutine()
    {
        while (true)
        {
            // Выполняем только если ветер активен
            if (isWindActive)
            {
                // Генерируем случайные значения для осей x и z
                float x = Random.Range(-1.0f, 1.0f);
                float z = Random.Range(-1.0f, 1.0f);

                // Создаем новый вектор для направления ветра
                Vector3 newWindDirection = new Vector3(x, 0, z).normalized;

                // Генерируем случайную скорость ветра
                windSpeed = Random.Range(minWindSpeed, maxWindSpeed);

                // Генерируем случайный интервал для следующей смены направления
                changeInterval = Random.Range(minChangeInterval, maxChangeInterval);

                // Выполняем плавное изменение направления ветра
                float elapsedTime = 0;
                while (elapsedTime < smoothness)
                {
                    windDirection = Vector3.Lerp(windDirection, newWindDirection, elapsedTime / smoothness);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                windDirection = newWindDirection; // Устанавливаем финальное направление ветра

                // Ждем заданный интервал перед следующим изменением направления и скорости
                yield return new WaitForSeconds(changeInterval - smoothness); // Вычитаем smoothness, чтобы компенсировать время плавного изменения направления
            }
            else
            {
                yield return null; // Если ветер неактивен, просто ждем следующего кадра
            }
        }
    }

    // Функция для переключения состояния ветра
    private void ToggleWind()
    {
        isWindActive = !isWindActive; // Инвертируем состояние ветра
    }
}
