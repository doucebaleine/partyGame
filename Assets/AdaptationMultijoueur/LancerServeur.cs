using UnityEngine;
using UnityEngine.SceneManagement;

public class LancerServeur : MonoBehaviour
{
    public static LancerServeur instance;
    [SerializeField] string NomSceneDepart;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.LoadScene(NomSceneDepart);
    }
}
