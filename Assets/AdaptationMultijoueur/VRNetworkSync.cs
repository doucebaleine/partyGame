using UnityEngine;
using Unity.Netcode;

public class VRNetworkSync : NetworkBehaviour
{
    public Transform head, leftHand, rightHand;
    public Transform headVR, leftHandVR, rightHandVR;

    /*public override void OnNetworkSpawn() 
    {
        if (!isOwner) 
        {
            enabled = false;
            return;
        }
        var xrRig = FindObjectOfType<XROrigin>();
        headVR = xrRig.Camera.transform;
        leftHandVR = xrRig.transform.Find("LeftHand Controller");
        rightHandVR = xrRig.transform.Find("RightHand Controller");
    }

    void Update()
    {

    }
    */
}