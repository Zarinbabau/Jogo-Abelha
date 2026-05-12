using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movimento")]
    public float moveSpeed = 6f;
    public float liftForce = 50f;

    [Header("Rotação")]
    public float tiltAngle = 25f;

    [Header("Inventário")]
    public int capacidadeMaxima = 3;

    float horizontalInput;
    bool liftInput;

    // Lista de pólens carregados
    private List<Polen> polensCarregados =
        new List<Polen>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        liftInput = Input.GetKey(KeyCode.W);
    }

    void FixedUpdate()
    {
        HandleRotation();

        Vector2 velocity = rb.linearVelocity;

        velocity.x = horizontalInput * moveSpeed;

        if (liftInput)
        {
            velocity.y += liftForce * Time.fixedDeltaTime;
        }

        rb.linearVelocity = velocity;
    }

    void HandleRotation()
    {
        float targetAngle = -horizontalInput * tiltAngle;

        transform.rotation =
            Quaternion.Euler(0, 0, targetAngle);
    }

    // =========================
    // PÓLEN
    // =========================

    public bool PodeColetar()
    {
        return polensCarregados.Count < capacidadeMaxima;
    }

    public void ColetarPolen(Polen polen)
    {
        if (!PodeColetar())
            return;

        polensCarregados.Add(polen);
    }

    public void EntregarPolen()
    {
        if (polensCarregados.Count <= 0)
            return;

        int pontosGanhos = 0;

        foreach (Polen polen in polensCarregados)
        {
            polen.Entregar();

            pontosGanhos += polen.Score;
        }

        GameController.instance.AddScore(pontosGanhos);

        polensCarregados.Clear();
    }

    public void DroparPolens()
    {
        if (polensCarregados.Count <= 0)
            return;

        foreach (Polen polen in polensCarregados)
        {
            polen.Respawn();
        }

        polensCarregados.Clear();
    }
}