using UnityEngine;

public class XRRigPersistant : MonoBehaviour
{
    static XRRigPersistant instance;

    void Awake()
    {
        // Vérifie s'il existe déjà un XR Rig persistant
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Évite les duplications
            return;
        }

        instance = this;

        // Rend ce XR Rig persistant entre les scènes
        DontDestroyOnLoad(gameObject);
    }
}