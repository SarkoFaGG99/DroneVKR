using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyDrone
{
    [RequireComponent(typeof(BoxCollider))]
    public class IP_Drone_Engine : MonoBehaviour, IEngine
    {
        #region Variables
        [Header("Engine Properties")]
        private static float maxPower = 10f;
        [SerializeField] public float currentPowerCoef = 1f;
        [SerializeField] public float maxPowerCoef = 1.5f;
        [SerializeField] private float minPowerCoef = 0f;

        [Header("Propeller Properties")]
        [SerializeField] private Transform propeller;
        [SerializeField] private float propRotSpeed = 3f;
        public bool checkOffOnEngine = true; // true - включен
        
        #endregion
        #region Interface Methods
        public void InitEngine()
        {
            throw new System.NotImplementedException();
        }
        public void UpdateEngine(Rigidbody rb, IP_Drone_Inputs input)
        {
            if (!checkOffOnEngine) return;

            Vector3 upVec = transform.up;
            upVec.x = 0f;
            upVec.z = 0f;
            float diff = 1 - upVec.magnitude;
            float finalDiff = Physics.gravity.magnitude * diff;

            Vector3 engineForce = Vector3.zero;
            engineForce = transform.up * ((rb.mass * Physics.gravity.magnitude + finalDiff) + (input.Throttle * maxPower)) / 4f;
            
            rb.AddForceAtPosition(engineForce * currentPowerCoef, transform.position, ForceMode.Force);
            HendlePropellers();
        }
        void UpdateCoef()
        {
            if (currentPowerCoef > maxPowerCoef) currentPowerCoef = maxPowerCoef;
            if (currentPowerCoef < minPowerCoef) currentPowerCoef = minPowerCoef;
        }
        void changeCoef(bool IsUp)
        {
            Debug.Log("changeCoef");
            float toCh = 0.1f;
            if (IsUp == true) currentPowerCoef += toCh;
            if (IsUp == false) currentPowerCoef -= toCh;
        }
        private void Update()
        {
            UpdateCoef();
            if (Input.GetKeyDown(KeyCode.E)) SwitchEngineOffOn();
            if (Input.GetKeyDown(KeyCode.F))
            {
                maxPowerCoef = 1.5f;
                currentPowerCoef = 1f;
            }
            if (Input.GetKeyDown(KeyCode.Equals)) changeCoef(true);
            if (Input.GetKeyDown(KeyCode.Minus)) changeCoef(false);
        }
        void HendlePropellers()
        {
            if (!propeller)
            {
                return;
            }
            propeller.Rotate(Vector3.up, propRotSpeed);
        }
        #endregion
        public void SwitchEngineOffOn()
        {  
            checkOffOnEngine = !checkOffOnEngine;
        }
        #region Collision
        public void Crash()
        {
            Debug.Log("Crash" + gameObject);
            maxPowerCoef -= 0.1f;
            if (maxPowerCoef < 0) maxPowerCoef = 0;
        }
        #endregion
    }
}
