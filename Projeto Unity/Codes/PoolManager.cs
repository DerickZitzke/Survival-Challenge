using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Array de GameObjects que serão pré-fabricados
    public GameObject[] prefabs;

    // Lista de listas de GameObjects, usada para armazenar os pools de objetos
    List<GameObject>[] pools;

    void Awake()
    {
        // Inicializa o array de pools com o tamanho igual ao número de prefabs
        pools = new List<GameObject>[prefabs.Length];

        // Itera sobre o array de pools
        for (int index = 0; index < pools.Length; index++)
        {
            // Inicializa cada elemento do array como uma nova lista de GameObjects
            pools[index] = new List<GameObject>();
        }       
    }

    // Método para obter um objeto de um pool específico com base no índice
    public GameObject Get(int index)
    {
        // Inicializa uma referência para o GameObject selecionado
        GameObject select = null;           

        // Itera sobre os objetos no pool selecionado
        foreach (GameObject item in pools[index])
        {
            // Verifica se o item não está ativo na cena
            if (!item.activeSelf)
            {                
                select = item; // Armazena o item inativo para ser reutilizado
                select.SetActive(true); // Ativa o item selecionado
                break; // Sai do loop
            }
        }

        // Se nenhum item inativo foi encontrado no pool
        if (!select)
        {
            // Cria uma nova instância do prefab correspondente ao índice passado
            select = Instantiate(prefabs[index], transform); // Instancia o prefab no PoolManager
            pools[index].Add(select); // Adiciona o novo item ao pool correspondente
        }

        return select; // Retorna o GameObject selecionado (inativo ou recém-criado)
    }
}
