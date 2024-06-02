using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.HID;
using System.Reflection.Emit;
using UnityEngine.InputSystem;
namespace MyDrone
{
    [RequireComponent(typeof(IP_Drone_Inputs))]
    public class IP_Drone_Controller : IP_Base_Rigidbody
    {
        #region Variables   
        [Header("Controle Properties")]
        [SerializeField] private float minMaxPitch = 30f;
        [SerializeField] private float minMaxRoll = 30f;
        [SerializeField] private float yawPower = 4f;
        [SerializeField] private float lerpSpeed = 2f;

        private IP_Drone_Inputs input;
        private List<IP_Drone_Engine> engines = new List<IP_Drone_Engine>();

        private float finalPitch;
        private float finalRoll;
        private float yaw;
        private float finalYaw;

        public Transform target; // Целевая точка
        //public float moveSpeed = 1f; // Скорость движения
        public float stoppingDistance = 1f; // Расстояние, на котором дрон остановится
        public float obstacleAvoidanceDistance = 10f;
        public bool autoUp = false;
        public bool autoPilot = false;
        Coroutine FlyUpCor;
        Coroutine MoveToTargetCor;

        public delegate void AutoPilotChangedEventHandler(bool isAutoPilot);
        public event AutoPilotChangedEventHandler OnAutoPilotChanged;

        private bool autoPilotEnabled = false;
        



        #endregion



        #region Main Methods
        void Start()
        {
            input = GetComponent<IP_Drone_Inputs>();
            engines = GetComponentsInChildren<IP_Drone_Engine>().ToList<IP_Drone_Engine>();
        }
        #endregion
        bool checkPhys;
        #region Custom Methods
        protected override void HandlePhysics()
        {
            HandleEngines();
            if(checkPhys == true) HandleAutopilotControls();
            else HandleControls();
        }
        protected virtual void HandleControls()
        {
            if (!FindObjectOfType<IP_Drone_Engine>().checkOffOnEngine) return;

            float pitch = input.Cyclic.y * minMaxPitch;
            float roll = -input.Cyclic.x * minMaxRoll;
            yaw += input.Pedals * yawPower;

            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);
            finalYaw = Mathf.Lerp(finalYaw, yaw, Time.deltaTime * lerpSpeed);

            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }
        protected virtual void HandleAutopilotControls()
        {
            if (target == null) return;

            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0; // Игнорируем высоту

            Vector3 localDirection = transform.InverseTransformDirection(directionToTarget).normalized;

            float pitch = Mathf.Clamp(localDirection.z * minMaxPitch, -minMaxPitch, minMaxPitch);
            float roll = Mathf.Clamp(-localDirection.x * minMaxRoll, -minMaxRoll, minMaxRoll);
            yaw = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);
            finalYaw = Mathf.Lerp(finalYaw, yaw, Time.deltaTime * lerpSpeed);

            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }
       
        protected virtual void HandleEngines()
        {
            foreach (IEngine engine in engines)
            {
                engine.UpdateEngine(rb, input);
            }
        }
        private void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.H))
            {
                SwitchAutoPilot();
                MoveToTarget();
            }
        }
        void SwitchAutoPilot()
        {
            autoPilot = !autoPilot;
            if (autoPilot)
            {
                autoUp = true;
                OnAutoPilotChanged?.Invoke(true);
            }
            else
            {
                autoUp = false;
                checkPhys = false;
                OnAutoPilotChanged?.Invoke(false);
            }
        }
        #endregion

        void OnCollisionEnter(Collision collision)
        {
            Collider myCollider = collision.GetContact(0).thisCollider;
            if (Speed >= 2)
            {
                myCollider.GetComponent<IP_Drone_Engine>().Crash();
            }
        }

        public IEnumerator FlyUp()
        {
            while (autoPilot)
            {
                if (autoPilot) MaxCurrentPower();
                
                if (currentAltitude >= 10f)
                {
                    CurrentPower();
                    MoveToTargetCor = null;
                    startMoveToTargetFunc();

                    yield break;
                }
                yield return null;
            }
        }
        void MaxCurrentPower()
        {
            IP_Drone_Engine[] maxPower = GetComponentsInChildren<IP_Drone_Engine>();
            foreach (IP_Drone_Engine engine in engines) engine.currentPowerCoef = 1.5f;
        }
        void CurrentPower()
        {
            IP_Drone_Engine[] currentPower = GetComponentsInChildren<IP_Drone_Engine>();
            foreach (IP_Drone_Engine engine in engines) engine.currentPowerCoef = 1f;
        }
        IEnumerator MoveToTargetFunc()
        {
            checkPhys = true;
            while (autoPilot)
            {
                Vector3 directionToTarget = target.position - transform.position;
                directionToTarget.y = 0; // Игнорируем высоту для движения в горизонтальной плоскости

                float distanceToTarget = Vector3.Distance(new Vector3(target.position.x, 0, target.position.z), new Vector3(transform.position.x, 0, transform.position.z));
                if (currentAltitude < 10) MaxCurrentPower();
                else CurrentPower();

                if (distanceToTarget < 2f) // Проверяем расстояние в горизонтальной плоскости
                {
                    checkPhys = false;

                    Debug.Log("Target reached! Distance: " + distanceToTarget); 

                    IP_Drone_Engine[] maxPower = GetComponentsInChildren<IP_Drone_Engine>();
                    foreach (IP_Drone_Engine engine in engines)
                    {
                        engine.currentPowerCoef = 0.7f;
                    }
                    yield break; // Останавливаем корутину
                }
                Vector3 avoidanceDirection = AvoidObstacles(directionToTarget.normalized);
                rb.AddForce(directionToTarget.normalized, ForceMode.Force); // Добавляем силу для перемещения к цели
                yield return null; // Ждем следующий кадр
            }
        }
        void stopFlyUpCor()
        {
            //if (FlyUpCor == null) return;
            CurrentPower();

            if (FlyUpCor != null)
            {
                StopCoroutine(FlyUpCor);
                FlyUpCor = null;
            }

            autoUp = false;
        }
        void startFlyUpCor()
        {
            FlyUpCor = StartCoroutine(FlyUp());
        }
        void startMoveToTargetFunc()
        {
            MoveToTargetCor = StartCoroutine(MoveToTargetFunc());
        }
        public void MoveToTarget()
        {
            if (target == null) return;
            switch (autoUp)
            {
                case true:
                    if (FlyUpCor == null) startFlyUpCor();
                    break;
                case false:
                    stopFlyUpCor();
                    break;
            }
        }
        private Vector3 AvoidObstacles(Vector3 direction)
        {
            RaycastHit hit;
            Vector3 avoidanceDirection = direction;

            if (Physics.Raycast(transform.position, direction, out hit, obstacleAvoidanceDistance))
            {
                Vector3 hitNormal = hit.normal;
                hitNormal.y = 0; // Убираем вертикальную составляющую
                avoidanceDirection = direction + hitNormal;
            }

            return avoidanceDirection.normalized;
        }
    }
}