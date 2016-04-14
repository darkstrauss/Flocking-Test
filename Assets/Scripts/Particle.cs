using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Particle : MonoBehaviour {

    //direction in witch the particle will move
    public Vector3 velocity = new Vector3();

    //nearby neighbours
    public List<GameObject> nearParticles;

    private ParticleManager particleManager;

    private Camera cam = Camera.main;
    private Vector3 viewPortPostition;
    private Vector3 newPosition;
    private Renderer rendererComponent;

    bool isWrappingX = false;
    bool isWrappingY = false;

    void Start()
    {
        particleManager = Camera.main.GetComponent<ParticleManager>();
        rendererComponent = GetComponent<Renderer>();
    }

    void Update()
    {
        //vectors that will influence the direction of the particle
        Vector3 cohesion = new Vector3();
        Vector3 alignment = new Vector3();
        Vector3 separation = new Vector3();

        ScreenWrap();

        //adds the cohesion, alignment, and separeation vectors to the particle
        foreach (GameObject gO in nearParticles)
        {
            cohesion += gO.transform.position;
            alignment += gO.GetComponent<Particle>().velocity;
            separation += transform.position - gO.transform.position;
        }

        //checks the nearby particles and divides the cohesion and alignment vectors by the amount of nearby neignbours
        if (nearParticles.Count > 0)
        {
            cohesion = cohesion / nearParticles.Count;
            alignment = alignment / nearParticles.Count;
        }

        cohesion = cohesion - transform.position;

        velocity = velocity + particleManager.cohesionWeight * cohesion;
        velocity = velocity + particleManager.alignmentWeight * alignment;
        velocity = velocity + particleManager.separationWeight * separation;
        velocity = particleManager.maxSpeed * velocity.normalized;

        transform.Translate(velocity * Time.deltaTime, Space.World);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90.0f);

        
    }

    //this wraps the particles around the screen if they travel too far
    void ScreenWrap()
    {
        bool isVisible = CheckRenderer();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
        }

        if (isWrappingX || isWrappingY)
        {
            return;
        }

        //gets the world position of the particle relative to the main camera
        viewPortPostition = cam.WorldToViewportPoint(transform.position);

        newPosition = transform.position;

        //sends the particle to the opposite screen side
        if (viewPortPostition.x > 1 || viewPortPostition.x < 0)
        {
            newPosition.x = -newPosition.x * 0.9f;
            isWrappingX = true;
        }
        if (viewPortPostition.y > 1 || viewPortPostition.y < 0)
        {
            newPosition.y = -newPosition.y * 0.9f;
            isWrappingY = true;
        }

        transform.position = newPosition;

    }

    //checks if the given particle in still within the viewport
    bool CheckRenderer()
    {
        if (rendererComponent.isVisible)
        {
            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        nearParticles.Add(collider.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        nearParticles.Remove(collider.gameObject);
    }
}
