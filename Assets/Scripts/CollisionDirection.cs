using UnityEngine;

public class CollisionDirection
{
    private static LayerMask All => ~0;
    private readonly GameObject _source;
    private readonly GameObject _target;
    private readonly LayerMask _mask;

    public CollisionDirection(GameObject source, GameObject target)
    {
        _source = source;
        _target = target;
        _mask = 1 << target.layer;
    }
    
    public bool Raycast(Vector2 direction)
    {
        var hit = Physics2D.Raycast(_source.transform.position, direction, Mathf.Infinity, _mask);
        return hit.collider != null && ReferenceEquals(hit.collider.gameObject, _target);
    }

    public static bool CheckDirection(GameObject source, GameObject target, Vector2 direction)
    {
        var hit = Physics2D.Raycast(source.transform.position, direction, Mathf.Infinity, 1 << target.layer);
        return hit.collider != null && ReferenceEquals(hit.collider.gameObject, target);
    }
}
