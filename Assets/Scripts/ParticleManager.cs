using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

    public int numParticles;
    public GameObject particlePrefab = null;
    public float spawnRadius = 3.0f;
    public List<GameObject> particles = new List<GameObject>();
    public Vector2 velocity;

    public float maxSpeed = 3.0f;
    public float cohesionWeight;
    public float alignmentWeight;
    public float separationWeight;

    void Start()
    {

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

    

    void Update()
    {
        Vector3 center = new Vector3();

        foreach (GameObject gameObject in particles)
        {
            center += gameObject.transform.position;
            
        }
        center /= particles.Count;
        //Camera.main.transform.position = new Vector3(center.x, center.y, -10.0f);
    }
}
