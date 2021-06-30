using BusinessFacade;
using Common.Data;
using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EateryDuwamish
{
    public partial class Recipe : System.Web.UI.Page
    {
        static int DishID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Request.QueryString["id"] == null)
                {
                    Response.Redirect("Dish.aspx");
                }
                DishID = int.Parse(Request.QueryString["id"]);

                ShowNotificationIfExists();
                setTitle(DishID);
                LoadDishTable(DishID);
            }
        }
        #region TITLE MANAGEMENT
        private void setTitle(int DishID)
        {
            try
            {
                DishData dish = new DishSystem().GetDishByID(DishID);
                DishTypeData dishType = new DishTypeSystem().GetDishTypeByID(dish.DishTypeID);
                lbDishName.Text = dish.DishName;
                lbDishType.Text = dishType.DishTypeName;
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR LOAD DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region FORM MANAGEMENT
        private void FillForm(RecipeData recipe)
        {
            hdfRecipeId.Value = recipe.DishID.ToString();
            txtRecipeName.Text = recipe.RecipeName;
        }
        private void ResetForm()
        {
            hdfRecipeId.Value = String.Empty;
            txtRecipeName.Text = String.Empty;
        }
        private RecipeData GetFormData()
        {
            RecipeData recipe = new RecipeData();
            recipe.DishID = String.IsNullOrEmpty(hdfRecipeId.Value) ? 0 : Convert.ToInt32(hdfRecipeId.Value);
            recipe.RecipeName = txtRecipeName.Text;
            recipe.DishID = DishID;
            recipe.RecipeDescription = String.Empty;

            return recipe;
        }
        #endregion

        #region DATA TABLE MANAGEMENT
        private void LoadDishTable(int dishID)
        {
            try
            {
                List<RecipeData> ListRecipe = new RecipeSystem().GetRecipebyDishID(dishID);
                rptRecipe.DataSource = ListRecipe;
                rptRecipe.DataBind();
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptRecipe_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RecipeData recipe = (RecipeData)e.Item.DataItem;
                Literal lbRecipeName = (Literal)e.Item.FindControl("lbRecipeName");
                Button btnDetail = (Button)e.Item.FindControl("btnDetail");

                lbRecipeName.Text = recipe.RecipeName;
                btnDetail.CommandArgument = recipe.RecipeID.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", recipe.RecipeID.ToString());
            }
        }
        protected void rptRecipe_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        #endregion

        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeData recipe = GetFormData();
                int rowAffected = new RecipeSystem().InsertUpdateRecipe(recipe);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect(Request.UrlReferrer.PathAndQuery);
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormRecipe.Visible = true;
            txtRecipeName.Focus();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedRecipes.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new RecipeSystem().DeleteRecipe(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect(Request.UrlReferrer.PathAndQuery);
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = int.Parse(btn.CommandArgument);
            Response.Redirect("./Ingredient.aspx?id=" + id);
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifRecipe.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifRecipe.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion

    }
}