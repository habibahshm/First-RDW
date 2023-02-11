using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject desk;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject Env;
    [SerializeField] private GameObject zigzag;
    [SerializeField] private float radius;

    List<XRNodeState> nodes;

    private Vector3 Head_pos;
    private Vector3 user_initial_pos;

    private void Start()
    {
        //Get the user's current position and rotation in world coordinates, XR origin transform must be reset to align with wirld coordinates
        nodes = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodes);

        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
            {
                node.TryGetPosition(out Head_pos);
                user_initial_pos = Head_pos;
            }
        }

        //Place env position neasr the user
        Env.transform.position = new(Env.transform.position.x + Head_pos.x, Env.transform.position.y, Env.transform.position.z + Head_pos.z);
       
        //Adjust desk height according to the height of the user
        float deskHeight = (float)Head_pos.y - 0.5f;
        desk.transform.localScale = new Vector3(.5f, deskHeight, 1);
        desk.transform.position = new Vector3(desk.transform.position.x, deskHeight / 2.0f, desk.transform.position.z);

        zigzag.transform.position=new Vector3(zigzag.transform.position.x, desk.transform.position.y+1f, zigzag.transform.position.z);

        //Place stick on top of the desk
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

        InputTracking.GetNodeStates(nodes);
        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
            {
                node.TryGetPosition(out Head_pos);
                debugText.SetText(Head_pos.ToString());
                /*node.TryGetRotation(out Head_rot);*/
            }

        }

        float distance_travelled = Mathf.Abs(Head_pos.x - user_initial_pos.x);
        float ratio = (180*(1/radius))/Mathf.PI;

        Quaternion currentQ = Env.transform.localRotation;
        Vector3 currentRot = Env.transform.localEulerAngles;
        float deltaY = distance_travelled * ratio;
        float newYrot = currentRot.y - deltaY;

        Vector3 newRot = new Vector3(currentQ.x, newYrot, currentQ.z);
        currentQ.eulerAngles = newRot;
        Env.transform.rotation = currentQ;

        debugText.text = debugText.text +"\n distance traveled: " + distance_travelled + "\n current rotation: "+ newRot.ToString();

    }
}
