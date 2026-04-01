using Unity.Mathematics;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;    
    public float maxSize = 2.0f;
    public float minSpeed = 100f;
    public float maxSpeed = 200f;
    public float maxSpinSpeed = 10f;
    public GameObject collisonEffectPrefab;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = UnityEngine.Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed) / randomSize;

        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = UnityEngine.Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //Create collison effect only if the speed of the object is faster than what it hit to avoid spawning two effect for each collison
        //Throws an error when hitting the wall because it doesn't have a rigidbody
        //if (rb.linearVelocity.magnitude >= collision.collider.attachedRigidbody.linearVelocity.magnitude)
        {
        Vector2 contactPoint = collision.GetContact(0).point;
        //Scale the side of the collision effect based on the speed of the object
        collisonEffectPrefab.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f) * rb.linearVelocity.magnitude;
        GameObject collisionEffect = Instantiate(collisonEffectPrefab, contactPoint, quaternion.identity);
        
        Destroy(collisionEffect, 1f);
        }
    }
}
