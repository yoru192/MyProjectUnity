using System;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{

    public Transform player;
    private NavMeshAgent _agent;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.destination = player.position;
    }
}
