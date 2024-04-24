using Movement;
using System;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    [SerializeField] private PlayerBody body;
    public float force;
    public int maxQty;
    public float floorAngle;
    private int currentJumpQty;

    public event Action OnJump = delegate { };
    public event Action OnLand = delegate { };
    private void Reset()
    {
        body = GetComponent<PlayerBody>();
    }

    public void TryJump()
    {
        if (currentJumpQty >= maxQty)
        {
            return;
        }
        currentJumpQty++;
        body.RequestImpulse(new ImpulseRequest(Vector3.up, force));
        OnJump.Invoke();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        float contactAngle = Vector3.Angle(contact.normal, Vector3.up);
        if (contactAngle <= force)
        {
            OnLand?.Invoke();
            currentJumpQty = 0;
        }
    }
}
