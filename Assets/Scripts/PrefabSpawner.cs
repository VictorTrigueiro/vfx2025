using UnityEngine;
using System.Collections;

public class SpawnAndTogglePrefabs : MonoBehaviour
{
    // A referência ao Transform do jogador que será a posição de spawn.
    public Transform playerTransform;

    // Atraso antes de começar a spawnar os prefabs.
    public float startDelay = 0f;

    // Atraso entre o spawn de cada prefab.
    public float intervalBetweenSpawns = 2f;

    // Struct para agrupar as informações de cada prefab.
    [System.Serializable]
    public struct PrefabSettings
    {
        public GameObject prefabToSpawn;
        public float disableTime;
        public float reEnableTime;
        // NOVO: Opção para destruir o objeto no final do ciclo.
        public bool destroyAfterCycle;
    }

    // Array de PrefabSettings para podermos configurar vários prefabs no Inspector.
    public PrefabSettings[] prefabsToToggle;

    void Update()
    {
        // Pressionar a tecla 'R' para iniciar o processo de spawn e toggle.
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(SpawnAllPrefabsWithDelay());
        }
    }

    IEnumerator SpawnAllPrefabsWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (PrefabSettings settings in prefabsToToggle)
        {
            if (playerTransform == null)
            {
                Debug.LogError("Player Transform não está definido. Arraste o objeto do jogador para o campo 'Player Transform' no Inspector.");
                yield break;
            }

            GameObject spawnedObject = Instantiate(settings.prefabToSpawn, playerTransform.position, playerTransform.rotation);

            // Passamos a informação sobre destruir o objeto para a coroutine de toggle.
            StartCoroutine(ToggleObjectCoroutine(spawnedObject, settings.disableTime, settings.reEnableTime, settings.destroyAfterCycle));

            yield return new WaitForSeconds(intervalBetweenSpawns);
        }
    }

    // Coroutine para gerenciar o ciclo de ativação/desativação de um único objeto.
    // Agora aceita um parâmetro para saber se deve ser destruído.
    IEnumerator ToggleObjectCoroutine(GameObject obj, float disableTime, float reEnableTime, bool destroyAtEnd)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("O prefab " + obj.name + " não tem um MeshRenderer!");
            yield break;
        }

        // Deixa o objeto visível.
        meshRenderer.enabled = true;

        // Espera pelo tempo de desativação.
        yield return new WaitForSeconds(disableTime);

        // Desativa o mesh (torna-o invisível).
        meshRenderer.enabled = false;

        // Espera pelo tempo de reativação (mas o objeto vai permanecer desativado).
        yield return new WaitForSeconds(reEnableTime);

        // NOVO: Removemos a linha que reativava o mesh. Ele permanece desativado.

        // NOVO: Se a opção de destruir no final estiver marcada, o objeto é destruído.
        if (destroyAtEnd)
        {
            Destroy(obj);
        }
    }
}