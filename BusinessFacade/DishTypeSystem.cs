using Common.Data;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class DishTypeSystem
    {
        public List<DishTypeData> GetDishTypeList()
        {
            try
            {
                return new DishTypeDB().GetDishTypeList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DishTypeData GetDishTypeByID(int dishTypeID)
        {
            try
            {
                return new DishTypeDB().GetDishTypeByID(dishTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
