using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Entity
{
    public float Speed = 10.0f;

    public Swarm Swarm;
    public Camera PlayerCamera;
    public GameObject SpriteGameObject;

    public override void Update()
    {
        base.Update();

        PlayerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, PlayerCamera.transform.position.z);

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var velocity = new Vector2(x, y) * Speed;

        _rigidbody.velocity = velocity;

        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePosition - (Vector2)transform.position).normalized;

        Swarm.SetIsShooting(Input.GetButton("Fire1"), direction);
        SpriteGameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }
}
