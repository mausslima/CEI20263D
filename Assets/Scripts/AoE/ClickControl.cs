using UnityEngine;

public class ClickControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; //referencia a camera principal
    [SerializeField] private LayerMask groundLayer; //layer do solo para que o raycast detecte apenas o solo
    [SerializeField] private float rayDistance = 200f; //distancia maxima do raycast

    private TroopsMovement troops;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GameObject.FindAnyObjectByType<Camera>();

        if (mainCamera == null) //se nenhuma camera foi assignada, usa a principal
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Ao clicar com o Botão Esquerdo (Selecionar ou Mover)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Lança um raio genérico para ver no que o mouse clicou
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // A) PRIMEIRO VERIFICA: O objeto clicado tem a tag "troop"?
                if (hit.collider.CompareTag("Troop"))
                {
                    TroopsMovement clickedTroop = hit.collider.GetComponent<TroopsMovement>();

                    if (clickedTroop != null)
                    {
                        SelectTroop(clickedTroop);
                        return; // Para o código aqui para não tentar mover no mesmo clique
                    }
                }

                // B) SEGUNDO VERIFICA: Clicou no chão E temos uma tropa selecionada?
                // Verifica se a layer do objeto atingido está no groundLayer
                if (troops != null && IsInLayerMask(hit.collider.gameObject, groundLayer))
                {
                    troops.MoveTroops(hit.point);
                }
            }
        }

        // 2. Ao clicar com o Botão Direito (Cancela a Seleção)
        if (Input.GetMouseButtonDown(1))
        {
            DeselectTroop();
        }
    }

    void SelectTroop(TroopsMovement newTroop)
    {
        // Limpa a tropa selecionada anteriormente
        DeselectTroop();

        troops = newTroop;

        // Feedback visual: Mudar a cor para verde ao selecionar
        Renderer renderer = troops.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }

        Debug.Log("Tropa Selecionada: " + troops.name);
    }
    void DeselectTroop()
    {
        if (troops != null)
        {
            // Restaura a cor original para branco
            Renderer renderer = troops.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white;
            }

            troops = null;
        }
    }
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}
