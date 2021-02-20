using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public Animator Animator;

    public float Speed;
    public float JumpVelocity;
    public float VelocityCap;
    public short MaxJumps;

    public Action EntityInteract = () => { };
    
    private short _jumps;

    private bool _doLeft;
    private bool _doRight;

    void Start()
    {
        _jumps = MaxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        _doLeft = false;
        _doRight = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_jumps-- > 0)
            {
                var newVel = RB.velocity;
                newVel.y += JumpVelocity;
                _cap(ref newVel.y, JumpVelocity, VelocityCap);
                RB.velocity = newVel;
            }
        } 
        if (Input.GetKey(KeyCode.A)) _doLeft = true;
        if (Input.GetKey(KeyCode.D)) _doRight = true;
        if (Input.GetKey(KeyCode.S)) EntityInteract();

        if      (_doRight && !_doLeft) RB.velocity = new Vector2(Speed, RB.velocity.y);
        else if (_doLeft && !_doRight) RB.velocity = new Vector2(-Speed, RB.velocity.y);
        else RB.velocity = new Vector2(0, RB.velocity.y);

        _animate();
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Surface")) return;
        
        //if player is above surface. this stops banging into a wall from giving jumps back
        //if(transform.position.y - transform.localScale.y / 2 > other.transform.position.y + other.transform.localScale.y / 2)
            _jumps = MaxJumps;
    }

    private void _cap<T>(ref T value, T min, T max) where T : struct, IComparable
    {
        if (value.CompareTo(min) < 0) value = min;
        else if (value.CompareTo(max) > 0) value = max;
    }

    private void _animate()
    {
        if (RB.velocity.x < 0) SR.flipX = true;
        if (RB.velocity.x > 0) SR.flipX = false;
        Animator.SetFloat("xSpeed", Math.Abs(RB.velocity.x));
        Animator.SetFloat("yVelocity", RB.velocity.y);
    }
}
