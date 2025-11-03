using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject prefabTV;
    public GameObject prefabPlateforme;
    public List<Material> listeCouleur;

    public int tailleGrille = 4; // Taille du quadrillage (4x4 par défaut)
    public float espaceEntrePlateformes = 2.0f; // Espacement entre les plateformes
    public float delaiChangementCouleur = 5.0f; // Délai avant changement de couleur

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
        GenererTelevisions();
        InvokeRepeating(nameof(ChangerCouleurs), delaiChangementCouleur, delaiChangementCouleur);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenererPlateformes()
    {
        // Générer un quadrillage de plateformes
        for (int x = 0; x < tailleGrille; x++)
        {
            for (int z = 0; z < tailleGrille; z++)
            {
                Vector3 position = new Vector3(x * espaceEntrePlateformes, 0, z * espaceEntrePlateformes);
                GameObject plateforme = Instantiate(prefabPlateforme, position, Quaternion.identity);
                plateformes.Add(plateforme);

                // Assigner une couleur aléatoire à chaque plateforme
                Material couleurAleatoire = listeCouleur[Random.Range(0, listeCouleur.Count)];
                plateforme.GetComponent<Renderer>().material = couleurAleatoire;
            }
        }
    }

    void GenererTelevisions()
    {
        // Générer 4 télévisions autour de l'espace de jeu
        float offset = tailleGrille * espaceEntrePlateformes / 2;
        Vector3[] positionsTV = {
            new Vector3(-offset, 2, 0), // Gauche
            new Vector3(offset, 2, 0),  // Droite
            new Vector3(0, 2, -offset), // Bas
            new Vector3(0, 2, offset)   // Haut
        };

        foreach (Vector3 position in positionsTV)
        {
            GameObject tv = Instantiate(prefabTV, position, Quaternion.identity);
            televisions.Add(tv);
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

        // Vérifier les plateformes et désactiver celles qui ne correspondent pas
        StartCoroutine(VerifierPlateformes());
    }

    System.Collections.IEnumerator VerifierPlateformes()
    {
        yield return new WaitForSeconds(5.0f); // Attendre 5 secondes avant de vérifier

        foreach (GameObject plateforme in plateformes)
        {
            Material couleurPlateforme = plateforme.GetComponent<Renderer>().material;
            if (couleurPlateforme != couleurActuelleTV)
            {
                plateforme.SetActive(false); // Désactiver les plateformes de la mauvaise couleur
            }
        }

        yield return new WaitForSeconds(5.0f); // Attendre avant de réactiver les plateformes

        foreach (GameObject plateforme in plateformes)
        {
            plateforme.SetActive(true); // Réactiver toutes les plateformes
        }
    }
}
