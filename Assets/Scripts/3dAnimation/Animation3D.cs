using UnityEngine;

public class Animation3D : MonoBehaviour
{
    Animator playerAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerAnimation.SetBool("IsMov", true);
        }
        else
        {
            playerAnimation.SetBool("IsMov", false);
        }
    }
}
