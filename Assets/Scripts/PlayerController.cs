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
    public float attackKnockback = 4;

    private Vector2 slashDirection;
    private SpriteRenderer playerSprite;
    private bool isAttacking = false;
    private Collider2D slashCollider;

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

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        targetVelocity = move * maxSpeed;
    }

    protected override void LookDirection()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            playerSprite.flipX = false;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            playerSprite.flipX = true;
        }

        if (isAttacking == false)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                lookDirection.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI);
            }
            else if (playerSprite.flipX == true)
            {
                lookDirection.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                lookDirection.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            slashDirection = lookDirection.transform.eulerAngles;
        }

    }

    protected override void Attack()
    {
        if (Input.GetButtonDown("Attack") && isAttacking == false)
        {
            isAttacking = true;
            StartCoroutine(AttackAndCooldown());
        }
    }

    public void AttackOverlap(Collider2D other)
    {
        if (other == gameObject.GetComponent<CapsuleCollider2D>())
        {
            print("player got hit");
        }
        else
        {
            velocity = -slashDirection * attackKnockback;
            print("hit");
        }
    }

    IEnumerator AttackAndCooldown()
    {
        slash.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        slash.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}
