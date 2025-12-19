using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Renderer[] meshToDisable;
    public Renderer[] meshesToColor;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // Désactiver les meshes pour le joueur local
        foreach (var item in meshToDisable)
        {
            item.enabled = !IsOwner;
        }
        AjouterCouleurMesh();
    }

    void Update()
    {
        if(!IsOwner || VRRigReferences.Singleton == null) 
            return;
       
        root.position = VRRigReferences.Singleton.root.position;
        root.rotation = VRRigReferences.Singleton.root.rotation;

        head.position = VRRigReferences.Singleton.head.position;
        head.rotation = VRRigReferences.Singleton.head.rotation;

        leftHand.position = VRRigReferences.Singleton.leftHand.position;
        leftHand.rotation = VRRigReferences.Singleton.leftHand.rotation;

        rightHand.position = VRRigReferences.Singleton.rightHand.position;
        rightHand.rotation = VRRigReferences.Singleton.rightHand.rotation;
    }

    void AjouterCouleurMesh()
    {
        // Couleur par défaut
        Color playerColor = Color.white;

        if (OwnerClientId == 0)
            playerColor = new Color(0.6f, 0.2f, 0.8f); // mauve
        else if (OwnerClientId == 1)
            playerColor = Color.yellow;

        foreach (var rend in meshesToColor)
        {
            // Appliquer la couleur au matériau
            rend.material.color = playerColor;
        }
    }
}