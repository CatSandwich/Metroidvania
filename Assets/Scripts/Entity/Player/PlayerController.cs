using System;
using System.Collections;
using Entity.Player.EventArgs;
using Helpers.Mask;
using UnityEngine;
using UnityEngine.UIElements;

namespace Entity.Player
{
    public class PlayerController : MonoBehaviour
    {
        private static MetroidMask TerrainMask => new MetroidMask("Terrain", null);
        
        #region Constants
        public Vector3 Normal => new Vector3(1f, 1f, 1f);
        public Vector3 Flipped => new Vector3(-1f, 1f, 1f);
        public Vector2 StopX => new Vector2(0f, _rb.velocity.y);
        #endregion
        
        #region Components
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Animator _animator;
        
        public AnimationClip AttackAnimation;
        #endregion

        #region Temporary KeyCode Settings
        public KeyCode KRunLeft;
        public KeyCode KRunRight;
        public KeyCode KJump;
        public KeyCode KEntityInteract;
        public MouseButton MAttack;
        public MouseButton MShoot;
        #endregion

        #region Stats
        public float Speed;
        public float JumpVelocity;
        public float VelocityCap;
        public short MaxJumps;

        public int MaxHealth;

        public GameObject SwordHit;
        #endregion

        #region Event Declarations
        public event EventHandler<RunEventArgs> Run;
        public event EventHandler<JumpEventArgs> Jump;
        public event EventHandler<LandEventArgs> Land;
        public event EventHandler<AttackEventArgs> Attack;
        public event EventHandler<EntityInteractEventArgs> EntityInteract;
        #endregion

        #region Private Fields
        private short _jumps;
        private bool _isAttacking;
        private bool _isGrounded;

        private int _health;

        private System.Random _random;
        #endregion

        #region Unity Events

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            
            _jumps = MaxJumps;
            _isAttacking = false;
            _random = new System.Random();

            _health = MaxHealth;

            Attack += (sender, e) =>
            {
                if (_random.Next(0, 10) == 0) e.Crit = true;
            };
        }

        // Update is called once per frame
        private void Update()
        {
            
            _rb.velocity = StopX;
            _handleInput();
            _animate();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var mask = TerrainMask;
            mask.GO = other.gameObject;
            if (!mask.HasLayer) return;
            
            if (!CollisionDirection.CheckDirection(gameObject, other.gameObject, Vector2.down)) return;
            _onLand();
        }
        #endregion

        #region Events
        private void _onRun(bool direction)
        {
            var e = new RunEventArgs(direction, _isGrounded);
            Run?.Invoke(this, e);
            if (!e.Default) return;

            _rb.velocity = new Vector2((direction == RunEventArgs.Left ? -1 : 1) * Speed, _rb.velocity.y);
        }

        private void _onJump()
        {
            var e = new JumpEventArgs(_jumps);
            Jump?.Invoke(this, e);
            if (!e.Default) return;

            if (_jumps-- <= 0) return;

            var vel = _rb.velocity;
            vel.y += JumpVelocity;
            _cap(ref vel.y, JumpVelocity, VelocityCap);
            _rb.velocity = vel;
            _isGrounded = false;
        }

        private void _onLand()
        {
            var e = new LandEventArgs();
            Land?.Invoke(this, e);
            if (!e.Default) return;

            _jumps = MaxJumps;
            _isGrounded = true;
        }

        private void _onAttack()
        {
            if (_isAttacking) return;
            
            var e = new AttackEventArgs();
            Attack?.Invoke(this, e);
            if (!e.Default) return;

            AttackType type;

            if (e.Crit) type = AttackType.AttackCrit;
            else type = _random.Next(0, 2) == 0 ? AttackType.Attack1 : AttackType.Attack2;
            
            StartCoroutine(_attack(type));
        }

        private IEnumerator _attack(AttackType type)
        {
            _isAttacking = true;
            Run += cancelMove;

            _animateAttack(type);
            yield return new WaitForSeconds(AttackAnimation.length);
            _animateIdle();

            var hit = Instantiate(SwordHit);
            hit.transform.position = transform.position;
            hit.transform.position += new Vector3(transform.localScale.x, 0.25f, 0f);
            var bc = hit.GetComponent<BoxCollider2D>();
            bc.size = new Vector2(1f, 2f);

            Run -= cancelMove;
            _isAttacking = false;

            static void cancelMove(object sender, RunEventArgs e)
            {
                if (e.Grounded)
                    e.PreventDefault();
            }
        }
        #endregion

        #region Input
        private void _handleInput()
        {
            if (Input.GetMouseButton((int)MAttack))
                _onAttack();
            
            if (Input.GetKeyDown(KJump))
                _onJump();

            if (Input.GetKeyDown(KEntityInteract))
                EntityInteract?.Invoke(this, new EntityInteractEventArgs());

            var dir = _getRunDirection();
            if (dir == null)
                _rb.velocity = StopX;
            else
                _onRun(dir.Value);
        }
    
        private bool? _getRunDirection()
        {
            var left = Input.GetKey(KRunLeft);
            var right = Input.GetKey(KRunRight);
            if (left && !right) return RunEventArgs.Left;
            if (right && !left) return RunEventArgs.Right;
            return null;
        }
        #endregion

        #region Combat

        public void TakeDamage(int damage)
        {
            _health -= damage;
            //print($"Health: {_health}");
        }
        #endregion

        #region Helpers
        private void _cap<T>(ref T value, T min, T max) where T : struct, IComparable
        {
            if (value.CompareTo(min) < 0) value = min;
            else if (value.CompareTo(max) > 0) value = max;
        }
        #endregion

        #region Animation
        private void _animate()
        {
            if (_rb.velocity.x > 0f)
                transform.localScale = Normal;
            else if (_rb.velocity.x < 0f)
                transform.localScale = Flipped;
            
            _animator.SetFloat("xSpeed", Math.Abs(_rb.velocity.x));
            _animator.SetFloat("yVelocity", _rb.velocity.y);
        }

        private void _animateAttack(AttackType type)=> _animator.SetTrigger(Enum.GetName(typeof(AttackType), type));
        private void _animateIdle() => _animator.SetTrigger("Idle");

        #endregion
    }

    internal enum AttackType
    {
        Attack1,
        Attack2,
        AttackCrit,
        AttackShoot
    }
}
