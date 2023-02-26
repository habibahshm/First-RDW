using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject desk;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject Env;
    [SerializeField] private GameObject zigzag;
    [SerializeField] private float radius;
    private enum Rotation_dir:int {  left = -1,  right = 1 }
    [SerializeField] private Rotation_dir rotation_dir;

    List<XRNodeState> nodes;

    private Vector3 Head_pos;
    private Quaternion Head_rot;
    private Vector3 inital_pos;
    private Vector3 inital_rot;
    private Vector3 env_forward;

    private bool paused = false;
    private bool prev_state_pause = false;
    private bool prev_state_touch = false;
    private bool ui_active = false;
    private float curv_gain;
  


    private void Start()
    {
        Time.timeScale = 1;

        //Get the user's current position and rotation in world coordinates, XR origin transform must be reset to align with wirld coordinates
        nodes = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodes);

        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
            {
                node.TryGetPosition(out Head_pos);
                node.TryGetRotation(out Head_rot);
                inital_pos = Head_pos;
            }
        }

        //Adjust Env direction according to where the user is looking at the beginning (rotation around y axis)
        Vector3 newRot = new Vector3(Env.transform.localRotation.x, Head_rot.eulerAngles.y, Env.transform.localRotation.z);
        Quaternion currentQ = new Quaternion();
        currentQ.eulerAngles = newRot;
        Env.transform.rotation = currentQ;

        //Place env position neasr the user
        Env.transform.position = new(Env.transform.position.x + Head_pos.x, Env.transform.position.y, Env.transform.position.z + Head_pos.z);
        inital_rot = Env.transform.rotation.eulerAngles;
        env_forward = Env.transform.forward;
       
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
            if (button_pressed != prev_state_pause)
            {
                if (button_pressed)
                {
                    Time.timeScale = paused ? 1 : 0;
                    paused = !paused;
                }
                prev_state_pause = button_pressed;
            }

            bool secondry_touched;
            bool secondary_t = rightHand.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondry_touched) && secondry_touched;
            if (secondary_t != prev_state_touch)
            {
                if (secondary_t)
                {
                    ui_active= !ui_active;
                    UI.SetActive(ui_active);
                }
                prev_state_touch = secondary_t;
            }

        }

        InputTracking.GetNodeStates(nodes);
        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
                node.TryGetPosition(out Head_pos);
        }

        Vector3 distance_vector = Head_pos - inital_pos;
        float dist_so_far = distance_vector.magnitude;
        dist_so_far = Mathf.Round(dist_so_far * 10f) / 10f;

        if (Vector3.Dot(distance_vector.normalized, env_forward) > 0.8)
        {
            float deltaY_sofar = dist_so_far * curv_gain;
            float newYrot = inital_rot.y + ((int)rotation_dir) * deltaY_sofar;

            var newRot = Quaternion.Euler(inital_rot.x, newYrot, inital_rot.z);
            Env.transform.rotation = Quaternion.Slerp(Env.transform.rotation, newRot, Time.deltaTime);

            debugText.SetText("\n total dist: " + dist_so_far +
                "\n delta y so far: " + deltaY_sofar.ToString() + "\n current rotation: " + Env.transform.localEulerAngles.ToString());
        }
            
    }

}
