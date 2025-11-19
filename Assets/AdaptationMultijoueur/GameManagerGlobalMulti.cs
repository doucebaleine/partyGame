using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;

public class GameManagerGlobalMulti : NetworkBehaviour
{
    public static GameManagerGlobalMulti instance;

    public GameObject playerPrefab; // XR Rig prefab
    public GameObject instructionMinigame1;
    public GameObject instructionMinigame2;
    public GameObject instructionMinigame3;

    // Varialbles réseau pour suivre l'état d'avancement des mini-jeux
    public NetworkVariable<bool> minigame1Completed = new NetworkVariable<bool>();
    public NetworkVariable<bool> minigame2Completed = new NetworkVariable<bool>();
    public NetworkVariable<bool> minigame3Completed = new NetworkVariable<bool>();

    bool finJeuDeclenchee = false;

    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }  
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnNouveauClientConnecte;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnNouveauClientConnecte;
    }

    void OnNouveauClientConnecte(ulong clientId)
    {
        Debug.Log($"OnClientConnected triggered: {clientId}");
        if (!NetworkManager.Singleton.IsServer) return;

        // Vérifier le nombre maximum de joueurs
        if (NetworkManager.Singleton.ConnectedClients.Count > 2)
        {
            Debug.Log("Nombre maximum de joueurs atteint.");
            NetworkManager.Singleton.DisconnectClient(clientId);
            return;
        }

        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            Debug.Log("Nombre maximum de joueurs atteint. Démarrage du jeu...");
            // Démarrer le jeu lorsque le nombre maximum de joueurs est atteint
            NetworkManager.SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }
    }

    void Update()
    {
        if (!IsServer || finJeuDeclenchee) return;

        // Vérifier si tous les mini-jeux sont terminés
        if (minigame1Completed.Value && minigame2Completed.Value && minigame3Completed.Value)
        {
            finJeuDeclenchee = true;
            StartCoroutine(FinJeu());
        }
    }

    // Appel depuis les portes (côté client)
    public void Minigame1() => LoadMinigameRpc(1);
    public void Minigame2() => LoadMinigameRpc(2);
    public void Minigame3() => LoadMinigameRpc(3);

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void LoadMinigameRpc(int index)
    {
        string nomScene = index switch
        {
            1 => "Minigame1",
            2 => "Minigame2",
            3 => "Minigame3",
            _ => ""
        };

        // Charger la scène pour tous les clients si le nom de la scène est valide
        if (!string.IsNullOrEmpty(nomScene))
        {
            NetworkManager.SceneManager.LoadScene(nomScene, LoadSceneMode.Single);       
        }
    }

    // Appel depuis les mini-jeux (côté client)
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void CompleteMinigameRpc(int indexMinigame)
    {
        switch (indexMinigame)
        {
            case 1:
                minigame1Completed.Value = true;
                break;
            case 2:                   
                minigame2Completed.Value = true;
                break;
            case 3:
                minigame3Completed.Value = true;
                break;
        }
    }

    IEnumerator FinJeu()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Jeu termin� !");
        NetworkManager.SceneManager.LoadScene("SceneFin", LoadSceneMode.Single);
    }
}