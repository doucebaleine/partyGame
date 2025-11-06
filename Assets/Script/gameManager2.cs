using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject prefabPlateforme;
    public List<Material> listeCouleur;

    public int tailleGrille = 4; // Taille du quadrillage (4x4 par défaut)
    public float espaceEntrePlateformes = 2.0f; // Espacement entre les plateformes
    public float delaiVerification = 5.0f; // Délai avant changement de couleur

    private List<GameObject> plateformes = new List<GameObject>();
    private List<GameObject> televisions = new List<GameObject>();
    private Material couleurActuelleTV;

    public bool finJeu;
    public bool jeuReussi;
    public Button boutonRecommencer;
    public Button boutonQuitter;
    public gameManagerGlobal gameManagerGlobal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenererPlateformes();
        televisions.AddRange(GameObject.FindGameObjectsWithTag("tv"));
        ChangerCouleurs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenererPlateformes()
    {
        // Générer un quadrillage de plateformes
        for (int colonne = 0; colonne < tailleGrille; colonne++)
        {
            for (int ligne = 0; ligne < tailleGrille; ligne++)
            {
                // Calculer la position de la plateforme
                Vector3 positionPlateforme = new Vector3(colonne * espaceEntrePlateformes, 0, ligne * espaceEntrePlateformes);

                // Instancier la plateforme à la position calculée
                GameObject nouvellePlateforme = Instantiate(prefabPlateforme, positionPlateforme, Quaternion.identity);
                plateformes.Add(nouvellePlateforme);

                // Assigner une couleur aléatoire à la plateforme
                Material couleurAleatoire = listeCouleur[Random.Range(0, listeCouleur.Count)];
                nouvellePlateforme.GetComponent<Renderer>().material = couleurAleatoire;
            }
        }
    }

    void ChangerCouleurs()
    {
        // Changer la couleur des télévisions
        couleurActuelleTV = listeCouleur[Random.Range(0, listeCouleur.Count)];
        foreach (GameObject tv in televisions)
        {
            tv.GetComponent<Renderer>().material = couleurActuelleTV;
        }

        foreach (GameObject plateforme in plateformes)
        {
            // Assigner une nouvelle couleur aléatoire aux plateformes
            Material couleurAleatoire = listeCouleur[Random.Range(0, listeCouleur.Count)];
            plateforme.GetComponent<Renderer>().material = couleurAleatoire;
        }
        
        // Vérifier les plateformes et désactiver celles qui ne correspondent pas
        StartCoroutine(VerifierPlateformes());
    }

   IEnumerator VerifierPlateformes()
    {
        yield return new WaitForSeconds(delaiVerification); // Attendre le délai indiqué avant de vérifier

        foreach (GameObject plateforme in plateformes)
        {
            //Debug.Log("Vérification de la plateforme: " + plateforme.name);
            Texture texturePlateforme = plateforme.GetComponent<MeshRenderer>().material.mainTexture;
            //Debug.Log("Texture plateforme: " + texturePlateforme.name);
            if (texturePlateforme != couleurActuelleTV.mainTexture)
            {
                plateforme.SetActive(false); // Désactiver les plateformes de la mauvaise couleur
            }
        }

        yield return new WaitForSeconds(delaiVerification); // Attendre avant de réactiver les plateformes

        foreach (GameObject plateforme in plateformes)
        {
            plateforme.SetActive(true); // Réactiver toutes les plateformes
        }
        ChangerCouleurs(); // Recommencer le processus de changement de couleur
        delaiVerification = Mathf.Max(0.5f, delaiVerification - 0.5f); // Réduire le délai pour augmenter la difficulté
    }
}
