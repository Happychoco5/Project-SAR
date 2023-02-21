using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelBar : MonoBehaviour
{
    public Vector3 localScale;
    Animator anim;

    bool active;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            transform.forward = -Camera.main.transform.forward;
        }
    }

    public void StartAnim()
    {
        active = true;
        anim.SetBool("active", true);
        StartCoroutine(startTimer(2));
    }

    IEnumerator startTimer(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("active", false);
        active = false;


    }
}
