using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject prefabMeduse;
    public Transform positionSpawnAlien;
    public float vitesseAlien;

    public List<GameObject> listeSpawnAlien;

    public bool finJeu;
    public bool jeuReussi;
    public TextMeshProUGUI TextePerdant;
    public TextMeshProUGUI TexteGagnant;
    public Button boutonRecommencer;
    public Button boutonQuitter;
    public TextMeshProUGUI TexteTuto;
    public gameManagerGlobal gameManagerGlobal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finJeu = false;
        vitesseAlien = 0.005f;
        // On appelle la coroutine qui permet de faire apparaître des aliens
        StartCoroutine(SpawnAlien());
        StartCoroutine(FinTuto());
    }

    // Update is called once per frame
    void Update()
    {
        if (finJeu)
        {
            
            //On désactive tous les aliens
            GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien");
            foreach (GameObject alien in aliens)
            {
                Destroy(alien);
            }
            //StartCoroutine(JeuAuto());
            if (jeuReussi)
            {
                gameManagerGlobal.minigame1Completed = true;
            }
        }
    }

    IEnumerator SpawnAlien()
    {
        
        while (true)
        {
            // On fait choisir un spawn au hasard
            GameObject spawnChoisi = listeSpawnAlien[Random.Range(0, 3)];

            // On instanciate un alien à cette position
            Instantiate(prefabMeduse, spawnChoisi.transform.position, Quaternion.identity);

            // On augmente la vitesse des aliens toutes les 3 secondes
            vitesseAlien += 0.0005f;
            yield return new WaitForSeconds(2f);
        }
    }
    public void RecommencerJeu()
    {
        //Debug.Log("click");
        SceneManager.LoadScene("Minigame1");
    }
    
    public void QuitterJeu()
    {
        SceneManager.LoadScene("MainGame");
    }

    //IEnumerator JeuAuto()
    //{   // On attend 10 secondes avant de recommencer le jeu
    //    yield return new WaitForSeconds(5f);
    //    SceneManager.LoadScene("Jeu");
    //}

    IEnumerator FinTuto()
    {
        // On attend 10 secondes avant de faire disparaître le tuto
        yield return new WaitForSeconds(5f);
        TexteTuto.gameObject.SetActive(false);
    }
}

