using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimator : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("SpeedX", Mathf.Abs(_rb.velocity.x));
    }
}
