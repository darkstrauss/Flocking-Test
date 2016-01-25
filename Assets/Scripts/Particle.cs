using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Particle : MonoBehaviour {

    public Vector3 velocity = new Vector3();

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
        Vector3 cohesion = new Vector3();
        Vector3 alignment = new Vector3();
        Vector3 separation = new Vector3();

        ScreenWrap();

        foreach (GameObject gO in nearParticles)
        {
            cohesion += gO.transform.position;
            alignment += gO.GetComponent<Particle>().velocity;
            separation += transform.position - gO.transform.position;
        }

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

        viewPortPostition = cam.WorldToViewportPoint(transform.position);

        newPosition = transform.position;

        if (viewPortPostition.x > 1 || viewPortPostition.x < 0)
        {
            newPosition.x = -newPosition.x * 0.9f;
            //velocity = -velocity;
            isWrappingX = true;
        }
        if (viewPortPostition.y > 1 || viewPortPostition.y < 0)
        {
            newPosition.y = -newPosition.y * 0.9f;
            //velocity = -velocity;
            isWrappingY = true;
        }

        transform.position = newPosition;

    }

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
