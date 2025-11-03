using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gestionTemps : MonoBehaviour
{

    // Ce code ne m'aappartient pas, je l'ai pris à partir de cette vidéo : https://www.youtube.com/watch?v=hxpUk0qiRGs

    public float TempsRestant;
    public bool TempsMarche = false;


    public TextMeshProUGUI Temps;
    private GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
        TempsMarche = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TempsMarche)
        {
            if (TempsRestant>0)
            {
                TempsRestant -= Time.deltaTime;
                mettreAJour(TempsRestant);
            }
            else
            {
                // Scene fin
                gameManagerScript.finJeu = true;
                gameManagerScript.TexteGagnant.gameObject.SetActive(true);
                gameManagerScript.boutonRecommencer.gameObject.SetActive(true);
                gameManagerScript.boutonQuitter.gameObject.SetActive(true);
                TempsRestant = 0;
                TempsMarche = false;
            }
        }

        if (gameManagerScript.finJeu)
        {
            TempsMarche = false;
            Temps.gameObject.SetActive(false);
        }
    }

    void mettreAJour(float tempsPresent)
    {
        tempsPresent += 1;

        float minutes = Mathf.FloorToInt(tempsPresent / 60);
        float secondes = Mathf.FloorToInt(tempsPresent % 60);

        Temps.text = string.Format("{0:00} : {1:00}", minutes, secondes);
    }

    

}


