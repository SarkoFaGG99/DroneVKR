using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RingManager : MonoBehaviour
{
    [SerializeField] List<GameObject> CheckGalochka = new List<GameObject>();
    [SerializeField] GameObject GalochkaPrefab;
    [SerializeField] Transform GalochkaParent;
    public void DroneInRing(int ringID)
    {
        CheckGalochka[ringID].transform.GetChild(1).gameObject.SetActive(true);

        Stopwatch.Instance.StartTimer();
        foreach (Ring r in FindObjectsOfType<Ring>())
        {
            if (r.CheckedRing == false)
            {
                return;
            }
        }
        Stopwatch.Instance.StopTimer();
    }
    // Start is called before the first frame update
    void Start()
    {
        RingFinder();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ResetGalochkaR();
            Stopwatch.Instance.ResetTimer();
        }
    }
    void RingFinder()
    {
        for (int i = 1; i <= FindObjectsOfType<Ring>().Length; i++)
        {
            GameObject checkGalochka = Instantiate(GalochkaPrefab, GalochkaParent);
            checkGalochka.GetComponent<TMP_Text>().text = "Кольцо " + i;

            CheckGalochka.Add(checkGalochka);
        }
    }
    void ResetGalochkaR()
    {
        for (int i = 0; i < CheckGalochka.Count; i++)
        {
            CheckGalochka[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
