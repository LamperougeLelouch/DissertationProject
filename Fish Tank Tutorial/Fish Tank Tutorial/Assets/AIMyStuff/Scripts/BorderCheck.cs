using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCheck : MonoBehaviour
{
    void OnTriggerExit(Collider collider)
    {
        Debug.Log("Creature at the edge of the tank. Destroy immediately. ");
        Destroy(collider.gameObject);
    }
}
