using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Variável booleana que determina se a mão é a esquerda
    public bool isLeft;

    // Referência ao componente SpriteRenderer da mão
    public SpriteRenderer spriter;

    // Referência ao componente SpriteRenderer do jogador
    SpriteRenderer player;

    // Posições e rotações para mão direita e esquerda
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0); // Posição local da mão direita
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0); // Posição local da mão direita espelhada
    Quaternion leftRot = Quaternion.Euler(0, 0, -35); // Rotação da mão esquerda
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135); // Rotação da mão esquerda espelhada

    // Awake é chamado quando o script é carregado
    void Awake()
    {
        // Obtém o componente SpriteRenderer do jogador
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    // LateUpdate é chamado após o Update em todos os objetos
    void LateUpdate()
    {
        // Verifica se o jogador está virado
        bool isReverse = player.flipX;

        if (isLeft) // Se a mão for a esquerda
        {
            // Define a rotação da mão com base na direção do jogador
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse; // Inverte a orientação vertical da mão
            spriter.sortingOrder = isReverse ? 4 : 6; // Define a ordem de renderização da mão
        }
        else // Se a mão for a direita
        {
            // Define a posição da mão com base na direção do jogador
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse; // Inverte a orientação horizontal da mão
            spriter.sortingOrder = isReverse ? 6 : 4; // Define a ordem de renderização da mão
        }
    }
}
