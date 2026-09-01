using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class CharacterFallRespawn : MonoBehaviour
{
    //limite de queda
    [SerializeField] private float deathY = -10f;

    //ponto de respawn
    [SerializeField] private Transform respawnPoint1;
    [SerializeField] private Transform respawnPoint2;
    [SerializeField] private Transform respawnPoint3;

    //Ponto de respawn selecionado
    [SerializeField, Range(1, 3)] private int selectedRespawnPoint;

    private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y < deathY) { Respawn(); }
    }

    private void Respawn()
    {
        Transform targetPoint = GetSelectedRespawnPoint();
        if (targetPoint == null)
        {
            Debug.LogWarning("no se ha asignado el punto de respawn seleccionado", this);
            return;
        }
        // é importante desativar temporariamente o character controller antes de modificar sua posicao
        characterController.enabled = false;
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
        characterController.enabled = true;
    }


    private Transform GetSelectedRespawnPoint()
    {
        switch (selectedRespawnPoint)
        {
            case 1:
                return respawnPoint1;
            case 2:
                return respawnPoint2;
            case 3: 
                return respawnPoint3;
            default:
                return respawnPoint1;
        }
    }
    
    //permite trocar o spawnpoint atraves de outros scripts, checkpoints, botoes, etc
    public void SetRespawnPoint(int pointNumber)
    {
        if (pointNumber < 1 || pointNumber > 3)
        {
            Debug.LogWarning("El punto de respawn debe estar entre 1 y 3");
            return;
        }

        selectedRespawnPoint = pointNumber;
    }
}
