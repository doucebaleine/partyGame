using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class gestionJoueur : MonoBehaviour
{
    ///SCORE
    public int score = 0;
    public TextMeshProUGUI texteScore;

    /// Minigame3
    public float forceTrampoline = 10f;
    public bool enJeu = false;
    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ajouter10()
    {
        score += 10;
    }

    public void Supprimer10()
    {
        score -= 10;
    }

    public void Ajouter50()
    {
        score += 50;
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
        if (collision.gameObject.tag == "zoneJeu")
        {
            enJeu = true;
        }
    }
}
