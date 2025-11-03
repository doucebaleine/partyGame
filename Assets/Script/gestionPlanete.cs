using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class gestionPlanete : MonoBehaviour
{
    public GameObject spawnPlanete;
    public GameObject terre;

    private bool spawnOK;

    private XRGrabInteractable grabInteractable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spawnOK = false;
        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the grab event
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        StartCoroutine(SpawnPlanete());
        // On instanciate une planète à la position du spawn choisi

    }

    IEnumerator SpawnPlanete()
    {
        // On attend 3 secondes avant de pouvoir en faire apparaître une autre
        yield return new WaitForSeconds(3f);
        Instantiate(terre, spawnPlanete.transform.position, Quaternion.identity);
    }
    private void OnRelease(SelectExitEventArgs args)
    {
        // Call your desired function here when the object is released
        Debug.Log("Object released or thrown!");
        HandleObjectRelease();
    }
    private void HandleObjectRelease()
    {
        // On appeller une coroutine pour faire disparaître la planete
        StartCoroutine(DetruirePlanete());
    }

    IEnumerator DetruirePlanete()
    {
        // On attend 5 secondes avant de détruire la planète
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
