﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item {
    public ItemData ItData;
    public event Action OnLose;
    protected ClockManager CM;
    public Item() {
       
    }
    /// <summary>
    /// Called when the item is moved into player's inventory.
    /// </summary>
    /// <param name="data"></param>
    public virtual void Obtain(ItemData data) {
        ItData = data;
        CM = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ClockManager>();
    }
    public virtual void EffectByTime(float time) { }
    /// <summary>
    /// Called when the item is removed from player's inventory.
    /// </summary>
    public virtual void Lose() {
        OnLose?.Invoke();
    }
}

/// <summary>
/// This item is not used anymore.
/// </summary>
public class ItemDisk : Item {
    public event Action<string> OnDecode;
    public float InitTime;
    public float decode;
    public string Opdata;
    
    
    public ItemDisk() : base() {
       
    }
    public override void EffectByTime(float time) {
        base.EffectByTime(time);
        /*
        if (Slot == ItemSlot.Analyzer1 || Slot == ItemSlot.Analyzer2) {
            decode = time-InitTime; if (decode >= ItData.value1) { OnDecode?.Invoke(Opdata); Destroy(); }
        }
        */
    }
    

}
/// <summary>
/// Debug Item.
/// </summary>
public class ItemSpeedUp : Item {
    
    EntityMove PlayerMove;
    public ItemSpeedUp() : base() {

    }

    public override void Obtain(ItemData data) {
        base.Obtain(data);

        PlayerMove = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<EntityManager>().Player.GetComponent<EntityMove>();
        PlayerMove.SpeedModifier /= 1.5f;
    }
    public override void Lose() {
        PlayerMove.SpeedModifier *= 1.5f;
        base.Lose();

    }


}

public class ItemWeaponRebar : ItemWeapon {

    


}


public class ItemCollection : MonoBehaviour
{
    public event Action OnItemCollectionInitialized;
    public event Action<Item[]> OnRefreshItems;
    public event Action<int> OnChangeItemSlotAvailability;
    InstructionCollection OC;
    //[SerializeField]ItemPromptUI ItemDestroyPrompt;
    [SerializeField]ItemDataset DataContainer;
    List<Item> Items = new List<Item>();
    int MaximumItemCount = 10;
    int ActivatedSlots;
    
    PlayerController PC;
    ClockManager CM;
    private void Awake() {
        for(int i=0;i<MaximumItemCount;i++)
        Items.Add(null);
        PC = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<PlayerController>();
        CM = PC.GetComponent<ClockManager>();
        //CM.OnClockModified += CM_OnClockModified;
        OC = GameObject.FindGameObjectWithTag("OperatorRack").GetComponent<InstructionCollection>();
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeActivatedSlots(3);
        OnRefreshItems(Items.ToArray());
        AddItem("WeaponRebar");
        OnItemCollectionInitialized?.Invoke();
    }
    /// <summary>
    /// Adds the item corresponding to the name to the inventory.
    /// </summary>
    /// <param name="name"></param>
    public void AddItem(string name) {
        var Disktest = name.Split(' ')[0];
        Item AddedItem;
        ItemData AddedItemData;

        AddedItem = new Item();//If the item class for the specific item is not defined, we will just use the base class.
        if (typeof(Item).Namespace != null) {
            if (Type.GetType(typeof(Item).Namespace + ".Item" + name) != null)
                AddedItem = (Item)Activator.CreateInstance(Type.GetType(typeof(Item).Namespace + ".Item" + name));
        } else {
            if (Type.GetType("Item" + name) != null)
                AddedItem = (Item)Activator.CreateInstance(Type.GetType("Item" + name));
        }
            AddedItemData = DataContainer.Dataset.Find((x) => { return x.Name == name; });
       
        AddedItem.Obtain(AddedItemData);
        PushItem(AddedItem);

    }
    /// <summary>
    /// Subscribe to the destroy event of an item.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="position"></param>
    public void SetDestroyCallback(Item i, int position) {
        i.OnLose += () => {
            Items[position] = null;
            OnRefreshItems(Items.ToArray());
        };
    }
    public void ChangeActivatedSlots(int after) {
        ActivatedSlots = after;
        if (ActivatedSlots < Items.Count) {
            for (int i = ActivatedSlots; i < Items.Count; i++) {
                Items[i]?.Lose();
            }
            Items.RemoveRange(after, Items.Count - after - 1);
        }
        OnChangeItemSlotAvailability?.Invoke(after);
        OnRefreshItems(Items.ToArray());
    }
    /// <summary>
    /// This method is used to move an item from inventory to somewhere else.
    /// Notice that Destroy() is still called.
    /// </summary>
    /// <param name="index"></param>
    public Item PopItem(int index) {
        var i = Items[index];
        i.Lose(); Items[index] = null;

        OnRefreshItems?.Invoke(Items.ToArray());
        return i;
    }
    /// <summary>
    /// This method is used to move an item into inventory.
    /// </summary>
    /// <param name="item"></param>
    public void PushItem(Item item) {

       
        bool flag = true;
        for (int i = 1; i < Items.Count; i++) {
            if (Items[i] == null) {
                Items[i] = item;

                OnRefreshItems(Items.ToArray());
                SetDestroyCallback(item, Items.Count - 1);
                flag = false;
                break;
            }

        }
        if (flag) {

            //Instead of opening Item destroy prompt, the item in the temporary slot will be overwritten without prompt.
            var i = 0;//Temporary slot
            Items[i]?.Lose();
            Items[i] = item;
            SetDestroyCallback(item, i);
            OnRefreshItems(Items.ToArray());
        }
        OnRefreshItems?.Invoke(Items.ToArray());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
