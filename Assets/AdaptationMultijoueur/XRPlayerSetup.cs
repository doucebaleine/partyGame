using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR;

public class XRPlayerSetup : NetworkBehaviour
{
    public GameObject cameraOffset;

    void Start()
    {
       if (!IsOwner)
       {
            // Désactiver la caméra locale pour les autres joueurs
           cameraOffset.GetComponent<Camera>().enabled = false;
       }
    }
}
