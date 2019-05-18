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

    private SpriteRenderer playerSprite;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
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
        if (velocity.x > 0)
        {
            playerSprite.flipX = false;
        }
        else if (velocity.x < 0)
        {
            playerSprite.flipX = true;
        }
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

    }

    protected override void Attack()
    {
        if (Input.GetButtonDown("Attack") && isAttacking == false)
        {
            isAttacking = true;
            StartCoroutine(AttackAndCooldown());
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
