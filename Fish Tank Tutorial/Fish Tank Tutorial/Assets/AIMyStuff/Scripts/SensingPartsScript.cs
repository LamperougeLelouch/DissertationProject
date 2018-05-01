using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingPartsScript : MonoBehaviour
{
    // This script is responsible for handling the actions of all sensing parts. 
    // First of all, the sensing parts will be recognized by their tag. After that, methods implemented for all sensing parts
    // will be executed, based on what is attached on the creature.
    // Simple eye -> vision/no vision (I see a creature/ I don't see anything).
    // Composite eye -> vision/no vision, distance (I see a creature X meters away./ I don't see anything).
    // Complex eye -> vision/no vision, distance, size (I see a creature X meters away of Y size./ I don't see anything).
    // Ears -> direction, distance (I hear a creature from X position Y meters away).

    // Declaration of variables.

    FixedJoint[] fixedJoints;
    int sensingPartsNum = 0;
    int simpleEyeNum = 0;
    int compositeEyeNum = 0;
    int complexEyeNum = 0;
    int earsNum = 0;
    Rigidbody creatureRigidbody;

	// Use this for initialization
	void Start ()
    {
        fixedJoints = GetComponentsInChildren<FixedJoint>();
        creatureRigidbody = GetComponent<Rigidbody>();
        IdentifySensingParts();
	}
	
    void IdentifySensingParts()
    {
        for(int i=0; i<fixedJoints.Length; i++)
        {
            if(fixedJoints[i].connectedBody.tag == "Simple Eye")
            {
                sensingPartsNum = sensingPartsNum + 1;
                simpleEyeNum = simpleEyeNum + 1;
            }
            if (fixedJoints[i].connectedBody.tag == "Composite Eye")
            {
                sensingPartsNum = sensingPartsNum + 1;
                compositeEyeNum = compositeEyeNum + 1;
            }
            if (fixedJoints[i].connectedBody.tag == "Complex Eye")
            {
                sensingPartsNum = sensingPartsNum + 1;
                complexEyeNum = complexEyeNum + 1;
            }
            if (fixedJoints[i].connectedBody.tag == "Ears")
            {
                sensingPartsNum = sensingPartsNum + 1;
                earsNum = earsNum + 1;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {

        // You want to calculate the direction between the enemy and the player, get the forward vector of the enemy, and then use Vector3.Angle to calculate the angle.
        // Then you can check if the angle is less than whatever angle you want to be the enemy's field of vision 
        // (eg: a 20 degree cone, for instance), and if it is, then the player is in that enemy's field of vision.
        // Raycasts should ignore attached parts of the creature.

        // Debugging Ray for sight
        Debug.DrawRay(transform.position, transform.forward.normalized * 10, Color.green);

        // Debugging Rays for ears
        Debug.DrawRay(transform.position, transform.right.normalized * 5, Color.green);
        Debug.DrawRay(transform.position, - transform.right.normalized * 5, Color.green);

        RaycastHit hit;

        // Eyes checking the environment, debugging to see that it works
        if(Physics.Raycast(transform.position,transform.forward * 10, out hit))
        {
            if(simpleEyeNum != 0)
            {
                //Debug.Log("Simple Eye sees " + hit.collider.tag + ".");
                if (creatureRigidbody.tag == "Herbivore Body Part" && hit.collider.tag == "Carnivore Body Part")
                {
                    //Debug.Log("Herbivore faced a danger. Change direction immediately.");
                }
                else if(creatureRigidbody.tag == "Carnivore Body Part" && hit.collider.tag == "Herbivore Body Part")
                {
                    Debug.Log("Carnivore found food. Moving towards it.");
                    // need to access the m_speed variable from the AIMove script for the maxDistanceDelta variable
                    // maxDistanceDelta = m_speed * Time.deltatime
                    transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, 7*Time.deltaTime );
                }
            }
            if(compositeEyeNum != 0)
            {
                //Debug.Log("Composite Eye sees " + hit.collider.tag + " " + hit.distance + " meters away.");
                if (creatureRigidbody.tag == "Herbivore Body Part" && hit.collider.tag == "Carnivore Body Part")
                {
                    //Debug.Log("Herbivore faced a danger. Change direction immediately.");
                }
                else if (creatureRigidbody.tag == "Carnivore Body Part" && hit.collider.tag == "Herbivore Body Part")
                {
                    Debug.Log("Carnivore found food. Moving towards it.");
                    transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, 7 * Time.deltaTime);
                }
            }
            if(complexEyeNum != 0)
            {
                //Debug.Log("Complex Eye sees " + hit.collider.tag + " " + hit.distance + " meters away, of size " + hit.collider.bounds.size + ".");
                if (creatureRigidbody.tag == "Herbivore Body Part" && hit.collider.tag == "Carnivore Body Part")
                {
                    //Debug.Log("Herbivore faced a danger. Change direction immediately.");
                }
                else if (creatureRigidbody.tag == "Carnivore Body Part" && hit.collider.tag == "Herbivore Body Part")
                {
                    Debug.Log("Carnivore found food. Moving towards it.");
                    transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, 7 * Time.deltaTime);
                }
            }
        }
        else
        {
            // Ears checking the environment
            if (Physics.Raycast(transform.position, transform.right.normalized * 5, out hit))
            {
                if (earsNum != 0)
                {
                    //Debug.Log("Ears listen a creature from " + hit.collider.transform.position + " and is " + hit.distance + " meters away.");
                    if (creatureRigidbody.tag == "Herbivore Body Part" && hit.collider.tag == "Carnivore Body Part")
                    {
                        Debug.Log("Herbivore heard a danger from " + hit.collider.transform.position + " and is " + hit.distance + " meters away.");
                    }
                    else if (creatureRigidbody.tag == "Carnivore Body Part" && hit.collider.tag == "Herbivore Body Part")
                    {
                        Debug.Log("Carnivore heard from " + hit.collider.transform.position + " and is " + hit.distance + " meters away.");
                        transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, 7 * Time.deltaTime);
                    }
                }
            }
            else if (Physics.Raycast(transform.position, - transform.right.normalized * 5, out hit))
            {
                if (earsNum != 0)
                {
                    //Debug.Log("Ears listen a creature from " + hit.collider.transform.position + " and is " + hit.distance + " meters away.");
                    if (creatureRigidbody.tag == "Herbivore Body Part" && hit.collider.tag == "Carnivore Body Part")
                    {
                        Debug.Log("Herbivore heard a danger from " + hit.collider.transform.position + " and is " + hit.distance + " meters away.");
                    }
                    else if (creatureRigidbody.tag == "Carnivore Body Part" && hit.collider.tag == "Herbivore Body Part")
                    {
                        Debug.Log("Carnivore heard from " + hit.collider.transform.position + " and is " + hit.distance + " meters away.");
                        transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, 7 * Time.deltaTime);
                    }
                }
            }
        }            
    }
}
