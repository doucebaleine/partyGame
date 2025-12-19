using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Netcode;

public class gameManagerGlobal : NetworkBehaviour
{
    public GameObject joueur;

    public NetworkVariable<bool> minigame1Completed = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> minigame2Completed = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> minigame3Completed = new NetworkVariable<bool>(false);

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
       if (minigame1Completed.Value && minigame2Completed.Value && minigame3Completed.Value)
        {
            StartCoroutine(FinJeu());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LancerMinigameServerRpc(string nomScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(nomScene, LoadSceneMode.Single);
    }

    void RecupererScores()
    {
        // Récupérer la liste des joueurs et mettre à jour les scores
        joueurs.Clear();
        joueurs.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        scoresJoueurs.Clear();

        foreach (var joueur in joueurs)
        {
            var scriptJoueur = joueur.GetComponent<gestionJoueur>();
            if (scriptJoueur != null)
            {
                scoresJoueurs.Add(scriptJoueur.score.Value);
            }
        }
    }

    [ClientRpc]
    void AfficherScoresClientRpc()
    {
        string texte = "Scores des joueurs:\n";
        foreach (var joueur in joueurs)
        {
            var scriptJoueur = joueur.GetComponent<gestionJoueur>();
            if (scriptJoueur != null)
            {
                texte += $"Joueur {joueurs.IndexOf(joueur) + 1} : {scriptJoueur.score.Value}\n";
            }
        }
        scoresTexte.text = texte;
    }

    IEnumerator FinJeu()
    {
        yield return new WaitForSeconds(3f);

        RecupererScores();
        AfficherScoresClientRpc();

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
