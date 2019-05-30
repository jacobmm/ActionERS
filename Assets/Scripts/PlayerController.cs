using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public GameObject lookDirection;
    public GameObject slash;
    public float attackDuration = 0.3f;
    public float attackCooldown = 0.1f;
    public float attackKnockback = 8;
    public float attackDamage = 25;

    public string horizontalControl;
    public string jumpControl;
    public string attackControl;
    public string verticalControl;

    private SpriteRenderer playerSprite;
    private bool isAttacking = false;
    private Collider2D slashCollider;
    private bool hitGround = false;
    private Vector2 slashDirection;
    private bool moveDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        slashCollider = slash.GetComponent<BoxCollider2D>();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        Vector2 move = Vector2.zero;

        if (moveDisabled == false)
        move.x = Input.GetAxis(horizontalControl);

        if (Input.GetButtonDown(jumpControl) && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp(jumpControl))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        targetVelocity = move * maxSpeed;

        if (hitGround == true)
        {
            
            rb2d.AddForce(-slashDirection * attackKnockback, ForceMode2D.Impulse);
            hitGround = false;
        }
    }

    // flips character sprite based on movment direction and rotates the attack direction
    protected override void LookDirection()
    {
        if (Input.GetAxis(horizontalControl) > 0)
        {
            playerSprite.flipX = false;
        }
        else if (Input.GetAxis(horizontalControl) < 0)
        {
            playerSprite.flipX = true;
        }

        if (isAttacking == false)
        {
            if (Input.GetAxis(verticalControl) != 0 || Input.GetAxis(horizontalControl) != 0)
            {
                lookDirection.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis(verticalControl), Input.GetAxis(horizontalControl)) * 180 / Mathf.PI);
            }
            else if (playerSprite.flipX == true)
            {
                lookDirection.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                lookDirection.transform.eulerAngles = new Vector3(0, 0, 0);
            }

            Vector3 direction = Vector3.right;

            slashDirection = lookDirection.transform.TransformDirection(direction);

            Vector2 position = lookDirection.transform.position;

            Debug.DrawLine(position, position + slashDirection, Color.green);
        }
    }

    protected override void Attack()
    {
        if (Input.GetButtonDown(attackControl) && isAttacking == false)
        {
            isAttacking = true;
            StartCoroutine(AttackAndCooldown());
        }
    }

    public void AttackOverlap(Collider2D other)
    {
        if (other == gameObject.GetComponent<CapsuleCollider2D>())
        {

        }
        else if (other.tag == "Player")
        {
            var otherPlayer = other.GetComponent<PlayerStatus>();
            otherPlayer.TakeDamage(attackDamage);
            print(otherPlayer.playerCurrentHealth);
            hitGround = true;
            
        }
    }

    IEnumerator AttackAndCooldown()
    {
        slash.SetActive(true);
        moveDisabled = true;
        yield return new WaitForSeconds(attackDuration);

        slash.SetActive(false);
        moveDisabled = false;
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}
