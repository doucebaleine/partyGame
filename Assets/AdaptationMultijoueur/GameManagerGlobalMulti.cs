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

    void Start()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += (ulong clientId) =>
        {
           if(NetworkManager.Singleton.ConnectedClients.Count == 2)
            {
                NetworkManager.SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
            }
        };
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
    public void Minigame1() => LoadMinigameServerRpc(1);
    public void Minigame2() => LoadMinigameServerRpc(2);
    public void Minigame3() => LoadMinigameServerRpc(3);

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void LoadMinigameServerRpc(int index)
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
    public void CompleteMinigameServerRpc(int indexMinigame)
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