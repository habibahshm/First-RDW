using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject desk;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject Env;
    [SerializeField] private GameObject zigzag;

    private Vector3 Head_pos;
    /*private Quaternion Head_rot;
    private Quaternion currentQ;
    private Quaternion stick_rot;*/

    private void Start()
    {
        //Get the user's current position and rotation in world coordinates, XR origin transform must be reset to align with wirld coordinates
        List<XRNodeState> nodes = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodes);

        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
            {
                node.TryGetPosition(out Head_pos);
                /*node.TryGetRotation(out Head_rot);*/
            }
        }

        //Adjust Env direction according to where the user is looking at the beginning (rotation around y axis)
       /* Vector3 rot = Head_rot.eulerAngles;
        Vector3 newRot = new Vector3(Env.transform.localRotation.x, rot.y, Env.transform.localRotation.z);
        currentQ.eulerAngles = newRot;
        Env.transform.localRotation = currentQ;*/

        //Place env position neasr the user
        Env.transform.position = new(Env.transform.position.x + Head_pos.x, Env.transform.position.y, Env.transform.position.z + Head_pos.z);
        //Env.transform.position = new(Head_pos.x - 1.2f, Env.transform.position.y, Head_pos.z);

        //Adjust desk height according to the height of the user
        float deskHeight = (float)Head_pos.y - 0.5f;
        desk.transform.localScale = new Vector3(.5f, deskHeight, 1);
        desk.transform.position = new Vector3(desk.transform.position.x, deskHeight / 2.0f, desk.transform.position.z);

        zigzag.transform.position=new Vector3(zigzag.transform.position.x, desk.transform.position.y+1f, zigzag.transform.position.z);

        //Place stick on top of the desk
      /*  Vector3 stick_orient = new Vector3(stick.transform.localRotation.x, stick.transform.localRotation.y, rot.y+5);
        stick_rot.eulerAngles = stick_orient;
        stick.transform.localRotation = stick_rot;*/

        stick.transform.position = new Vector3(desk.transform.position.x, deskHeight + 0.025f, desk.transform.position.z);


    }
    private void Update()
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHand.isValid)
        {
            bool buttonPressed;
            rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed);
            if (buttonPressed)
            {
                SceneManager.LoadScene(0);
            }
        }
       
            
    }
}
