using UnityEngine;

public class RotateCharacterBasedOnVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minimumSpeedForRotation = 0.001f;

    private void Update()
    {
        Vector3 velocity = rigidBody.velocity;
        velocity.y = 0;
        if (velocity.magnitude < minimumSpeedForRotation)
            return;
        var rotationAngle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
        transform.Rotate(Vector3.up, rotationAngle * rotationSpeed * Time.deltaTime);
    }
}
