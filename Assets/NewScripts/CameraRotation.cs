using Inputs;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float mindistance;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float distance;

    private void OnEnable()
    {
        inputReader.OnCameraRotation += RotateCamera;
    }
    private void Update()
    {
        distance = (transform.position-target.position).magnitude;
        if (distance > mindistance) 
        {
            Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    private void RotateCamera(Vector2 direction)
    {
        transform.RotateAround(target.transform.position, Vector3.up, direction.x* rotationSpeed * Time.deltaTime);
    }
}
