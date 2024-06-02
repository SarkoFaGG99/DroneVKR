using TMPro;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private int RingID;
    public bool CheckedRing = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<RingManager>().DroneInRing(RingID);
            CheckedRing = true;
        }
    }
}
