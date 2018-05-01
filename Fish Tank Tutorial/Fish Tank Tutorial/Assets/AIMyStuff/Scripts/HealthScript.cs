using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    // this script is responsible for increasing/decreasing the life of a creature

    int health = 100;
    GameObject creature;

    // Use this for initialization
    void Start ()
    {
        creature = GetComponent<GameObject>();
	}

    public void IncreaseHP(int increase)
    {
        health = health + increase;
    }

    public void DecreaseHP(int decrease)
    {
        health = health - decrease;
    }

    public void setHP(int HP)
    {
        health = HP;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (health == 0)
        {
            Debug.Log("Creature has zero hitpoints.");
            Destroy(creature);
        }
    }
}
