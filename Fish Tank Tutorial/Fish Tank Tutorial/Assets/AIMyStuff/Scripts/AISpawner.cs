using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AIObjects
{
    public string AIGroupName { get { return m_AIGroupName; } }
    public GameObject objectPrefab { get { return m_prefab; } }
    public int maxAI { get { return m_maxAI; } }
    public int spawnRate { get { return m_spawnRate; } }
    public int spawnAmount { get { return m_maxSpawnAmount; } }
    public bool randomizeStats { get { return m_randomizeStats; } }
    public bool enableSpawner { get { return m_enableSpawner; } }

    [Header("AI Group Stats")]
    [SerializeField]
    private string m_AIGroupName;
    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    [Range(0f, 40f)]
    private int m_maxAI;
    [SerializeField]
    [Range(0f, 20f)]
    private int m_spawnRate;
    [SerializeField]
    [Range(0f, 10f)]
    private int m_maxSpawnAmount;

    [Header("Main Settings")]
    [SerializeField]
    private bool m_randomizeStats;
    [SerializeField]
    private bool m_enableSpawner;

    public AIObjects(string Name, GameObject Prefab, int MaxAI, int SpawnRate, int SpawnAmount, bool RandomizeStats)
    {
        this.m_AIGroupName = Name;
        this.m_prefab = Prefab;
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
        this.m_randomizeStats = RandomizeStats;
    }

}

public class AISpawner : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();

    public float spawnTimer { get { return m_SpawnTimer; } } // global value - how often we run the spawner
    public Vector3 spawnArea { get { return m_SpawnArea; } }

    [Header("Global Stats")]
    [Range(0f, 600f)]
    [SerializeField]
    private float m_SpawnTimer;
    [SerializeField]
    private Color m_SpawnColor = new Color(1.000f, 0.000f, 0.000f, 0.300f); // gizmo color - red
    [SerializeField]
    private Vector3 m_SpawnArea = new Vector3(20f, 10f, 20f);


    [Header("AI Group Settings")]
    public AIObjects[] AIObject = new AIObjects[5];

    // Use this for initialization
    void Start ()
    {
        GetWaypoints();
        RandomizeGroups();
        CreateAIGroups();
        InvokeRepeating("SpawnNPC", 0.5f, spawnTimer);
        hideWaypoints();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Method for hiding the waypoints.
    void hideWaypoints()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            Waypoints[i].GetComponent<Renderer>().enabled = false;
        }
    }

    void SpawnNPC()
    {
        // loop through all AI groups
        for(int i=0; i < AIObject.Count(); i++)
        {
            // check to make sure spawner is enabled
            if(AIObject[i].enableSpawner && AIObject[i].objectPrefab != null)
            {
                // make sure that the group does not have max NPCs
                GameObject tempGroup = GameObject.Find(AIObject[i].AIGroupName);
                if(tempGroup.GetComponentInChildren<Transform>().childCount < AIObject[i].maxAI)
                {
                    // spawn random number of NPCs from 0 to Max Spawn Amount
                    for(int y = 0; y < Random.Range(0,AIObject[i].spawnAmount); y++)
                    {
                        // get random rotation
                        Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
                        // create spawned gameobject
                        GameObject tempSpawn;
                        tempSpawn = Instantiate(AIObject[i].objectPrefab, RandomPosition(), randomRotation);
                        // put spawned NPC as child of group
                        tempSpawn.transform.parent = tempGroup.transform;
                        // add the AIMove script and class to the new NPC
                        tempSpawn.AddComponent<AIMove>();
                    }

                }
            }
        }
    }

    // public method for random position within the Spawn Area
    public Vector3 RandomPosition()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z));
        randomPosition = transform.TransformPoint(randomPosition * .5f);
        return randomPosition;
    }

    // public method to return a random waypoint
    public Vector3 RandomWaypoint()
    {
        int randomWP = Random.Range(0, (Waypoints.Count - 1));
        Vector3 randomWaypoint = Waypoints[randomWP].transform.position;
        return randomWaypoint;
    }

    // Method for putting random values in AI groups.
    void RandomizeGroups()
    {
        for(int i = 0; i < AIObject.Count(); i++)
        {
            if(AIObject[i].randomizeStats)
            {
                AIObject[i] = new AIObjects(AIObject[i].AIGroupName, AIObject[i].objectPrefab, Random.Range(1, 30), Random.Range(1, 20), Random.Range(1, 10), AIObject[i].randomizeStats);
            }
        }
    }

    // Method for creating the empty worldobject groups
    void CreateAIGroups()
    {

        for(int i=0; i < AIObject.Count(); i++)
        {
            // Empty GameObject to keep our AI in.
            GameObject m_AIGroupSpawn;

            // create a new game object
            m_AIGroupSpawn = new GameObject(AIObject[i].AIGroupName);
            m_AIGroupSpawn.transform.parent = this.gameObject.transform;
        }
    }

    void GetWaypoints()
    {
        Transform[] wpList = transform.GetComponentsInChildren<Transform>();
        for(int i=0; i < wpList.Length; i++)
        {
            if(wpList[i].tag == "waypoint")
            {
                Waypoints.Add(wpList[i]);
            }
        }
    }

    // show the gizmos in color
    void OnDrawGizmosSelected()
    {
        Gizmos.color = m_SpawnColor;
        Gizmos.DrawCube(transform.position, spawnArea);
    }
}
