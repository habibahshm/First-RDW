using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI debugText;
    private Vector3 head_vel;

    private void Start()
    {
      
       
    }
    private void Update()
    {

        List<XRNodeState> nodes = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodes);
        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.Head)
            {
                node.TryGetVelocity(out head_vel);
                debugText.SetText(head_vel.ToString()); 
            }
        }


    }
}
