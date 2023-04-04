using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data){

        //Basix set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.Player.transform;
        transform.localPosition = Vector3.zero;

        //property set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();

    }

    public void LevelUp(float rate){
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear(){
        switch(type){
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;

        }
    }


    void RateUp(){
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons){
            switch(weapon.Id){
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.Speed = 150 + (speed *rate);
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.Speed = speed * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.Player.speed = speed + speed * rate;
    }

}
