using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    public Rigidbody2D RB;

    public float Speed;
    public float JumpForce;
    public short MaxJumps;

    private short _jumps;

    private bool _doLeft;
    private bool _doRight;
    
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        _jumps = MaxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        _doLeft = false;
        _doRight = false;
        
        if (Input.GetKey(KeyCode.Space) && _jumps-- > 0) RB.AddForce(new Vector2(0, JumpForce));
        if (Input.GetKey(KeyCode.A)) _doLeft = true;
        if (Input.GetKey(KeyCode.D)) _doRight = true;

        if      (_doRight && !_doLeft) RB.velocity = new Vector2(Speed, RB.velocity.y);
        else if (_doLeft && !_doRight) RB.velocity = new Vector2(-Speed, RB.velocity.y);
        else RB.velocity = new Vector2(0, RB.velocity.y);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Surface")) _jumps = MaxJumps;
    }
}
