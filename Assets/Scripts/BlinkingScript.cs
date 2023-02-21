using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingScript : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //anim.ResetTrigger("Blink");

            anim.SetTrigger("Blink");
        }
    }

    private IEnumerator Blink()
    {
        float random = Random.RandomRange(0.5f, 2);
        anim.SetTrigger("Blink");
        yield return new WaitForSeconds(random);
        StartCoroutine(Blink());
    }
}
