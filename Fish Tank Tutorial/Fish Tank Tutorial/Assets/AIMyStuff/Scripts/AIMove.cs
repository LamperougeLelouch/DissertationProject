using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    // declare variable for AISpawner script
    private AISpawner m_AIManager;

    // declare variables for moving and turning
    private bool m_hasTarget = false;
    private bool m_isTurning;

    // variable for current waypoint
    private Vector3 m_wayPoint;
    private Vector3 m_lastWaypoint = new Vector3(0f, 0f, 0f);

    // going to use this to set the animation speed
    //private Animator m_animator;
    private float m_speed;

    private Collider m_collider;

    // these variables will be used to understand what kinds of moving parts the creature has attached on it.
    // tails increase speed whereas fins increase angular speed. 
    // but modifying the creature's velocity and angular velocity directly results in unrealistic behavior 
    // angular drag will be used. High angular drag -> harder to turn
    // according to the number of fins the creature will receive an angular drag smaller than before 

    //Rigidbody creatureRigidbody;
    FixedJoint[] fixedJoints;
    int tailNum = 0;
    int finNum = 0;

	// Use this for initialization
	void Start ()
    {
        m_AIManager = transform.parent.GetComponentInParent<AISpawner>();
        //m_animator = GetComponent<Animator>();

        SetUpNPC();
        fixedJoints = GetComponentsInChildren<FixedJoint>();
        //creatureRigidbody = GetComponent<Rigidbody>();
        IdentifyMovingParts();
    }

    void SetUpNPC()
    {
        if(transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().enabled == true)
        {
            m_collider = transform.GetComponent<Collider>();
        }
        else if(transform.GetComponentInChildren<Collider>() != null && transform.GetComponentInChildren<Collider>().enabled == true)
        {
            m_collider = transform.GetComponentInChildren<Collider>();
        }
    }

    void IdentifyMovingParts()
    {
        for(int i=0; i<fixedJoints.Length; i++)
        {
            if(fixedJoints[i].connectedBody.tag == "Tail")
            {
                tailNum = tailNum + 1;
            }
            if(fixedJoints[i].connectedBody.tag == "Fin")
            {
                finNum = finNum + 1;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // if we have not found a waypoint to move to
        // if we found a waypoint we need to move there
        if(!m_hasTarget)
        {
            m_hasTarget = CanFindTarget();
        }
        else
        {
            // make sure we rotate the NPC to face its waypoint
            RotateNPC(m_wayPoint, m_speed);
            // move the NPC in a straight line toward the waypoint
            transform.position = Vector3.MoveTowards(transform.position, m_wayPoint, m_speed * Time.deltaTime);

            // check if collided - if yes then lose the target and look for new waypoint
            CollidedNPC();
        }
        
        // if NPC reaches waypoint reset target
        if(transform.position == m_wayPoint)
        {
            m_hasTarget = false;
        }
    }

    // method for changing direction if NPC collides with something
    void CollidedNPC()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, transform.localScale.z))
        {
            //if collider has hit a waypoint or registers itself ignore raycasthit
            if(hit.collider == m_collider | hit.collider.tag == "waypoint")
            {
                return;
            }
            // otherwise have a random chance that NPC will change direction
            int randomNum = Random.Range(1, 100);
            if (randomNum < 40)
                m_hasTarget = false;

        }
    }

    // get the waypoint
    Vector3 GetWaypoint(bool isRandom)
    {
        // if isRandom is true then get a random position location
        if(isRandom)
        {
            return m_AIManager.RandomPosition();
        }
        // otherwise ge a random waypoint from the list of waypoint gameobjects
        else
        {
            return m_AIManager.RandomWaypoint();
        }
    }

    bool CanFindTarget(float start = 1f, float end = 7f)
    {
        m_wayPoint = m_AIManager.RandomWaypoint();
        // make sure we dont set the same waypoint twice
        if(m_lastWaypoint == m_wayPoint)
        {
            // get a new waypoint
            m_wayPoint = GetWaypoint(true);
            return false;
        }
        else
        {
            // set the new waypoint as the last waypoint
            m_lastWaypoint = m_wayPoint;
            // get random speed for movement and animation
            m_speed = Random.Range(start, end);
            if(tailNum != 0)
            {
                //Debug.Log("Tail detected. Speed increased.");
                m_speed = m_speed * tailNum * 2;
            }
            //m_animator.speed = m_speed;
            // set bool to true to say we found a waypoint
            return true;
        }
    }

    // rotate the NPC to face new waypoint
    void RotateNPC(Vector3 waypoint, float currentSpeed)
    {
        // get random speed up for the turn
        float TurnSpeed = currentSpeed * Random.Range(1f, 3f);
        if(finNum !=0)
        {
            //Debug.Log("Fin detected. Rotation Speed increased.");
            TurnSpeed = TurnSpeed * finNum * 2;
        }

        // get new direction to look at for target
        Vector3 LookAt = waypoint - this.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), TurnSpeed * Time.deltaTime);
    }
}
