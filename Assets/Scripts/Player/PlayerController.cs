using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float speed = 3f;
    [SerializeField] Color highlightColor;

    List<GameObject> collidingWithList = new List<GameObject>();
    GameObject collidingWith;
    Transform item;

    void Update()
    {
        Move();
        Interact();
    }

    void Interact()
    {
        if (this.collidingWith == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (this.collidingWith.tag)
            {
                case "Container":
                    ContainerController container = this.collidingWith.GetComponent<ContainerController>();

                    if (this.item != null && !container.HaveItem())
                    {
                        Transform itemRef = container.GetItemRef();
                        this.item.position = itemRef.position;
                        this.item.parent = itemRef;
                        this.item.localScale = new Vector3(1, 1, 1);
                        container.SetItem(this.item);
                        this.item = null;
                        return;
                    }

                    if (this.item == null && container.HaveItem())
                    {
                        this.item = container.GetItem();
                        this.item.position = this.transform.position;
                        this.item.parent = this.transform;
                        return;
                    }

                    break;
            }
        }
    }

    void Move()
    {
        Vector2 dir = Vector2.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.y = Input.GetAxis("Vertical");
        dir.Normalize();

        GetComponent<Animator>().SetBool("isWalking", dir != Vector2.zero);

        if (dir.x > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        } else if (dir.x < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        GetComponent<Rigidbody2D>().velocity = dir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.collidingWithList.Contains(collision.gameObject)) return;
        this.collidingWithList.Add(collision.gameObject);

        CleanHighlight();
        GetFirstCollider();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!this.collidingWithList.Contains(collision.gameObject)) return;
        this.collidingWithList.Remove(collision.gameObject);

        CleanHighlight();
        if (this.collidingWithList.Count > 0)
        {
            GetFirstCollider();
            return;
        }

        this.collidingWith = null;
    }

    void GetFirstCollider()
    {
        this.collidingWith = this.collidingWithList.First();
        this.collidingWith.GetComponent<SpriteRenderer>().color = this.highlightColor;
    }

    void CleanHighlight()
    {
        if (this.collidingWith != null) this.collidingWith.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
