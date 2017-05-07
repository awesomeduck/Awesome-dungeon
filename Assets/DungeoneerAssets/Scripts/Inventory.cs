using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DungeoneerAssets
{
    [Serializable]
    public class Inventory
    {
        public List<ItemSlot> Items { get; set; }
        public List<EquipmentSlot> Equipment { get; set; }

        public Inventory()
        {
            this.Items = new List<ItemSlot>();
            this.Equipment = new List<EquipmentSlot>();
        }

        public void AddEquipment(string name)
        {
            EquipmentDefinition def = EquipmentDefinitions.Equipment.Values.FirstOrDefault(x => x.Name == name);
            if (def != null)
                this.AddEquipment(def);
        }

        public void AddEquipment(EquipmentDefinition def)
        {
            this.Equipment.Add(new EquipmentSlot { Definition = def, IsEquipped = false, Name = def.Name });
        }

        public int GetQuantity(ItemDefinition def)
        {
            ItemSlot item = this.Items.FirstOrDefault(x => x.Definition == def);
            if (item != null)
                return item.Quantity;
            else return 0;
        }

        public EquipmentSlot GetEquipedItem(EquipmentType t)
        {
            return this.Equipment.FirstOrDefault(x => x.IsEquipped && x.Definition.Type == t);
        }

        public void AddItem(string name, int quantity)
        {
            ItemSlot item = this.Items.FirstOrDefault(x => x.Name == name);
            if (item != null)
            {
                item.Quantity += quantity;
                return;
            }

            ItemDefinition itemDef = ItemDefinitions.Items[name];
            if (itemDef != null)
            {
                ItemSlot newItem = new ItemSlot
                {
                    Name = name,
                    Quantity = quantity,
                    Definition = itemDef,
                };
                this.Items.Add(newItem);
                return;
            }
        }

        public void AddItem(ItemDefinition def, int quantity)
        {
            ItemSlot item = this.Items.FirstOrDefault(x => x.Definition == def);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                ItemSlot newSlot = new ItemSlot
                {
                    Name = def.Name,
                    Quantity = quantity,
                    Definition = def
                };
                this.Items.Add(newSlot);
            }
        }

        internal void EquipItem(EquipmentSlot item)
        {
            this.Equipment.Where(x => x.Definition.Type == item.Definition.Type)
                .ToList()
                .ForEach(x => x.IsEquipped = false);

            item.IsEquipped = true;
        }

        public void UnEquipType(EquipmentType t)
        {
            this.Equipment.Where(x => ((EquipmentDefinition)x.Definition).Type == t)
                .ToList()
                .ForEach(x => x.IsEquipped = false);
        }

        internal void CalibrateDefinitions()
        {
            foreach (var item in this.Items)
            {
                if (!string.IsNullOrEmpty(item.Name))
                    item.Definition = ItemDefinitions.Items[item.Name];
            }

            foreach (var item in this.Equipment)
            {
                if (!string.IsNullOrEmpty(item.Name))
                    item.Definition = EquipmentDefinitions.Get(item.Name);
            }
        }
    }

    [Serializable]
    public class InventorySlotBase<T> where T : DefinitionBase
    {
        public string Name { get; set; }
        [XmlIgnore]
        public T Definition { get; set; }
    }

    [Serializable]
    public class ItemSlot : InventorySlotBase<ItemDefinition>
    {
        public int Quantity { get; set; }
    }

    [Serializable]
    public class EquipmentSlot : InventorySlotBase<EquipmentDefinition>
    {
        public bool IsEquipped { get; set; }
    }
}