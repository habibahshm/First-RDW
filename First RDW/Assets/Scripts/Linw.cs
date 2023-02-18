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
    private Stick stickScript;
    public bool onLine = false;

    void Start()
    {
        stickScript = GameObject.FindWithTag("Player").GetComponent<Stick>();
    }

    private void OnTriggerEnter(Collider other)
    {
        debugText.SetText("trigeeres");
    }

    private void OnTriggerStay(Collider other)
    {
        /*debugText.text = "line z: " + transform.position.z + "\n collision name: " + collision.gameObject.name
              + "\n stick pos: " + collision.gameObject.transform.position.z;*/
        if (other.gameObject.CompareTag("Player"))
        {

            debugText.SetText("collided");
            onLine = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            debugText.SetText("exited linw col");
            onLine = false;
        }
    }
    
}
