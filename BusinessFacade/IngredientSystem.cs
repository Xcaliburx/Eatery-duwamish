using BusinessRule;
using Common.Data;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class IngredientSystem
    {
        public List<IngredientData> GetIngredientbyRecipeID(int recipeID)
        {
            try
            {
                return new IngredientDB().GetIngredientbyRecipeID(recipeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IngredientData GetIngredientByID(int ingredientID)
        {
            try
            {
                return new IngredientDB().GetIngredientByID(ingredientID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateIngredient(IngredientData ingredient)
        {
            try
            {
                return new IngredientRule().InsertUpdateIngredient(ingredient);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteIngredients(IEnumerable<int> ingredientIDs)
        {
            try
            {
                return new IngredientRule().DeleteIngredients(ingredientIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
