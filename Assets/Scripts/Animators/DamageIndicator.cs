using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public float SpeedY;
    public float SpeedAlpha;
    private TextMeshPro _tmp;
    
    void Awake()
    {
        _tmp = GetComponent<TextMeshPro>();
    }
    
    public void Setup(int damage)
    {
        _tmp.SetText(damage.ToString());
    }

    void Update()
    {
        transform.position += new Vector3(0f, SpeedY) * Time.deltaTime;
        _tmp.alpha -= SpeedAlpha * Time.deltaTime;
        if(_tmp.alpha <= 0f) Destroy(gameObject);
    }
}
