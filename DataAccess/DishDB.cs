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
    public class DishDB
    {
        public List<DishData> GetDishList()
        {
            try
            {
                string SpName = "dbo.Dish_Get";
                List<DishData> ListDish = new List<DishData>();
                using(SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    using(SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                DishData dish = new DishData();
                                dish.DishID = Convert.ToInt32(Reader["DishID"]);
                                dish.DishTypeID = Convert.ToInt32(Reader["DishTypeID"]);
                                dish.DishName = Convert.ToString(Reader["DishName"]);
                                dish.DishPrice = Convert.ToInt32(Reader["DishPrice"]);
                                ListDish.Add(dish);
                            }
                        }
                    }
                    SqlConn.Close();
                }
                return ListDish;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DishData GetDishByID(int dishID)
        {
            try
            {
                string SpName = "dbo.Dish_GetByID";
                DishData dish = null;
                using(SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@DishID", dishID));
                    using(SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            Reader.Read();
                            dish = new DishData();
                            dish.DishID = Convert.ToInt32(Reader["DishID"]);
                            dish.DishTypeID = Convert.ToInt32(Reader["DishTypeID"]);
                            dish.DishName = Convert.ToString(Reader["DishName"]);
                            dish.DishPrice = Convert.ToInt32(Reader["DishPrice"]);
                        }
                    }
                    SqlConn.Close();
                }
                return dish;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertUpdateDish(DishData dish, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Dish_InsertUpdate";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter DishId = new SqlParameter("@DishID", dish.DishID);
                DishId.Direction = ParameterDirection.InputOutput;
                SqlCmd.Parameters.Add(DishId);

                SqlCmd.Parameters.Add(new SqlParameter("@DishTypeID", dish.DishTypeID));
                SqlCmd.Parameters.Add(new SqlParameter("@DishName", dish.DishName));
                SqlCmd.Parameters.Add(new SqlParameter("@DishPrice", dish.DishPrice));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteDishes(string dishIDs,SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Dish_Delete";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@DishIDs", dishIDs));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
