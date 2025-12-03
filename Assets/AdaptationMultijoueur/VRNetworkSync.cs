using UnityEngine;
using Unity.Netcode;
using Unity.XR.CoreUtils;

public class VRNetworkSync : NetworkBehaviour
{
    public Transform head, leftHand, rightHand;
    Transform headVR, leftHandVR, rightHandVR;
    XROrigin localXRRig;

    public override void OnNetworkSpawn() 
    {
        if (!IsOwner) 
        {
            enabled = false;
            return;
        }

        localXRRig = FindAnyObjectByType<XROrigin>();
        
        headVR = localXRRig.Camera.transform;
        leftHandVR = localXRRig.transform.Find("Camera Offset/Left Controller");
        rightHandVR = localXRRig.transform.Find("Camera Offset/Right Controller");
    }

    void Update()
    {
        if (!IsOwner) return;

        head.SetPositionAndRotation(headVR.position, headVR.rotation);
        leftHand.SetPositionAndRotation(leftHandVR.position, leftHandVR.rotation);
        rightHand.SetPositionAndRotation(rightHandVR.position, rightHandVR.rotation);
    }
}