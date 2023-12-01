using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    // Enumeração para os tipos de item disponíveis
    public enum ItemType { Melee, Range, Gloves, Shoes, Heal }

    // Informações principais do item
    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    // Dados de níveis e aprimoramentos do item
    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    // Dados específicos para armas
    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
