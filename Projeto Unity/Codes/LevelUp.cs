using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    // Awake é chamado quando a instância do script está sendo carregada
    void Awake()
    {
        rect = GetComponent<RectTransform>(); // Atribui o RectTransform do objeto atual
        items = GetComponentsInChildren<Item>(true); // Pega todos os componentes 'Item' no filho do objeto atual
    }

    // Exibe a tela de level-up
    public void Show()
    {        
        // Verifica se todos os itens alcançaram o nível máximo
        if (CheckAllItemsMaxLevel())
        {
            // Todos os itens atingiram o nível máximo, então não exibe a tela de level-up
            Hide();
            return;
        }
        // Avança para a próxima tela de level-up
        Next();
        // Configura a escala do RectTransform para exibir a tela
        rect.localScale = Vector3.one;
        // Pausa o GameManager
        GameManager.instance.Stop();
        // Toca o efeito sonoro de level-up
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // Ativa o efeito sonoro do background
        AudioManager.instance.EffectBgm(true);
    }

    // Esconde a tela de level-up
    public void Hide()
    {
        // Configura a escala do RectTransform para esconder a tela
        rect.localScale = Vector3.zero;
        // Resuma o GameManager
        GameManager.instance.Resume();
        // Toca o efeito sonoro de seleção
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // Desativa o efeito sonoro do background
        AudioManager.instance.EffectBgm(false);
    }

    // Seleciona um item específico
    public void Select(int index)
    {
        // Chama a função de clique do item selecionado
        items[index].OnClick();
    }

    // Configura a próxima tela de level-up
    void Next()
    {
        // Esconde todos os itens
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // Array para guardar índices aleatórios
        int[] ran = new int[3];
        
        // Laço para obter três índices aleatórios distintos
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        // Iteração para exibir os itens aleatórios ou esconder se já estiverem no nível máximo
        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            // Verifica se o item já atingiu o nível máximo
            if (ranItem.level == ranItem.data.damages.Length)
            {
                // Esconde a tela de level-up se o item estiver no nível máximo e exibe um aviso
                Hide();
                items[4].gameObject.SetActive(true); // Índice 4 exibe um aviso especial
            }
            else
            {
                // Exibe o item normalmente se não estiver no nível máximo
                ranItem.gameObject.SetActive(true);
            }
        }
    }

    // Verifica se todos os itens estão no nível máximo
    private bool CheckAllItemsMaxLevel()
    {
        foreach (Item item in items)
        {
            // Se qualquer item não estiver no nível máximo, retorna falso
            if (item.level < item.data.damages.Length)
            {
                return false;
            }
        }
        // Todos os itens estão no nível máximo
        return true;
    }
}
