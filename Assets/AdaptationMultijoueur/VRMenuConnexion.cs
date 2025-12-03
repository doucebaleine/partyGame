using UnityEngine;

using System.Threading.Tasks;

public class VRMenuConnexion : MonoBehaviour
{
    [SerializeField] private bool isHost = true; // Définir sur true si bouton "Créer Partie", false si "Rejoindre Partie"

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    // Appel depuis le bouton VR
    public void StartHostButton() => StartHostAsync();
    public void StartClientButton() => StartClientAsync();


    private async Task StartHostAsync()
    {
        await RelayManager.Instance.StartHostAsync();
        Debug.Log("Host prêt. En attente du client...");
        // Pas de LoadScene ici ! GameManagerGlobalMulti s'en occupera.
    }

    private async Task StartClientAsync()
    {
        string code = RelayManager.Instance.JoinCode;

        if (string.IsNullOrEmpty(code))
        {
            Debug.LogWarning("Join code non disponible. Attendez que le host crée la partie.");
            return;
        }

        await RelayManager.Instance.StartClientAsync(code);
        Debug.Log("Client connecté automatiquement au host.");
    }
}