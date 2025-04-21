using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.5f;
    public float jumpForce = 2.5f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    //References
    private Rigidbody2D _rigibody;
    private Animator _animator;

    //Movement
    private Vector2 _movement;
    private bool _facingRight = true;
    private bool _isGrounded;

    //Attack
    private bool _isAttacking;

    void Awake()
    {
        _rigibody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( _isAttacking == false)
        {
            //Movement
            float horizontalImput = Input.GetAxisRaw("Horizontal");
            _movement = new Vector2(horizontalImput, 0f);

            //Flip character
            if (horizontalImput < 0f && _facingRight == true)
            {
                Flip();
            }
            else if (horizontalImput > 0f && _facingRight == false)
            {
                Flip();
            }
        }        

        //Es Suelo?
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //Esta Saltando?
        if (Input.GetButtonDown("Jump") &&  _isGrounded == true && _isAttacking == false)
        {
            _rigibody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Atacara?
        if(Input.GetButtonDown("Fire1") && _isGrounded == true && _isAttacking == false)
        {
            _movement = Vector2.zero;
            _rigibody.velocity = Vector2.zero;
            _animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if (_isAttacking == false)
        {
            float horizontalVelocity = _movement.normalized.x * speed;
            _rigibody.velocity = new Vector2(horizontalVelocity, _rigibody.velocity.y);
        }
    }

    void LateUpdate()
    {
        _animator.SetBool("Idle", _movement == Vector2.zero);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("VerticalVelocity", _rigibody.velocity.y);

        // Animator
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _isAttacking = true;
        }
        else
        {
            _isAttacking = false;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
