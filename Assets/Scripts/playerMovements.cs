using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerMovements : MonoBehaviour
{
    private Rigidbody2D rigidb;
    private Animator animator;
    private bool hasWeapon = false;
    public RuntimeAnimatorController defaultController;
    private AnimatorOverrideController currentOverrideController;
    private bool isGrounded;
    private int jumpCount;
    private const int maxJumps = 2;
    private weaponChange nearbyWeapon; 
    private int coinCount = 0;
    public Text coinText;
    void Start()
    {
        rigidb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isIdle", false);
        UpdateCoinUI();
    }

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rigidb.velocity = new Vector2(dirX * 2.2f, rigidb.velocity.y);

        if (Input.GetKeyDown(KeyCode.E) && nearbyWeapon != null)
        {
            nearbyWeapon.PickUp(this.gameObject);
        }
        
        animator.SetBool("isRunning", Mathf.Abs(rigidb.velocity.x) > 0.1f);
        animator.SetBool("isJumping", !isGrounded);

        if (Input.GetKeyDown(KeyCode.V) && isGrounded)
        {
            StartCoroutine(TriggerRoll());
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rigidb.velocity = new Vector2(rigidb.velocity.x, 4.4f);
                isGrounded = false;
                jumpCount = 1;
            }
            else if (jumpCount < maxJumps)
            {
                rigidb.velocity = new Vector2(rigidb.velocity.x, 4.4f);
                jumpCount++;
            }
        }
        if (hasWeapon)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("F key pressed");
                animator.ResetTrigger("TriggerAttack2"); 
                animator.SetTrigger("TriggerAttack1");
                animator.SetBool("isIdle", false);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
              // Debug.Log("G key pressed");
                animator.ResetTrigger("TriggerAttack1"); 
                animator.SetTrigger("TriggerAttack2");
                animator.SetBool("isIdle", false);
            }
            else
            {
                animator.SetBool("isIdle", true);
            }
        }
    }

    public bool HasWeapon()
    {
        return hasWeapon;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = " " + coinCount; 
    }

    public void EquipWeapon(AnimatorOverrideController newOverride)
    {
        hasWeapon = true;
        currentOverrideController = newOverride;
        animator.runtimeAnimatorController = currentOverrideController;
    }

    IEnumerator TriggerRoll()
    {
        animator.SetBool("isRolling", true);

        float rollDuration = 0.5f;
        float rollSpeed = 4f;
        float direction = Mathf.Sign(rigidb.velocity.x);

        if (direction == 0) direction = 1;

        float elapsedTime = 0;
        while (elapsedTime < rollDuration)
        {
            rigidb.velocity = new Vector2(rollSpeed * direction, rigidb.velocity.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isRolling", false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            //Debug.Log("Entered weapon's trigger zone.");
            nearbyWeapon = collision.GetComponent<weaponChange>();
        }
    }
    public void DropWeapon()
    {
        hasWeapon = false;
        animator.runtimeAnimatorController = null; 
        currentOverrideController = null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
           //Debug.Log("Exited weapon's trigger zone.");
            nearbyWeapon = null;
        }
    }
}