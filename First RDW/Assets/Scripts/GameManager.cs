using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

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
    private Vector3 prev_pos;
    private bool paused = false;
    private bool prev_state = false;
    private float curv_gain;

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
                prev_pos = Head_pos;
            }
        }

        //Place env position neasr the user
        Env.transform.position = new(Env.transform.position.x + Head_pos.x, Env.transform.position.y, Env.transform.position.z + Head_pos.z);
       
        //Adjust desk height according to the height of the user
        float deskHeight = (float)Head_pos.y - 0.5f;
        desk.transform.localScale = new Vector3(.5f, deskHeight, 1);
        desk.transform.position = new Vector3(desk.transform.position.x, deskHeight / 2.0f, desk.transform.position.z);

        //Place stick on top of the desk
        stick.transform.position = new Vector3(desk.transform.position.x, deskHeight + 0.025f, desk.transform.position.z);

        curv_gain = (180 * (1 / radius)) / Mathf.PI;
        curv_gain = Mathf.Round(curv_gain * 1000f) / 1000f;
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

            bool pause_button;
            bool button_pressed = rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out pause_button) && pause_button;
            if (button_pressed != prev_state)
            {
                if (button_pressed)
                {
                    Time.timeScale = paused ? 1 : 0;
                    paused = !paused;
                }
                prev_state = button_pressed;
            }
        }

        InputTracking.GetNodeStates(nodes);
        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
            {
                node.TryGetPosition(out Head_pos);
                /*debugText.SetText(Head_pos.ToString());*/
                /*node.TryGetRotation(out Head_rot);*/
            }

        }

        float delta_d = Mathf.Abs(Head_pos.x - prev_pos.x);
        delta_d = Mathf.Round(delta_d * 10f) / 10f;

        //debugText.SetText(delta_d.ToString());

        if(delta_d > 0)
        {
            Quaternion currentQ = Env.transform.localRotation;
            Vector3 currentRot = Env.transform.localEulerAngles;
            float deltaY = delta_d * curv_gain;
            float newYrot = currentRot.y - deltaY;

            Vector3 newRot = new Vector3(currentQ.x, newYrot, currentQ.z);
            currentQ.eulerAngles = newRot;
            Env.transform.rotation = currentQ;

          /*  debugText.SetText("\n delta dist: " + delta_d +
                "\n delta y: " + deltaY.ToString() + "\n current rotation: " + newRot.ToString());*/

            prev_pos = Head_pos;
        }
       

    }
}
