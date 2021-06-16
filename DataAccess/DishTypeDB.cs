using Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFramework;

namespace DataAccess
{
    public class DishTypeDB
    {
        public List<DishTypeData> GetDishTypeList()
        {
            try
            {
                string SpName = "dbo.DishType_Get";
                List<DishTypeData> ListDishType = new List<DishTypeData>();
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                DishTypeData dishType = new DishTypeData();
                                dishType.DishTypeID= Convert.ToInt32(Reader["DishTypeID"]);
                                dishType.DishTypeName = Convert.ToString(Reader["DishTypeName"]);
                                ListDishType.Add(dishType);
                            }
                        }
                    }
                    SqlConn.Close();
                }
                return ListDishType;
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
                string SpName = "dbo.DishType_GetByID";
                DishTypeData dishType = null;
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@DishTypeID", dishTypeID));
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            Reader.Read();
                            dishType = new DishTypeData();
                            dishType.DishTypeID = Convert.ToInt32(Reader["DishTypeID"]);
                            dishType.DishTypeName = Convert.ToString(Reader["DishTypeName"]);
                        }
                    }
                    SqlConn.Close();
                }
                return dishType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
