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
    public partial class Dish : System.Web.UI.Page
    {
        protected const string DEFAULT_DDL_VALUE = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowNotificationIfExists();
                LoadDishTable();
                LoadDishTypeDropdown();
            }
        }
        #region FORM MANAGEMENT
        private void FillForm(DishData dish)
        {
            hdfDishId.Value = dish.DishID.ToString();
            ddlDishType.SelectedValue = dish.DishTypeID.ToString();
            txtDishName.Text = dish.DishName;
            txtPrice.Text = dish.DishPrice.ToString();
        }
        private void ResetForm()
        {
            hdfDishId.Value = String.Empty;
            txtDishName.Text = String.Empty;
            ddlDishType.SelectedValue = DEFAULT_DDL_VALUE;
            txtPrice.Text = String.Empty;
        }
        private DishData GetFormData()
        {
            DishData dish = new DishData();
            dish.DishID = String.IsNullOrEmpty(hdfDishId.Value) ? 0 : Convert.ToInt32(hdfDishId.Value);
            dish.DishTypeID = Convert.ToInt32(ddlDishType.SelectedValue);
            dish.DishName = txtDishName.Text;
            dish.DishPrice = Convert.ToInt32(txtPrice.Text);
            return dish;
        }
        private void LoadDishTypeDropdown()
        {
            List<DishTypeData> ListDishType = new DishTypeSystem().GetDishTypeList();
            ddlDishType.DataSource = ListDishType;
            ddlDishType.DataValueField = "DishTypeID";
            ddlDishType.DataTextField = "DishTypeName";
            ddlDishType.DataBind();
            ddlDishType.Items.Insert(0, new ListItem("-- Choose Dish Type --", DEFAULT_DDL_VALUE));
            ddlDishType.SelectedValue = DEFAULT_DDL_VALUE;
        }
        #endregion

        #region DATA TABLE MANAGEMENT
        private void LoadDishTable()
        {
            try
            {
                List<DishData> ListDish = new DishSystem().GetDishList();
                rptDish.DataSource = ListDish;
                rptDish.DataBind();
            }
            catch (Exception ex)
            {
                notifDish.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptDish_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DishData dish = (DishData)e.Item.DataItem;
                LinkButton lbDishName = (LinkButton)e.Item.FindControl("lbDishName");
                Literal litDishType = (Literal)e.Item.FindControl("litDishType");
                Literal litPrice = (Literal)e.Item.FindControl("litPrice");

                lbDishName.Text = dish.DishName;
                lbDishName.CommandArgument = dish.DishID.ToString();

                DishTypeData DishType = new DishTypeSystem().GetDishTypeByID(dish.DishTypeID);
                litDishType.Text = DishType.DishTypeName;
                litPrice.Text = dish.DishPrice.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", dish.DishID.ToString());
            }
        }
        protected void rptDish_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                LinkButton lbDishName = (LinkButton)e.Item.FindControl("lbDishName");
                Literal litDishType = (Literal)e.Item.FindControl("litDishType");
                Literal litPrice = (Literal)e.Item.FindControl("litPrice");

                int dishID = Convert.ToInt32(e.CommandArgument.ToString());
                DishData dish = new DishSystem().GetDishByID(dishID);
                FillForm(new DishData
                {
                    DishID = dish.DishID,
                    DishName = dish.DishName,
                    DishTypeID = dish.DishTypeID,
                    DishPrice = dish.DishPrice
                });
                litFormType.Text = $"UBAH: {lbDishName.Text}";
                pnlFormDish.Visible = true;
                txtDishName.Focus();
            }
        }
        #endregion

        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DishData dish = GetFormData();
                int rowAffected = new DishSystem().InsertUpdateDish(dish);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect("Dish.aspx");
            }
            catch(Exception ex)
            {
                notifDish.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormDish.Visible = true;
            txtDishName.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedDishes.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new DishSystem().DeleteDishes(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect("Dish.aspx");
            }
            catch (Exception ex)
            {
                notifDish.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifDish.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifDish.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion

    }
}