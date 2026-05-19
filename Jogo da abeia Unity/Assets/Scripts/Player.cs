using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public System.Action<int> OnInventarioChanged;

    [Header("Movimento")]
    public float moveSpeed = 6f;
    public float liftForce = 50f;

    [Header("Rotação")]
    public float tiltAngle = 25f;

    [Header("Inventário")]
    public int capacidadeMaxima = 3;

    float horizontalInput;
    bool liftInput;

    // Controle de movimento
    private bool podeMover = true;

    // Lista de pólens carregados
    private List<Polen> polensCarregados =
        new List<Polen>();
    void Start()
    {
        OnInventarioChanged?.Invoke(polensCarregados.Count);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Bloqueia inputs
        if (!podeMover)
        {
            horizontalInput = 0f;
            liftInput = false;
            return;
        }

        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        liftInput = Input.GetKey(KeyCode.W);
    }

    void FixedUpdate()
    {
        if (!podeMover)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

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
    // TRAVAR MOVIMENTO
    // =========================

    public void TravarMovimento()
    {
        podeMover = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.gravityScale = 0f;

        rb.constraints =
            RigidbodyConstraints2D.FreezeAll;
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
        
        OnInventarioChanged?.Invoke(polensCarregados.Count);

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

        OnInventarioChanged?.Invoke(0);
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

        OnInventarioChanged?.Invoke(0);

    }
}