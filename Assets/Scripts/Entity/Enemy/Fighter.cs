using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private SpriteRenderer _sr;
    
    public float Speed;
    public int StartDirection;

    private int _direction;

    void Start()
    {
        _direction = StartDirection;
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(Speed * _direction, _rb.velocity.y);
        var sprite = _sr.sprite;
        _bc.size = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
    }
}
