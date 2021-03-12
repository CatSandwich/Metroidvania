using UnityEngine;

namespace Entity.State_Machine
{
    public class StateMachineContext
    {
        public Transform Transform => GameObject.transform;
        public Vector3 Position => Transform.position;
        public readonly GameObject GameObject;
        public readonly Rigidbody2D Rigidbody;

        public StateMachineContext(GameObject go, Rigidbody2D rb)
        {
            GameObject = go;
            Rigidbody = rb;
        }
    }
}