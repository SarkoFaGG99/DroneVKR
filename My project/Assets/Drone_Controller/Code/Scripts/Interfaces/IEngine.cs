using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDrone
{
    public interface IEngine
    {
        void InitEngine();
        void UpdateEngine(Rigidbody rb, IP_Drone_Inputs input);
    }
}
