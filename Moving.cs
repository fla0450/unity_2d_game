using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public float speed;
    private Vector3 vector;
    public float runSpeed;
    private float applyRunSpeed;
    public int walkCount;
    private int currrentWalkCount;
    private bool canMove = true;
    private bool applayRunFlag=false;
    private Animator animator;
    private BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public Vector3 hhm;
    void Start()
    {
        boxCollider=GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                applyRunSpeed = runSpeed;
                applayRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applayRunFlag = false;
            }
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            RaycastHit2D hit;
            Vector2 start = transform.position;//캐릭터의 현재 위치 값
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;
            animator.SetBool("waking",true);
            if (hit.transform != null)
            {
                break;
            }
            while (currrentWalkCount < walkCount)
            {
                
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }
                if (applayRunFlag)
                {
                    currrentWalkCount++;
                }
                currrentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currrentWalkCount = 0;
            
        }
        animator.SetBool("waking", false);
        canMove = true; 
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        } 

    }
}
