using UnityEngine;

public class VespaBehaviour : MonoBehaviour

{
    [SerializeField] private float speed = 3f;

    private Transform target;
    private bool isFollowing = false;

    //vespa

    void Update()
    {
        if (isFollowing && target != null)
        {

            //trans vespa 
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        //vespa lerp ponto inicial
        
        if (collision.CompareTag("Player"))
        {
            isFollowing = false;
            target = null;
        }
    }
}