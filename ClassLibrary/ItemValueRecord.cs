using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ItemValueRecord
    {
        public int Id { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public string Date { get; set; }

        public double CalculateValue()
        {
            return Quantity * UnitPrice;
        }

        public ItemValueRecord(string name, string unitMeasure, string quantity, string unitPrice)
        {
            Item = new Item(name, ConvertUnitMeasure(unitMeasure.ToUpper()));
            Quantity = Convert.ToInt32(quantity);
            UnitPrice = Convert.ToDouble(unitPrice);
        }

        public ItemValueRecord(Item item, int quantity, double unitPrice)
        {
            Item = item;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        private UnitMeasure ConvertUnitMeasure(string unitMeasure)
        {
            if (UnitMeasure.KG.ToString().Equals(unitMeasure))
            {
                return UnitMeasure.KG;
            }

            if (UnitMeasure.PIECES.ToString().Equals(unitMeasure))
            {
                return UnitMeasure.PIECES;
            }

            if (UnitMeasure.LITER.ToString().Equals(unitMeasure))
            {
                return UnitMeasure.LITER;
            }

            return UnitMeasure.KG;
        }

        public ItemValueRecord(string name, UnitMeasure unit, int quantity, double unitPrice)
        {
            Item = new Item(name, unit);
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public override string ToString()
        {
            return $"Name: {Item}, Quantity: {Quantity}, Unit: {Item.Unit}, Price: {UnitPrice}, Total value: {CalculateValue()}";
        }
    }
}
