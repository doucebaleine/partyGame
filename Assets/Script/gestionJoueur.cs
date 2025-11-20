using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class gestionJoueur : MonoBehaviour
{
    ///SCORE
    public int score = 0;
    public TextMeshProUGUI texteScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
