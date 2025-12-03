using UnityEngine;
using Unity.Netcode;
using Unity.XR.CoreUtils;

public class VRNetworkSync : NetworkBehaviour
{
    public Transform head, leftHand, rightHand;
    public Transform headVR, leftHandVR, rightHandVR;

    public override void OnNetworkSpawn() 
    {
        if (!IsOwner) 
        {
            enabled = false;
            return;
        }

        var xrRig = FindAnyObjectByType<XROrigin>();
        
        headVR = xrRig.Camera.transform;
        leftHandVR = xrRig.transform.Find("Camera Offset/Left Controller");
        rightHandVR = xrRig.transform.Find("Camera Offset/Right Controller");
    }

    void Update()
    {
        if (!IsOwner) return;

        head.SetPositionAndRotation(headVR.position, headVR.rotation);
        leftHand.SetPositionAndRotation(leftHandVR.position, leftHandVR.rotation);
        rightHand.SetPositionAndRotation(rightHandVR.position, rightHandVR.rotation);
    }
}