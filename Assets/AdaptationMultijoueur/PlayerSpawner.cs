using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // XR Rig prefab
    public int maxPlayers = 2; // Nombre maximum de joueurs

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        NetworkManager.Singleton.OnClientDisconnectCallback += RemovePlayer;
    }

    void SpawnPlayer(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        // Vérifier le nombre maximum de joueurs
        if (NetworkManager.Singleton.ConnectedClients.Count >maxPlayers)
        {
            Debug.Log("Nombre maximum de joueurs atteint.");
            NetworkManager.Singleton.DisconnectClient(clientId);
            return;
        }

        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }

    void RemovePlayer(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkObject playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (playerObject != null)
            {
                playerObject.Despawn();
                Destroy(playerObject.gameObject);
            }
        }
    }
}