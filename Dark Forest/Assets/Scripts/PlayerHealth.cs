using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public static float health, maxHealth = 10;
    public Rigidbody2D rb;
    float damageTimer;
    float healTimer;
    public Collider2D collision;
    public GameObject bloodMagic;
    public GameObject deathMenuUI;
    public AudioSource playerHitSound;
    public AudioSource bloodDrainSound;
    public Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void OnTriggerStay2D(Collider2D collision) //if enemies are within the 'absord shield', the player will heal with the TakeHealth method
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            //Debug.Log("player healing...");
            Debug.Log("blood magic animation enabled...");
            TakeHealth(0.1f);
            bloodDrainSound.Play();

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("blood magic animation disabled...");
        bloodMagic.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)//if enemies collide with the player collider, the player will take damage
    {
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("player takes damage...");
            TakeDamage(1f);
            playerHitSound.Play();

        }
    }

    public void TakeHealth(float healAmount)
    {
        if (healTimer <= 0)
        {
            health += healAmount;
            healTimer = 1;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (damageTimer <= 0)
        {
            health -= damageAmount;
            damageTimer = 1;
        }
        if (health <= 0)
        {
            ExpScript.currentLevel = 1;
            ExpScript.currentExp = 0;
            playerAnimator.Play("player_death");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(waiter());
        }
    }
    IEnumerator waiter()
    {

        yield return new WaitForSeconds(0.50f);
        Time.timeScale = 0f;
        deathMenuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }

        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;
        }
    }
}
