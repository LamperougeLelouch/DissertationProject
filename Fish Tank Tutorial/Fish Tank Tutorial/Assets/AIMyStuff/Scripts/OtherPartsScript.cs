using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPartsScript : MonoBehaviour
{
    FixedJoint[] fixedJoints;
    int otherPartsNum = 0;
    int spineNum = 0;
    int plateNum = 0;
    Rigidbody creatureRigidbody;
    HealthScript healthScript;

    // Use this for initialization
    void Start ()
    {
        healthScript = gameObject.GetComponent<HealthScript>();
        fixedJoints = GetComponentsInChildren<FixedJoint>();
        creatureRigidbody = GetComponent<Rigidbody>();
        IdentifyOtherParts();
    }

    void IdentifyOtherParts()
    {
        for (int i = 0; i < fixedJoints.Length; i++)
        {
            if (fixedJoints[i].connectedBody.tag == "Spine")
            {
                otherPartsNum = otherPartsNum + 1;
                spineNum = spineNum + 1;
            }
            if (fixedJoints[i].connectedBody.tag == "Plate")
            {
                otherPartsNum = otherPartsNum + 1;
                plateNum = plateNum + 1;
            }
        }
    }
     // Here we have four cases: 
     // 1) both creatures do not have plates/spines attached
     // 2) the creature that collided had at least one spine attached and the one that took the collision had no plates
     // 3) the creature that collided had no spines attached and the one that took the collision had at least one plate
     // 4) both creatures have at least one plate/spine attached
              
     // for case 1), if the creature that collided first is a carnivore then the other one if it is a herbivore, it immediately dies
     // for case 2), the result is instant death for the creature that took the collision
     // for case 3), the result is -25 hp to the creature that collided first
     // for case 4), both creatures take some damage, -25 hp for the creature that collided first and the same for the creature that took the collision.
              
     // if hp of creature reaches 0, the creature is dead.
             

    private void OnTriggerEnter(Collider other)
    {
        // save the tags of the creatures in two strings for easier comparison
        // save the creature's attachments that did the collision first in an array
        // use similar method to the first one to identify the joints of the creature that collided
        // get the healthscript of the creature that collided first so that we can increase/decrease/zero the health according to each case

        string collideOne = other.tag;
        string collideTwo = creatureRigidbody.tag;
        FixedJoint[] colliderFixedJoints;
        colliderFixedJoints = other.GetComponentsInChildren<FixedJoint>();
        HealthScript otherHealthScript = other.gameObject.GetComponent<HealthScript>();
        int otherSpineNum = 0;
        int otherPlateNum = 0;
        for (int i = 0; i < colliderFixedJoints.Length; i++)
        {
            if (colliderFixedJoints[i].connectedBody.tag == "Spine")
            {
                otherSpineNum = otherSpineNum + 1;
            }
            if (colliderFixedJoints[i].connectedBody.tag == "Plate")
            {
                otherPlateNum = otherPlateNum + 1;
            }
        }
        // first case: a herbivore gets attacked by a carnivore (this means that the script is attached on a herbivore gameobject)
        if(collideOne == "Carnivore")
        {
            if(collideTwo == "Herbivore")
            {
                if(otherSpineNum == 0 && plateNum == 0)
                {
                    Debug.Log("Carnivore with no spines attacked and killed herbivore with no plates");
                    // kill the herbivore
                    healthScript.setHP(0);
                    // increase carnivore's life by 10
                    otherHealthScript.IncreaseHP(10);
                }
                else if(otherSpineNum != 0 && plateNum == 0)
                {
                    Debug.Log("Carnivore with at least one spine attacked and killed herbivore with no plates");
                    // kill the herbivore
                    healthScript.setHP(0);
                    // increase carnivore's life by 10
                    otherHealthScript.IncreaseHP(10);
                }
                else if (otherSpineNum == 0 && plateNum != 0)
                {
                    Debug.Log("Carnivore with no spines attacked a herbivore with at least one plate");
                    // carnivore's hp reduced by 25
                    otherHealthScript.DecreaseHP(25);
                }
                else if (otherSpineNum !=0 && plateNum != 0)
                {
                    Debug.Log("Carnivore with at least one spine attacked a herbivore with at least one plate");
                    // herbivore's hp reduced by 25
                    healthScript.DecreaseHP(25);
                    // carnivore's hp reduced by 25
                    otherHealthScript.DecreaseHP(25);
                }
            }
        }
        // second case: a carnivore collides with a herbivore (this means that the script is attached on a carnivore gameobject)
        else if(collideOne == "Herbivore")
        {
            if (collideTwo == "Carnivore")
            {
                if (spineNum == 0 && otherPlateNum == 0)
                {
                    Debug.Log("Carnivore with no spines collided and killed herbivore with no plates");
                    // kill the herbivore
                    otherHealthScript.setHP(0);
                    // increase carnivore's life by 10
                    healthScript.IncreaseHP(10);
                }
                else if (spineNum != 0 && otherPlateNum == 0)
                {
                    Debug.Log("Carnivore with at least one spine collided and killed herbivore with no plates");
                    // kill the herbivore
                    otherHealthScript.setHP(0);
                    // increase carnivore's life by 10
                    healthScript.IncreaseHP(10);
                }
                else if (spineNum == 0 && otherPlateNum != 0)
                {
                    Debug.Log("Carnivore with no spines collided a herbivore with at least one plate");
                    // carnivore's hp reduced by 25
                    healthScript.DecreaseHP(25);
                }
                else if (spineNum != 0 && otherPlateNum != 0)
                {
                    Debug.Log("Carnivore with at least one spine collided a herbivore with at least one plate");
                    // herbivore's hp reduced by 25
                    otherHealthScript.DecreaseHP(25);
                    // carnivore's hp reduced by 25
                    healthScript.DecreaseHP(25);
                }
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        
	}
}
