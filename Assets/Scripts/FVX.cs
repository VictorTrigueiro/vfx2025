using UnityEngine;

public class FVX : MonoBehaviour
{
    [SerializeField] private GameObject polymorphModelPrefab;  // Prefab do animal fofo (ex.: esquilo da Lulu)
    [SerializeField] private float polymorphDuration = 4f;  // Duração do polymorph (4 segundos, como no LoL)
    [SerializeField] private float vfxLifetime = 2f;  // Tempo de vida do VFX (partículas rodam e destroem)

    private GameObject originalModel;  // Armazena o modelo original do enemy
    private MeshRenderer originalRenderer;  // Para restaurar
    private Animator originalAnimator;  // Opcional: Para animações

    public void ApplyPolymorph(GameObject enemy)
    {
        // Pega componentes originais do enemy
        originalModel = enemy;  // Ou o child com o mesh principal
        originalRenderer = enemy.GetComponent<MeshRenderer>();
        originalAnimator = enemy.GetComponent<Animator>();

        if (originalRenderer == null) return;  // Sai se não tiver renderer

        // Troca o modelo: Desativa original e ativa o polymorph
        originalRenderer.enabled = false;  // Esconde o enemy original

        // Instancia o modelo polymorph como filho do enemy (posição exata)
        if (polymorphModelPrefab != null)
        {
            GameObject newModel = Instantiate(polymorphModelPrefab, enemy.transform.position, enemy.transform.rotation, enemy.transform);
            newModel.transform.localPosition = Vector3.zero;  // Exata posição relativa (centro)
            newModel.transform.localRotation = Quaternion.identity;  // Alinhado

            // Opcional: Ativa animação do animal
            if (newModel.GetComponent<Animator>() != null)
            {
                newModel.GetComponent<Animator>().SetTrigger("Polymorph");  // Trigger customizado
            }
        }

        Debug.Log("Polymorph aplicado: Enemy transformado em animal por " + polymorphDuration + "s!");

        // Reverte após duração
        Invoke(nameof(RevertPolymorph), polymorphDuration);

        // Auto-destroi o VFX após lifetime
        Destroy(gameObject, vfxLifetime);
    }

    private void RevertPolymorph()
    {
        // Restaura o modelo original
        if (originalRenderer != null)
        {
            originalRenderer.enabled = true;
        }

        // Remove o modelo polymorph (destrói filhos com tag ou nome específico)
        foreach (Transform child in originalModel.transform)
        {
            if (child.CompareTag("PolymorphModel"))  // Tag no prefab do animal
            {
                Destroy(child.gameObject);
            }
        }

        // Restaura animação se necessário
        if (originalAnimator != null)
        {
            originalAnimator.SetTrigger("Revert");  // Trigger para voltar ao normal
        }

        Debug.Log("Polymorph revertido: Enemy de volta ao normal!");
    }
}