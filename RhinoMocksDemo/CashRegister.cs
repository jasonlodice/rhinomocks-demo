using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhinoMocksDemo
{
    public class CashRegister
    {
        /// <summary>
        /// A single line item for the sale
        /// </summary>
        private class LineItem
        {
            public string Description { get; set; }
            public double Price { get; set; }

            public LineItem(string description, double price)
            {
                Description = description;
                Price = price;
            }
        }

        /// <summary>
        /// The line items for the current sale
        /// </summary>
        private readonly List<LineItem> _lineItems = new List<LineItem>();

        /// <summary>
        /// Add a new item to the same
        /// </summary>
        /// <param name="description"></param>
        /// <param name="price"></param>
        public void AddItem(string description, double price)
        {
            _lineItems.Add(new LineItem(description, price));
        }

        /// <summary>
        /// Return the sum of all line items for current sale
        /// </summary>
        /// <returns></returns>
        public double CalculateTotal()
        {
            return _lineItems.Sum(x => x.Price);
        }

        /// <summary>
        /// Complete the sale
        /// </summary>
        /// <param name="payment">Cash given by customer</param>
        /// <returns>Amount of change due to the customer</returns>
        public double CompleteSale(double payment)
        {
            var total = CalculateTotal();
            var change = payment - total;

            _lineItems.Clear();

            return change;
        }
    }
}
