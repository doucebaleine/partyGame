using UnityEngine;
using System.Collections;
using TMPro;

public class ConnexionUI : MonoBehaviour
{
    [Header("Panneaux")]
    [SerializeField] private GameObject panneauConnexion;
    [SerializeField] private GameObject panneauHote;
    [SerializeField] private GameObject panneauClient;

    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TextMeshProUGUI hostCodeText;
    
    public void HostGame()
    {
        panneauHote.SetActive(true);
        panneauConnexion.SetActive(false);
        // Démarrer la connexion en tant qu'hôte
        StartCoroutine(RelayManager.instance.ConfigureTransportAndStartNgoAsHost());
    }

    public void OuvrirPanneauClient()
    {
        panneauClient.SetActive(true);
        panneauConnexion.SetActive(false);
    }

    public void JoinGame()
    {
        // Récupérer le code de connexion entré par l'utilisateur
        RelayManager.instance.RelayJoinCode = joinCodeInputField.text;

        // Démarrer la connexion en tant que joueur connecté
        StartCoroutine(RelayManager.instance.ConfigureTransportAndStartNgoAsConnectingPlayer());
    }
}
