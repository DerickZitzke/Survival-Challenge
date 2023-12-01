using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // A classe Character herda MonoBehaviour e é responsável por definir as propriedades estáticas do personagem
    
    // Propriedade estática Speed que define a velocidade do personagem
    public static float Speed
    {
        // Obtém a velocidade do jogador com base no ID do jogador definido no GameManager
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f;}
    }

    // Propriedade estática WeaponSpeed que define a velocidade da arma do personagem
    public static float WeaponSpeed
    {
        // Obtém a velocidade da arma do jogador com base no ID do jogador definido no GameManager
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f;}
    }

    // Propriedade estática WeaponRate que define a taxa de disparo da arma do personagem
    public static float WeaponRate
    {
        // Obtém a taxa de disparo da arma do jogador com base no ID do jogador definido no GameManager
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f;}
    }

    // Propriedade estática Damage que define o dano causado pelo personagem
    public static float Damage
    {
        // Obtém o dano causado pelo jogador com base no ID do jogador definido no GameManager
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f;}
    }

    // Propriedade estática Count que define a contagem de algum elemento associado ao personagem
    public static int Count
    {
        // Obtém a contagem do jogador com base no ID do jogador definido no GameManager
        get { return GameManager.instance.playerId == 3 ? 1 : 0;}
    }
}
