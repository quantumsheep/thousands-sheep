using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Entity
{
    public float Speed = 10.0f;

    public override void Update()
    {
        base.Update();

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var velocity = new Vector2(x, y) * Speed;

        _rigidbody.velocity = velocity;
    }
}
