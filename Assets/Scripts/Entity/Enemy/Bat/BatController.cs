using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public int MaxHealth;
    public float Speed;
    public int StartDirection;

    private int _health;
    private int _direction;

    void Start()
    {
        _health = MaxHealth;
        _direction = StartDirection;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Speed * _direction * Time.deltaTime, 0f, 0f);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0) Destroy(gameObject);
    }
}
