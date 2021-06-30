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
    public partial class Ingredient : System.Web.UI.Page
    {
        static int RecipeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("Dish.aspx");
                }
                RecipeID = int.Parse(Request.QueryString["id"]);

                ShowNotificationIfExists();
                setTitle(RecipeID);
                LoadIngredientTable(RecipeID);
            }
        }

        #region TITLE MANAGEMENT
        private void setTitle(int RecipeID)
        {
            try
            {
                RecipeData recipe = new RecipeSystem().GetRecipeByID(RecipeID);
                lbRecipeName.Text = recipe.RecipeName;
                txtDescription.Text = recipe.RecipeDescription;
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR LOAD DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region FORM MANAGEMENT
        private void FillForm(IngredientData ingredient)
        {
            hdfIngredientId.Value = ingredient.IngredientID.ToString();
            txtIngredientName.Text = ingredient.IngredientName;
            txtQuantity.Text = ingredient.IngredientQuantity.ToString();
            txtUnit.Text = ingredient.IngredientUnit;
        }
        private void ResetForm()
        {
            hdfIngredientId.Value = String.Empty;
            txtIngredientName.Text = String.Empty;
            txtQuantity.Text = String.Empty;
            txtUnit.Text = String.Empty;
        }
        private IngredientData GetFormData()
        {
            IngredientData ingredient = new IngredientData();
            ingredient.IngredientID = String.IsNullOrEmpty(hdfIngredientId.Value) ? 0 : Convert.ToInt32(hdfIngredientId.Value);
            ingredient.IngredientName = txtIngredientName.Text;
            ingredient.IngredientQuantity = Convert.ToInt32(txtQuantity.Text);
            ingredient.IngredientUnit = txtUnit.Text;
            ingredient.RecipeID = RecipeID;
            return ingredient;
        }
        #endregion

        #region DATA TABLE MANAGEMENT
        private void LoadIngredientTable(int recipeId)
        {
            try
            {
                List<IngredientData> ListIngredient = new IngredientSystem().GetIngredientbyRecipeID(recipeId);
                rptIngredients.DataSource = ListIngredient;
                rptIngredients.DataBind();
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptIngredients_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                IngredientData ingredient = (IngredientData)e.Item.DataItem;
                LinkButton lbIngredientName = (LinkButton)e.Item.FindControl("lbIngredientName");
                Literal litQuantity = (Literal)e.Item.FindControl("litQuantity");
                Literal litUnit = (Literal)e.Item.FindControl("litUnit");

                lbIngredientName.Text = ingredient.IngredientName;
                lbIngredientName.CommandArgument = ingredient.IngredientID.ToString();

                litQuantity.Text = ingredient.IngredientQuantity.ToString();
                litUnit.Text = ingredient.IngredientUnit;

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", ingredient.IngredientID.ToString());
            }
        }

        protected void rptIngredients_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                LinkButton lbIngredientName = (LinkButton)e.Item.FindControl("lbIngredientName");
                Literal litQuantity = (Literal)e.Item.FindControl("litQuantity");
                Literal litUnit = (Literal)e.Item.FindControl("litUnit");

                int ingredientID = Convert.ToInt32(e.CommandArgument.ToString());
                IngredientData ingredient = new IngredientSystem().GetIngredientByID(ingredientID);
                FillForm(new IngredientData
                {
                    IngredientID = ingredient.IngredientID,
                    IngredientName = ingredient.IngredientName,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientUnit = ingredient.IngredientUnit,
                    RecipeID = ingredient.RecipeID
                });
                litFormType.Text = $"UBAH: {lbIngredientName.Text}";
                pnlFormIngredient.Visible = true;
                txtIngredientName.Focus();
            }
        }
        #endregion

        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IngredientData ingredient = GetFormData();
                int rowAffected = new IngredientSystem().InsertUpdateIngredient(ingredient);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect(Request.UrlReferrer.PathAndQuery);
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormIngredient.Visible = true;
            txtIngredientName.Focus();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedIngredients.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new IngredientSystem().DeleteIngredients(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect(Request.UrlReferrer.PathAndQuery);
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }

        private RecipeData GetData()
        {
            RecipeData recipe = new RecipeSystem().GetRecipeByID(RecipeID);

            RecipeData newRecipe = new RecipeData();
            newRecipe.RecipeID = recipe.RecipeID;
            newRecipe.RecipeName = recipe.RecipeName;
            newRecipe.DishID = recipe.DishID;
            newRecipe.RecipeDescription = txtDescription.Text;

            return newRecipe;
        }

        protected void btnUpdateDescription_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeData recipe = GetData();
                int rowAffected = new RecipeSystem().InsertUpdateRecipe(recipe);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect(Request.UrlReferrer.PathAndQuery);
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            txtDescription.Enabled = true;
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifIngredient.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifIngredient.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion
    }
}