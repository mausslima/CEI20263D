using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    GameObject playerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInstance = Instantiate(player, new Vector3(0,10,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
