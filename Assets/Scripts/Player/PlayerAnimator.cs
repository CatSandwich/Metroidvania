using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    public Sprite Rising;
    public Sprite Falling;
    public Animation Walking;
    public Animation Idle;

    [HideInInspector]
    public StateType State;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        if (!_rb) Debug.LogError("Failed to retrieve RididBody2D");
        if (!_sr) Debug.LogError("Failed to retrieve SpriteRenderer");
    }

    // Update is called once per frame
    void Update()
    {
        _setDirection();

        _sr.sprite = _vertical() ?? _walking() ?? _idle();
    }

    private void _setDirection()
    {
        if (_rb.velocity.x < 0) _sr.flipX = true;
        if (_rb.velocity.x > 0) _sr.flipX = false;
    }

    private Sprite _walking()
    {
        if (_rb.velocity.x == 0) return null;
        State = StateType.Walking;
        return Walking.Next();
    }

    private Sprite _vertical()
    {
        if (_rb.velocity.y == 0) return null;
        return _rb.velocity.y > 0 ? Rising : Falling;
    }

    private Sprite _idle()
    {
        return Idle.Next();
    }

    public enum StateType
    {
        Walking,
        Falling
    }

    [Serializable]
    public class Animation
    {
        public static Animation[] Animations;

        public bool Continuous;
        public float AnimationLength;
        public Sprite[] Frames;

        private int Num => Frames.Length;
        private float? _start;

        public static void Init(Animation[] animations)
        {
            Animations = animations;
        }

        public Sprite Next()
        {
            foreach (var a in Animations.Where(a => a != this))
            {
                a._reset();
            }

            _start ??= Time.time;

            var elapsed = Time.time - _start;
            var index = (int)((float)elapsed / AnimationLength * Num);

            if (index != Num) return Frames[index];

            _start = Time.time;
            return Continuous ? Frames[0] : null;
        }

        private void _reset()
        {
            _start = null;
        }
    }
}
