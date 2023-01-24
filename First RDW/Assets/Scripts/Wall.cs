using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;

public class Wall : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    Stick stickScript;
    

    // Start is called before the first frame update
    void Start()
    {
      
        stickScript = GameObject.FindWithTag("Player").GetComponent<Stick>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            if(stickScript != null)
            {
               
                IXRSelectInteractor hand = stickScript.firstInteractorSelecting;

                if (hand != null)
                {
                    /*debugText.SetText(hand.ToString());*/
                    InputDevice controller;

                    if (hand.ToString().ToLower().Contains("right"))
                    {
                        controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    }
                    else
                    {
                        controller = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    }
                    controller.SendHapticImpulse(0, .3f, .1f);
                }
               
                
            }
            
        }
    }


}
