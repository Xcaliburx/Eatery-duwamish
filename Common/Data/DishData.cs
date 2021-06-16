using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class DishData
    {
        private int _dishID;
        public int DishID
        {
            get { return _dishID; }
            set { _dishID = value; }
        }
        private int _dishTypeID;
        public int DishTypeID
        {
            get { return _dishTypeID; }
            set { _dishTypeID = value; }
        }
        private string _dishName;
        public string DishName
        {
            get { return _dishName; }
            set { _dishName = value; }
        }
        private int _dishPrice;
        public int DishPrice
        {
            get { return _dishPrice; }
            set { _dishPrice = value; }
        }
    }
}
