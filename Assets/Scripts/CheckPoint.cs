using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;

    public string checkPointID;
    public bool activated;

    private bool canActive;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canActive && Input.GetKeyDown(KeyCode.W))
        {
            ActivateCheckPoint();
            GameManager.instance.ActiveCheckPoint(this);
        }
    }

    [ContextMenu("生成检查点ID")]
    private void GenerateID()
    {
        checkPointID = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            canActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            canActive = false;
        }
    }
    public void ActivateCheckPoint()
    {
        activated = true;
        anim.SetBool("active", true);

    }

}
