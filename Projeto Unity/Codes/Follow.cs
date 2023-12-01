using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect; // Referência para o componente RectTransform

    // Awake é chamado quando o script é inicializado
    void Awake()
    {
        // Obtém o componente RectTransform associado ao objeto
        rect = GetComponent<RectTransform>();
    }

    // FixedUpdate é chamado a cada passo de física fixa
    void FixedUpdate()
    {
        // Define a posição do RectTransform baseado na posição do jogador convertida para coordenadas de tela
        // Usa a posição do jogador (GameManager.instance.player.transform.position) e converte para a posição da tela (ScreenPoint)
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
