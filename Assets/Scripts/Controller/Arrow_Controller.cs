using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;
    private ParticleSystem ps;

    private Archer archer;
    
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private Vector2 arrowVelociy;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    [SerializeField] private float timer;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        ps = GetComponentInChildren<ParticleSystem>();

        timer = 30;
    }

    private void Update()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(arrowVelociy.x, arrowVelociy.y);
            transform.right = arrowVelociy;
        }

        timer -= Time.deltaTime;
        if (timer < 0) Destroy(gameObject);
    }

    public void SetupArrow(Archer _archer, float _speed, Vector2 _dir)
    {
        archer = _archer;
        _dir.Normalize();
        arrowVelociy = new Vector2(_dir.x * _speed, _dir.y * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            archer.GetComponent<CharacterStats>().DoDamage(collision.GetComponent<CharacterStats>(), archer.attackMultiplier, DamageType.Physical);

            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        cd.enabled = false;
        ps.Stop();

        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, 5);
    }

    public void FlipArrow()
    {
        if (flipped) return;

        arrowVelociy *= -1;
        flipped = true;

        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }


}
