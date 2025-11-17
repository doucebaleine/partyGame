using UnityEngine;

public class CollisionPorte : MonoBehaviour
{
    public int indexMinigame; // 1, 2 ou 3

    void onTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Appel au GameManager pour charger le mini-jeu
        GameManagerGlobalMulti.instance.LoadMinigameServerRpc(indexMinigame);
    }
}
