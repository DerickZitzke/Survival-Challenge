using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Variáveis públicas que armazenam os dados do item, nível, armas e equipamentos associados
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    // Componentes de UI para exibir informações do item
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    // Método chamado quando o objeto é acordado (awake)
    void Awake()
    {
        // Obtém os componentes de UI (imagem e textos)
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    // Método chamado quando o objeto é ativado
    void OnEnable()
    {
        // Atualiza o texto do nível do item e sua descrição com base nos dados do item e nível atual
        textLevel.text = "Lv." + (level + 1);
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Gloves:
            case ItemData.ItemType.Shoes:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    // Método chamado quando o item é clicado
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // Verifica se a arma existe e a inicializa ou aprimora
                if (weapon == null)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else 
                {
                    // Calcula o próximo dano e quantidade com base no nível
                    float nextDamage = data.baseDamage + GameManager.instance.playerDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    // Atualiza o nível da arma
                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;  
            case ItemData.ItemType.Gloves:
            case ItemData.ItemType.Shoes:
                // Verifica se o equipamento existe e o inicializa ou aprimora
                if (gear == null)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    // Calcula a próxima taxa com base no nível
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;  
            case ItemData.ItemType.Heal:
                // Cura o jogador
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;          
        }

        // Desativa o botão se atingir o nível máximo do item
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
