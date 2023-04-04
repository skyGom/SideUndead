using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject 
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]    
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea] // 2줄 이상 넣기
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]  
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] count;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}