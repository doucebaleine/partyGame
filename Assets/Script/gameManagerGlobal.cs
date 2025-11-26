using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;


public class gameManagerGlobal : MonoBehaviour
{
    public GameObject joueur;

    

    public static bool minigame1Completed = false;
    public static bool minigame2Completed = false;
    public static bool minigame3Completed = false;

    private List<GameObject> joueurs = new List<GameObject>();
    public List<int> scoresJoueurs = new List<int>();

    public TextMeshProUGUI scoresTexte;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scoresTexte.text = "Scores des joueurs:\n";
        //joueurs.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        //foreach(GameObject joueur in joueurs)
        //{
        //    gestionJoueur scriptJoueur = joueur.GetComponent<gestionJoueur>();
        //    scoresJoueurs.Add(scriptJoueur.score);
        //    scoresTexte.text += "Joueur " + (scoresJoueurs.Count) + " : " + scriptJoueur.score + "\n";
        //}
    }

    // Update is called once per frame
    void Update()
    {
       if (minigame1Completed && minigame2Completed && minigame3Completed)
        {
            StartCoroutine(FinJeu());
        }
    }

    public void Minigame1()
    {
        SceneManager.LoadScene("Minigame1");
    }
    public void Minigame2()
    {
        SceneManager.LoadScene("Minigame2");
    }

    public void Minigame3()
    {
        SceneManager.LoadScene("Minigame3");
    }

    IEnumerator FinJeu()
    {
        yield return new WaitForSeconds(3f);

        ///On calcule le score total des joueurs
        foreach (GameObject joueur in joueurs)
        {
            gestionJoueur scriptJoueur = joueur.GetComponent<gestionJoueur>();
            if (scriptJoueur != null)
            {
                //Debug.Log("Score final du joueur: " + scriptJoueur.score);
            }
        }
    }
}
