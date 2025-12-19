using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scriptAlien : MonoBehaviour
{
    public GameObject joueur;

    //Variable de déplacement
    public Vector3 distancePerso;

    public GameObject alien;

    public static GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log(scriptGameManager.valeurTransition);
        //scriptGameManager.valeurTransition =+ 0.05f;
        joueur = GameObject.FindGameObjectWithTag("Player");

        GameObject managerObject = GameObject.Find("GameManager");
        if (managerObject != null)
        {
            // Get the gameManager component attached to the GameManager GameObject
            gameManager = managerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("pas de gamemanager");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 nouvellePos = joueur.transform.TransformPoint(0, 0, 0);
        transform.position = Vector3.Lerp(transform.position, nouvellePos, gameManager.vitesseAlien);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.tag == "planete")
        {
            //Debug.Log("planete collision");
            Destroy(gameObject);
        } 
        else if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("player collision");
            // tu as perdu!! 
            ////////////// change une variable du script de gameManager
            SceneManager.LoadScene("MainGame");
        }

    }
}
