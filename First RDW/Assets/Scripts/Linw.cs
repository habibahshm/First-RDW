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
    private float z_tol = 0.05f;
    public bool onLine = false;

    void Start()
    {
        stickScript = GameObject.FindWithTag("Player").GetComponent<Stick>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        /*debugText.text = "line z: " + transform.position.z + "\n collision name: " + collision.gameObject.name
                + "\n stick pos: " + collision.gameObject.transform.position.z;*/ 
        if (collision.CompareTag("Player"))
        {

            if (stickScript != null && Mathf.Abs(collision.gameObject.transform.position.z-transform.position.z)<=z_tol )
            {

                debugText.SetText("collided");
                onLine = true;

            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        debugText.SetText("exited");
        onLine=false;
    }

}
