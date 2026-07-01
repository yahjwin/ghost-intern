using UnityEngine;

public class RoadMover : MonoBehaviour
{
    public float speed = 10f;
    public float startZ = -50f;
    public float resetZ = 100f;

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= startZ)
        {
            Vector3 pos = transform.position;
            pos.z = resetZ;
            transform.position = pos;
        }
    }
}