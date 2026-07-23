using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Inst;

    void Awake() 
    {
        if (Singleton.Inst == null)
        {
            Singleton.Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
