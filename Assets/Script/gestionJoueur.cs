using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Netcode;

public class gestionJoueur : NetworkBehaviour
{
    ///SCORE
    public NetworkVariable<int> score = new (0);
    public TextMeshProUGUI texteScore;

    [Header("UI locale")]
    public GameObject introMinigame;
    public GameObject instructionMinigame1;
    public GameObject instructionMinigame2;
    public GameObject instructionMinigame3;

    /// Minigame3
    public float forceTrampoline = 10f;
    public bool enJeu = false;
    private Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            introMinigame.SetActive(false);
            instructionMinigame1.SetActive(false);
            instructionMinigame2.SetActive(false);
            instructionMinigame3.SetActive(false);
            this.enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (texteScore != null)
        {
            texteScore.text = "Score: " + score.Value;
        }
    }

    [ServerRpc]
    public void AjouterScoreServerRpc(int points)
    {
        score.Value += points;
    }

    public void Ajouter10()
    {
        AjouterScoreServerRpc(10);
    }

    public void Supprimer10()
    {
        AjouterScoreServerRpc(-10);
    }

    public void Ajouter50()
    {
        AjouterScoreServerRpc(50);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(!IsOwner) return;

        if (collision.gameObject.tag == "porte1")
        {
            instructionMinigame1.SetActive(true);
            introMinigame.SetActive(false);
        }
        if (collision.gameObject.tag == "porte2")
        {
            instructionMinigame2.SetActive(true);
            introMinigame.SetActive(false);
        }
        if (collision.gameObject.tag == "porte3")
        {
            instructionMinigame3.SetActive(true);
            introMinigame.SetActive(false);
        }
        if (collision.gameObject.tag == "zoneJeu")
        {
            enJeu = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(!IsOwner) return;

        if (collision.gameObject.tag == "porte1")
        {
            instructionMinigame1.SetActive(false);
            introMinigame.SetActive(true);
        }
        if (collision.gameObject.tag == "porte2")
        {
            instructionMinigame2.SetActive(false);
            introMinigame.SetActive(true);
        }
        if (collision.gameObject.tag == "porte3")
        {
            instructionMinigame3.SetActive(false);
            introMinigame.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "trampoline")
        {
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
            rb.AddForce(Vector3.up * forceTrampoline, ForceMode.VelocityChange);
        }
    }

    void OnColliderTriggerEnter(Collider collision)
    {
        
    }
}