using UnityEngine;

public class Sapo : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] Transform firePoint;
    [SerializeField] LineController lineController;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Config")]
    [SerializeField] private float cooldown = 1f;

    private float timer = 0f;

// controla o lado (true = frente, false = trás)
    private bool shootingForward = false;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            Shoot();
            timer = 0f;

            // alterna lado a cada tiro
            shootingForward = !shootingForward;

            // flipa o sprite
            spriteRenderer.flipX = !shootingForward;
        }
    }

    void Shoot()
    {
        Vector3 direction;

        if (shootingForward)
        {
            // 45° pra frente (direita + cima)
            direction = new Vector3(1f, 1f, 0f);
        }
        else
        {
            // 45° pra trás (esquerda + cima)
            direction = new Vector3(-1f, 1f, 0f);
        }

        direction.Normalize();

        lineController.ThrowLineRenderer(direction);
    }
}