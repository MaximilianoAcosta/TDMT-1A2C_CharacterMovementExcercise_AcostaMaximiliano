using Movement;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBody : MonoBehaviour
{
    [SerializeField] private LayerMask floorMask;
    private Rigidbody rb;
    private MovementRequest currentMovement = MovementRequest.InvalidRequest;
    private readonly List<ImpulseRequest> _impulseRequests = new();
    [SerializeField] private Vector3 floorCheckOffset = new(0, 0.001f, 0);

    [SerializeField] private float maxFloorDistance = .1f;
    [SerializeField] private float brakeMultiplier = 1;
    [SerializeField] private float gravitymultiplier;
    [SerializeField] private float fallspeedtreshold;
    //the limit of the normal of the surface the player can move up
    [SerializeField] private float normalAngleLimit;
    private bool isBrakeRequested = false;
    public bool isFalling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isBrakeRequested)
        {
            Break();
        }

        ManageMovement();
        ManageImpulseRequests();
    }

    public void SetMovement(MovementRequest movementRequest)
    {
        currentMovement = movementRequest;
    }

    public void RequestBrake()
    {
        isBrakeRequested = true;
    }

    public void RequestImpulse(ImpulseRequest request)
    {
        _impulseRequests.Add(request);
    }

    private void Break()
    {
        rb.AddForce(-rb.velocity * brakeMultiplier, ForceMode.Impulse);
        isBrakeRequested = false;
    }
    private void ManageMovement()
    {
        Vector3 velocity = rb.velocity;
        //velocity.y = 0;
        Vector3 accelerationVector;
        isFalling = !Physics.Raycast(transform.position + floorCheckOffset, -transform.up, out var hit, maxFloorDistance, floorMask);
        if (!currentMovement.IsValid() || velocity.magnitude >= currentMovement.GoalSpeed)
        {
            return;
        }

        accelerationVector = currentMovement.GetAccelerationVector();
        if (isFalling)
        {
            rb.AddForce(accelerationVector, ForceMode.Force);
        }
        else
        {
            accelerationVector = Vector3.ProjectOnPlane(accelerationVector, hit.normal);
            if (accelerationVector == Vector3.zero || Vector3.Angle(accelerationVector, transform.up) >= normalAngleLimit)
            {
                rb.AddForce(accelerationVector, ForceMode.Force);
            }
            else
            {
                rb.velocity = new Vector3(0, -Vector3.up.y*rb.velocity.y, 0);
            }
        }
    }

    private void ManageImpulseRequests()
    {
        foreach (ImpulseRequest request in _impulseRequests)
        {
            rb.AddForce(request.GetForceVector(), ForceMode.Impulse);
        }
        if (rb.velocity.y < fallspeedtreshold && isFalling)
        {
            rb.velocity += gravitymultiplier * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
        }
        _impulseRequests.Clear();
    }
}

