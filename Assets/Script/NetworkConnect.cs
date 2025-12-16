using UnityEngine;
using Unity.Netcode;

public class NetworkConnect : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    } 

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
