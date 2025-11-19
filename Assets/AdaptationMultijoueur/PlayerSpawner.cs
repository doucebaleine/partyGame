using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // XR Rig prefab
    public int maxPlayers = 2; // Nombre maximum de joueurs

    void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    void OnClientConnected(ulong clientId)
    {
        Debug.Log($"OnClientConnected triggered: {clientId}");
        if (!NetworkManager.Singleton.IsServer) return;

        // Vérifier le nombre maximum de joueurs
        if (NetworkManager.Singleton.ConnectedClients.Count > maxPlayers)
        {
            Debug.Log("Nombre maximum de joueurs atteint.");
            NetworkManager.Singleton.DisconnectClient(clientId);
            return;
        }

        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }

    void OnClientDisconnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            NetworkObject playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (playerObject != null)
            {
                playerObject.Despawn(true);
            }
        }
    }
}