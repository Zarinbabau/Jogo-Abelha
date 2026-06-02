using UnityEngine;
using System.Collections;

public class Sapo : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] Transform firePoint;
    [SerializeField] LineController lineController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Config")]
    [SerializeField] private float cooldown = 1f;

    [Header("Animação")]
    [SerializeField] private float mouthOpenTime = 0.5f;

    private float timer = 0f;

    // true = direita
    // false = esquerda
    private bool shootingForward = false;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            Shoot();

            timer = 0f;

            shootingForward = !shootingForward;

            spriteRenderer.flipX = !shootingForward;
        }
    }

    void Shoot()
    {
        animator.SetBool("MouthOpen", true);

        StopAllCoroutines();
        StartCoroutine(CloseMouth());

        Vector3 direction;

        if (shootingForward)
        {
            direction = new Vector3(1f, 1f, 0f);
        }
        else
        {
            direction = new Vector3(-1f, 1f, 0f);
        }

        direction.Normalize();

        lineController.ThrowLineRenderer(direction);
    }

    IEnumerator CloseMouth()
    {
        yield return new WaitForSeconds(mouthOpenTime);

        animator.SetBool("MouthOpen", false);
    }
}