using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] int maxJoueurs = 2;
    [SerializeField] string nomSceneJeu;

    public override void OnNetworkSpawn()
    {
        if (!IsServer || NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback += GererConnexionClient;

        GererConnexionClient(NetworkManager.Singleton.LocalClientId);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer || NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback -= GererConnexionClient;
    }

    void GererConnexionClient(ulong clientId)
    {
        int nbJoueursConnectes = NetworkManager.Singleton.ConnectedClients.Count;

        Debug.Log($"Client connecté: {clientId}. Nombre de joueurs connectés: {nbJoueursConnectes}/{maxJoueurs}");

        if (nbJoueursConnectes == maxJoueurs)
        {
            ChargerSceneJeu();
        }
    }

    void ChargerSceneJeu()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(nomSceneJeu, LoadSceneMode.Single);
    }
}
