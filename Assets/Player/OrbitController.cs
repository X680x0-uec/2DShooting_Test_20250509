using UnityEngine;

public class OrbitController : MonoBehaviour
{
    public float rotationSpeed = 1000f;
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
