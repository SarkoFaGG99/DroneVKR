using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
namespace MyDrone
{
    [RequireComponent(typeof(Rigidbody))]
    public class IP_Base_Rigidbody : MonoBehaviour
    {
        #region Variables
        [Header("Rigidbody Properties")]
        private float weightKg = 1f;
        protected float Speed;
        public float currentAltitude;

        protected Rigidbody rb;
        protected float startDrag;
        protected float startAngularDrag;
        [SerializeField] TMP_Text SpeedText;
        [SerializeField] TMP_Text HeightText;
        public IP_Drone_Controller DroneController;
        
        #endregion

        #region Main Methods
        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.mass = weightKg;
                startDrag = rb.drag;
                startAngularDrag = rb.angularDrag;
                DroneController = FindObjectOfType<IP_Drone_Controller>();
            }
        }

        protected void Update()
        {
            SpeedUpdate();
            HeightUpdate();
        }

        void FixedUpdate()
        {
            if (!rb) return;
            HandlePhysics();
        }


        #endregion
        #region Custom Methods
        protected virtual void HandlePhysics()
        {

        }
        #endregion

        #region Speed&Height
        void SpeedUpdate()
        {
            Speed = rb.velocity.magnitude * 3.6f;
            SpeedText.text = $"{(int)Speed} κμ/χ";
        }

        void HeightUpdate()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                currentAltitude = hit.distance;
                HeightText.text = currentAltitude.ToString("F2") + " μ";
            }
        }
        
        #endregion
    }
}