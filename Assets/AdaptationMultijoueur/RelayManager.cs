using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class RelayManager : MonoBehaviour
{
    public static RelayManager instance;

    const int MaxConnections = 1; // L'hôte compte comme 1
    public string RelayJoinCode; // Code de connexion relay

    private Allocation allocation; // Allocation pour l'hôte
    private JoinAllocation joinAllocation; // Allocation pour le client

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI joinCodeText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        AuthenticatePlayer();
    }

    /// <summary>
    /// Authentifie le joueur auprès de Unity Services
    /// </summary>
    private async void AuthenticatePlayer()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Player authenticated: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Crée un serveur Relay et récupère un code de connexion
    /// </summary>
    public async Task<string> AllocateRelayServerAndGetJoinCode(int maxConnections = MaxConnections, string region = null)
    {
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay allocation failed: {e.Message}");
            throw;
        }

        try
        {
            RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            joinCodeText.text = RelayJoinCode; // affichage dans l'UI
            Debug.Log($"Relay join code: {RelayJoinCode}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to get join code: {e.Message}");
            throw;
        }

        return RelayJoinCode;
    }

    /// <summary>
    /// Configure UnityTransport et démarre le Netcode en tant qu'hôte
    /// </summary>
    public IEnumerator ConfigureTransportAndStartNgoAsHost()
    {
        var task = AllocateRelayServerAndGetJoinCode(MaxConnections);
        while (!task.IsCompleted) yield return null;

        if (task.IsFaulted)
        {
            Debug.LogError("Failed to start relay host: " + task.Exception?.Message);
            yield break;
        }

        // Configure le transport Relay
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        NetworkManager.Singleton.NetworkConfig.NetworkTransport = unityTransport;
        unityTransport.SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));
        NetworkManager.Singleton.StartHost();
        yield return null;
    }

    /// <summary>
    /// Rejoindre un serveur Relay existant via le code
    /// </summary>
    public async Task<JoinAllocation> JoinRelayServerFromJoinCode(string joinCode)
    {
        try
        {
            joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to join relay server: {e.Message}");
            throw;
        }

        Debug.Log($"Client connected to host: {joinAllocation.HostConnectionData[0]}");
        return joinAllocation;
    }

    /// <summary>
    /// Configure UnityTransport et démarre le Netcode en tant que client
    /// </summary>
    public IEnumerator ConfigureTransportAndStartNgoAsConnectingPlayer()
    {
        if (string.IsNullOrEmpty(RelayJoinCode))
        {
            Debug.LogError("No relay join code available!");
            yield break;
        }

        var task = JoinRelayServerFromJoinCode(RelayJoinCode);
        while (!task.IsCompleted) yield return null;

        if (task.IsFaulted)
        {
            Debug.LogError("Failed to connect to relay server: " + task.Exception?.Message);
            yield break;
        }

        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        NetworkManager.Singleton.NetworkConfig.NetworkTransport = unityTransport;
        unityTransport.SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "dtls"));
        NetworkManager.Singleton.StartClient();
        
        yield return null;
    }
}