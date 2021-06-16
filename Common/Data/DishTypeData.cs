using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class DishTypeData
    {
        private int _dishTypeID;
        public int DishTypeID
        {
            get { return _dishTypeID; }
            set { _dishTypeID = value; }
        }
        private string _dishTypeName;
        public string DishTypeName
        {
            get { return _dishTypeName; }
            set { _dishTypeName = value; }
        }
    }
}
