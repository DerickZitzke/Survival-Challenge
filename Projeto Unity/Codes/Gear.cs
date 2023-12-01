using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type; // Tipo de item
    public float rate; // Taxa de modificação

    // Método para inicializar o equipamento com base nos dados do item
    public void Init(ItemData data)
    {
        // Configurações básicas
        name = "Gear " + data.itemId; // Define o nome do equipamento
        transform.parent = GameManager.instance.player.transform; // Define o pai do objeto como o jogador
        transform.localPosition = Vector3.zero; // Define a posição local como (0, 0, 0)

        // Configuração das propriedades
        type = data.itemType; // Define o tipo do equipamento com base nos dados do item
        rate = data.damages[0]; // Define a taxa de modificação com base no primeiro valor de danos do item
        ApplyGear(); // Aplica as modificações do equipamento
    }

    // Método para atualizar a taxa de modificação do equipamento (nível aumentado)
    public void LevelUp(float rate)
    {
        this.rate = rate; // Define a nova taxa de modificação
        ApplyGear(); // Aplica as modificações do equipamento
    }

    // Método para aplicar modificações no equipamento com base em seu tipo
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Gloves:
                RateUp(); // Modifica a taxa de ataque
                break;
            case ItemData.ItemType.Shoes:
                SpeedUp(); // Modifica a velocidade
                break;
        }
    }

    // Método para aumentar a taxa de ataque
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>(); // Obtém todas as armas equipadas

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // Se for uma arma de corpo a corpo
                    float speed = GameManager.instance.rotationSpeed * Character.WeaponSpeed; // Calcula a velocidade base da arma
                    weapon.speed = speed + (speed * rate); // Aumenta a velocidade com base na taxa
                    break;
                default: // Se não for de corpo a corpo
                    speed = GameManager.instance.fireSpeed * Character.WeaponRate; // Calcula a velocidade base do ataque
                    weapon.speed = speed * (1f - rate); // Ajusta a velocidade com base na taxa
                    break;
            }
        }
    }

    float speed = GameManager.instance.player.speed; // Obtém a velocidade do jogador
    // Método para aumentar a velocidade do jogador (caso o equipamento seja do tipo 'Shoes')
    void SpeedUp()
    {
        GameManager.instance.player.speed = speed + (speed * rate); // Aumenta a velocidade do jogador com base na taxa
    }
}
