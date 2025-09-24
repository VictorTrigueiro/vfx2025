using UnityEngine;

public class ToggleMesh : MonoBehaviour
{
    // Variáveis públicas para você poder ajustar no Inspector da Unity
    public float disableTime = 2.0f; // Tempo que o mesh ficará invisível (em segundos)
    public float reEnableTime = 3.0f; // Tempo que o mesh levará para reaparecer (em segundos)

    // Referência ao Mesh Renderer para podermos ativá-lo e desativá-lo
    private MeshRenderer meshRenderer;

    void Start()
    {
        // Pega o componente MeshRenderer no objeto atual
        meshRenderer = GetComponent<MeshRenderer>();
        // Verifica se o componente foi encontrado. Se não, exibe um erro no console.
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer não encontrado no objeto. Verifique se o objeto tem um componente MeshRenderer.");
        }
    }

    void Update()
    {
        // Verifica se a tecla 'R' foi pressionada
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Se o mesh está visível...
            if (meshRenderer.enabled)
            {
                // ...desativa o mesh e agenda a reativação.
                DisableMesh();
            }
        }
    }

    // Método para desativar o mesh.
    void DisableMesh()
    {
        // Desativa o MeshRenderer, tornando o objeto invisível.
        meshRenderer.enabled = false;
        Debug.Log("Mesh desativado. Reativando em " + disableTime + " segundos.");

        // Agendamos o método para reativar o mesh depois do tempo definido.
        Invoke("ReEnableMesh", disableTime);
    }

    // Método para reativar o mesh.
    void ReEnableMesh()
    {
        // Reativa o MeshRenderer, tornando o objeto visível novamente.
        meshRenderer.enabled = true;
        Debug.Log("Mesh reativado.");
    }
}