using UnityEngine;
using UnityEngine.SceneManagement;

public class LancerServeur : MonoBehaviour
{
    public static LancerServeur instance;
    [SerializeField] string NomSceneDepart;

    void Start()
    {
        SceneManager.LoadScene(NomSceneDepart);
    }
}
