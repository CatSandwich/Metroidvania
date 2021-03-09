using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public AnimationClip DeathAnimation;
    private Animator _animator;
    private Rigidbody2D _rb;

    public float Speed;
    public int StartDirection;

    public int StartingHealth;

    private int _health;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(Speed * StartDirection, 0f);

        _health = StartingHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0) StartCoroutine(_die());
    }

    private IEnumerator _die()
    {
        foreach (var bc in GetComponentsInChildren<BoxCollider2D>()) bc.enabled = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;

        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(DeathAnimation.length);
        Destroy(gameObject);
    }
}
