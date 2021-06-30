using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class RecipeData
    {
        private int _recipeID;
        public int RecipeID
        {
            get { return _recipeID; }
            set { _recipeID = value; }
        }

        private int _dishID;
        public int DishID
        {
            get { return _dishID; }
            set { _dishID = value; }
        }

        private string _recipeName;
        public string RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; }
        }

        private string _recipeDescription;
        public string RecipeDescription
        {
            get { return _recipeDescription; }
            set { _recipeDescription = value; }
        }
    }
}
