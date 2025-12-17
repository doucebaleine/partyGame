using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

public class NetworkConnect : MonoBehaviour
{
    public int maxConnexions = 2;
    public UnityTransport transport;

    // Référence au lobby actuel
    private Lobby lobby;

    async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();  
    }

    // Wrapper for UI buttons: fire-and-forget to satisfy UnityEvent signature.
    public void StartHostButton()
    {
        _ = StartHostAsync();
    }

    // Wrapper for UI buttons: fire-and-forget to satisfy UnityEvent signature.
    public void StartClientButton()
    {
        _ = StartClientAsync();
    }

    private async Task StartHostAsync()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnexions);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, 
            allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        // Créer un lobby pour que le client puisse le rejoindre via le code
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;
        options.Data = new Dictionary<string, DataObject>();
        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, joinCode);
        options.Data.Add("joinCode", dataObject);

        lobby = await LobbyService.Instance.CreateLobbyAsync("Nouveau Lobby", maxConnexions, options);

        NetworkManager.Singleton.StartHost();
    } 

    private async Task StartClientAsync()
    {
        // Rejoindre le lobby existant via le code
        lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
        string relayJoinCode = lobby.Data["joinCode"].Value;

        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

        transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, 
            allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
