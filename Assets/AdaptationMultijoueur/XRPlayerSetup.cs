using UnityEngine;
using Unity.Netcode;

public class XRPlayerSetup : NetworkBehaviour
{
    void Start()
    {
        if(!IsOwner)
        {
            DesactiverVrLocal();
        }
    }

    void DesactiverVrLocal()
    {
        // Désactiver les caméras des autres joueurs
        foreach (Camera camera in GetComponentsInChildren<Camera>())
        {
            camera.enabled = false;
        }

        // Désactiver les écouteurs audio des autres joueurs
        foreach (AudioListener audioListener in GetComponentsInChildren<AudioListener>())
        {
            audioListener.enabled = false;
        }

        // Désactiver input VR des autres joueurs
        foreach (var input in GetComponentsInChildren<MonoBehaviour>())
        {
            if (input.GetType().Name.Contains("Locomotion") || 
                input.GetType().Name.Contains("ActionBased") ||
                input.GetType().Name.Contains("Input") ||
                input.GetType().Name.Contains("Interactor"))
            {
                input.enabled = false;
            }
        }
    }
}
