using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;

    public Vector3 targetPosA;
    public Vector3 targetPosB;

    public float speed = 2f;

    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
      
        activated = true;

    }

    void Update()
    {
        if (!activated) return;

        objectA.position = Vector3.MoveTowards(
            objectA.position,
            targetPosA,
            speed * Time.deltaTime
        );

        objectB.position = Vector3.MoveTowards(
            objectB.position,
            targetPosB,
            speed * Time.deltaTime
        );
    }
}
