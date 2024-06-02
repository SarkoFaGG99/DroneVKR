using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyDrone
{
    [RequireComponent(typeof(PlayerInput))]
    public class IP_Drone_Inputs : MonoBehaviour
    {
        #region Variables
        private Vector2 cyclic;
        private float pedals;
        private float throttle;

        public Vector2 Cyclic { get => cyclic; }
        public float Pedals { get => pedals; }
        public float Throttle { get => throttle; }
        #endregion

        #region Main Methods
        void Update()
        {

        }
        #endregion

        #region Input Methods
        private void OnCyclic(InputValue value)
        {
            cyclic = value.Get<Vector2>();
        }

        private void OnPedals(InputValue value)
        {
            pedals = value.Get<float>();
        }

        private void OnThrottle(InputValue value)
        {
            throttle = value.Get<float>();
        }

        public void SetCyclic(Vector2 newCyclic)
        {
            cyclic = newCyclic;
        }

        public void SetPedals(float newPedals)
        {
            pedals = newPedals;
        }

        public void SetThrottle(float newThrottle)
        {
            throttle = newThrottle;
        }
        #endregion
    }
}
