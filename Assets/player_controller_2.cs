using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroller : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if(canMove) {
                    
            // If movement input is not 0, try to move
            if(movementInput != Vector2.zero){
            bool succes = TryMove(movementInput);
                
                if(!succes && movementInput.x != 0) {
                    succes = TryMove(new Vector2 (movementInput.x, 0)); 
                }

                if(!succes && movementInput.y != 0) {
                    succes = TryMove(new Vector2 (0, movementInput.y));
                }

                animator.SetBool("isMoving", succes);
            } else {
                animator.SetBool("isMoving", false);
            }

            // set direction of sprite to movement direction
            if(movementInput.x < 0) {
                spriteRenderer.flipX = true;
            } else if (movementInput.x > 0) {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction) {
        // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers collide with
                castCollisions, // List of collisions to store the found collisions into after the cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount of cast equal to the movement plus an offset

            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;    
            } else {
                return false;
            }
    }


    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack() {
        LockMovement();

        if(spriteRenderer.flipX == true){
            swordAttack.AttackLeft();
        } else {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack() {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement() {
        canMove = false;

    }

    public void UnlockMovement() {
        canMove = true;
    }
}
