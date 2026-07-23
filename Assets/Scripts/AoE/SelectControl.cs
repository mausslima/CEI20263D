using System.Collections.Generic;
using UnityEngine;

public class SelectControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 200f;

    // 1. Alterado: Agora usamos uma LISTA para guardar várias tropas
    private List<TroopsMovement> selectedTroops = new List<TroopsMovement>();

    // Variáveis para criar a caixa de seleção (Box Selection)
    private Vector3 startMousePos;
    private bool isDragging;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // === BOTÃO ESQUERDO: SELECIONAR ===
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Se o mouse moveu muito pouco, considera clique simples. Senão, é caixa de seleção.
            if (Vector3.Distance(startMousePos, Input.mousePosition) < 10f)
            {
                SelectByClick();
            }
            else
            {
                SelectByBox();
            }
        }

        // === BOTÃO DIREITO: MOVER ===
        if (Input.GetMouseButtonDown(1))
        {
            MoveTroopsCommand();
        }
    }

    void SelectByClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Troop"))
            {
                TroopsMovement clickedTroop = hit.collider.GetComponent<TroopsMovement>();
                if (clickedTroop != null)
                {
                    // Se não segurar o Shift, limpa a seleção anterior para selecionar só essa
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        DeselectAllTroops();
                    }
                    AddTroopToSelection(clickedTroop);
                }
            }
            else
            {
                // Clicou no chão sem arrastar, limpa a seleção
                DeselectAllTroops();
            }
        }
    }

    void SelectByBox()
    {
        // Se não segurar o Shift, limpa a seleção anterior ao criar uma nova caixa
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DeselectAllTroops();
        }

        // Cria a área do retângulo baseado na posição inicial e final do mouse
        Vector2 min = Vector2.Min(startMousePos, Input.mousePosition);
        Vector2 max = Vector2.Max(startMousePos, Input.mousePosition);
        Rect selectionRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

        // Busca todas as tropas na cena (Para projetos grandes, prefira guardar isso numa lista global em um TroopManager)
        TroopsMovement[] allTroops = FindObjectsByType<TroopsMovement>(FindObjectsSortMode.None);

        foreach (TroopsMovement troop in allTroops)
        {
            // Converte a posição da tropa no mundo 3D para o espaço 2D da tela
            Vector3 screenPos = mainCamera.WorldToScreenPoint(troop.transform.position);

            // Verifica se a posição 2D da tropa está dentro do retângulo desenhado
            if (selectionRect.Contains(screenPos, true))
            {
                AddTroopToSelection(troop);
            }
        }
    }

    void AddTroopToSelection(TroopsMovement troop)
    {
        // Evita adicionar a mesma tropa duas vezes
        if (!selectedTroops.Contains(troop))
        {
            selectedTroops.Add(troop);
            ChangeTroopColor(troop, Color.green);
        }
    }

    void DeselectAllTroops()
    {
        foreach (TroopsMovement troop in selectedTroops)
        {
            ChangeTroopColor(troop, Color.white);
        }
        selectedTroops.Clear();
    }

    void ChangeTroopColor(TroopsMovement troop, Color color)
    {
        Renderer renderer = troop.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void MoveTroopsCommand()
    {
        // Se não há tropas selecionadas, não faz nada
        if (selectedTroops.Count == 0) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, groundLayer))
        {
            // Faz um loop por toda a lista e manda todas se moverem para o ponto
            foreach (TroopsMovement troop in selectedTroops)
            {
                troop.MoveTroops(hit.point);
            }
        }
    }

    // Método nativo do Unity para desenhar elementos simples na tela (UI)
    void OnGUI()
    {
        if (isDragging)
        {
            // O eixo Y na OnGUI do Unity é invertido, então precisamos calcular a diferença
            Rect rect = new Rect(
                startMousePos.x,
                Screen.height - startMousePos.y,
                Input.mousePosition.x - startMousePos.x,
                (Screen.height - Input.mousePosition.y) - (Screen.height - startMousePos.y)
            );

            // Desenha uma caixa semi-transparente verde
            GUI.color = new Color(0, 1, 0, 0.2f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
    }
}
