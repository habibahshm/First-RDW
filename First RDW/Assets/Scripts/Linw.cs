using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using TMPro;

public class Linw : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI debugText;
    public bool onLine = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            onLine = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
            onLine = false;
    }
    
}
