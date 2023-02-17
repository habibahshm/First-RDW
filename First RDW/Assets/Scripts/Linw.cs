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
        debugText.SetText("trigeeres - FUCK!!!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        debugText.SetText("entered");
    }

    private void OnCollisionStay(Collision collision)
    {
        /*debugText.text = "line z: " + transform.position.z + "\n collision name: " + collision.gameObject.name
               + "\n stick pos: " + collision.gameObject.transform.position.z;*/
        if (collision.gameObject.CompareTag("Player"))
        {

            debugText.SetText("collided");
            onLine = true;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            debugText.SetText("exited linw col");
            onLine = false;
        }
     }

}
