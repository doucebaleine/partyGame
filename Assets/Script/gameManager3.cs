using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager3 : MonoBehaviour
{

    public List<GameObject> joueurs = new List<GameObject>();
    public gestionJoueur scriptJoueur;
    public GameObject spawnTerre1;
    public GameObject spawnTerre2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joueurs.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        scriptJoueur = joueurs[0].GetComponent<gestionJoueur>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scriptJoueur.finCourse)
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}
