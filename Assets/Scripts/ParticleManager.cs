using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

    //how many particles should spawn
    public int numParticles;

    //what particle it should spawn
    public GameObject particlePrefab = null;

    public float spawnRadius = 3.0f;

    //list that stores all of the spawned particles
    public List<GameObject> particles = new List<GameObject>();

    public Vector2 velocity;

    //speed and weights of the vectors
    public float maxSpeed = 3.0f;
    public float cohesionWeight;
    public float alignmentWeight;
    public float separationWeight;

    void Start()
    {
        //spawns particles according to how many you want and witch particle
        for (int i = 0; i < numParticles; i++)
        {
            Vector2 position = spawnRadius * Random.insideUnitCircle;
            velocity = Random.insideUnitCircle;

            GameObject gameObject = Instantiate(particlePrefab, position, Quaternion.identity) as GameObject;
            Particle particle = gameObject.GetComponent<Particle>();
            particle.velocity = velocity;

            particles.Add(gameObject);
        }
    }

    //gets the center of the camera in world space
    void Update()
    {
        Vector3 center = new Vector3();

        foreach (GameObject gameObject in particles)
        {
            center += gameObject.transform.position;
            
        }
        center /= particles.Count;
    }
}
