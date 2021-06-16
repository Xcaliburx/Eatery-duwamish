using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace EateryDuwamish.UserControl
{
    public partial class PagingControl : System.Web.UI.UserControl
    {
        public delegate void PageIndexChangedHandler(object sender, PageEventArgs e);
        public event PageIndexChangedHandler PageIndexChanged;
        private const int _TotalPageToView = 3;

        public int TotalPageToView
        {
            get { return (int)(ViewState["TotalPageToView"] == null ? _TotalPageToView : ViewState["TotalPageToView"]); }
            set { ViewState["TotalPageToView"] = value; }
        }

        /// <summary>
        /// get and set total page
        /// </summary>
        public int TotalPage
        {
            get { return (int)ViewState["TotalPage"]; }
            set { ViewState["TotalPage"] = value; }
        }

        /// <summary>
        /// get and set current page index.
        /// </summary>
        public int CurrentPageIndex
        {
            get { return (int)ViewState["CurrentPageIndex"]; }
            set { ViewState["CurrentPageIndex"] = value; }
        }

        public int StartPage
        {
            get { return (int)ViewState["StartPage"]; }
            set { ViewState["StartPage"] = value; }
        }

        public int EndPage
        {
            get { return (int)ViewState["EndPage"]; }
            set { ViewState["EndPage"] = value; }
        }

        private int TotalRange
        {
            get { return (TotalPageToView / 2) + 1; }
        }

        public void BindData()
        {
            /*if (TotalPage <= 1)
            {
                this.Visible = false;
                return;
            }
            else
            {
                this.Visible = true;
            }*/
            liFirst.Visible = true;
            liLast.Visible = true;
            if (CurrentPageIndex > TotalPage)
            {
                CurrentPageIndex = TotalPage;
                PageIndexChanged(null, new PageEventArgs(CurrentPageIndex));
                return;
            }

            if (CurrentPageIndex == 1)
            {
                liPrev.Attributes.Add("class", "paginate_button previous disabled");
                lbPrev.Attributes.Add("OnClick", "javascript:return false;");

                liFirst.Attributes.Add("class", "paginate_button active");
                lbFirst.Attributes.Add("OnClick", "javascript:return false;");
            }
            else
            {
                liPrev.Attributes.Add("class", "paginate_button previous");
                lbPrev.Attributes.Add("OnClick", "javascript:return true;");

                liFirst.Attributes.Add("class", "paginate_button");
                lbFirst.Attributes.Add("OnClick", "javascript:return true;");
            }

            if (CurrentPageIndex == TotalPage)
            {
                liNext.Attributes.Add("class", "paginate_button next disabled");
                lbNext.Attributes.Add("OnClick", "javascript:return false;");

                liLast.Attributes.Add("class", "paginate_button active");
                lbLast.Attributes.Add("OnClick", "javascript:return false;");
            }
            else
            {
                liNext.Attributes.Add("class", "paginate_button previous");
                lbNext.Attributes.Add("OnClick", "javascript:return true;");

                liLast.Attributes.Add("class", "paginate_button");
                lbLast.Attributes.Add("OnClick", "javascript:return true;");
            }

            if (TotalPage <= 1)
            {
                liPrev.Attributes.Add("class", "paginate_button previous disabled");
                lbPrev.Attributes.Add("OnClick", "javascript:return false;");

                liFirst.Attributes.Add("class", "paginate_button active");
                lbFirst.Attributes.Add("OnClick", "javascript:return false;");

                liNext.Attributes.Add("class", "paginate_button next disabled");
                lbNext.Attributes.Add("OnClick", "javascript:return false;");

                liLast.Attributes.Add("class", "paginate_button active");
                lbLast.Attributes.Add("OnClick", "javascript:return false;");
                liLast.Visible = false;
            }

            lbFirst.Text = "1";
            lbLast.Text = TotalPage.ToString();

            if (TotalPage > 2)
            {
                StartPage = (CurrentPageIndex - TotalRange == 1) ? 2 : CurrentPageIndex - (TotalRange - 1);
                EndPage = (CurrentPageIndex + TotalRange == TotalPage) ? (TotalPage - 1) : CurrentPageIndex + (TotalRange - 1);

                if (StartPage < 2)
                {
                    StartPage = 2;
                    EndPage = (StartPage + TotalPageToView) > TotalPage ? TotalPage - 1 : (StartPage + TotalPageToView - 1);
                }

                if (EndPage >= TotalPage)
                {
                    EndPage = TotalPage - 1;
                    StartPage = (EndPage - TotalPageToView) < 2 ? 2 : (EndPage - TotalPageToView + 1);
                }

                liNextJump.Visible = (EndPage + 1 == TotalPage) ? false : true;
                liPreviousJump.Visible = (StartPage - 1 == 1) ? false : true;

                ArrayList arr = new ArrayList();
                for (int i = StartPage; i <= EndPage; i++)
                {
                    if (i > TotalPage) break;
                    arr.Add(i);
                }

                rptPaging.Visible = true;
                rptPaging.DataSource = arr;
                rptPaging.DataBind();
            }
            else
            {
                liNextJump.Visible = false;
                liPreviousJump.Visible = false;
                rptPaging.Visible = false;

            }
        }


        protected void rptPaging_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbNo = (LinkButton)e.Item.FindControl("lbNo");
                HtmlGenericControl liPage = (HtmlGenericControl)e.Item.FindControl("liPage");

                if (CurrentPageIndex == Convert.ToInt32(e.Item.DataItem))
                {
                    liPage.Attributes.Add("class", "paginate_button active");
                    lbNo.Text = Convert.ToString(e.Item.DataItem);
                }
                else
                {
                    liPage.Attributes.Add("class", "paginate_button");
                    lbNo.Text = Convert.ToString(e.Item.DataItem);
                    lbNo.CommandArgument = lbNo.Text;
                    lbNo.CommandName = "Goto";
                    lbNo.Visible = true;
                }
            }

        }


        protected void rptPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName.Equals("Goto"))
            {
                CurrentPageIndex = Convert.ToInt32(e.CommandArgument.ToString());
            }

            //trigger event
            if (PageIndexChanged != null)
            {
                PageIndexChanged(source, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }

        protected void lbLast_Click(object senders, EventArgs e)
        {
            CurrentPageIndex = TotalPage;
            if (PageIndexChanged != null)
            {
                PageIndexChanged(senders, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }

        protected void lbFirst_Click(object senders, EventArgs e)
        {
            CurrentPageIndex = 1;
            if (PageIndexChanged != null)
            {
                PageIndexChanged(senders, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }

        protected void lbPrev_Click(object senders, EventArgs e)
        {
            CurrentPageIndex--;
            if (PageIndexChanged != null)
            {
                PageIndexChanged(senders, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }

        protected void lbNext_Click(object senders, EventArgs e)
        {
            CurrentPageIndex++;
            if (PageIndexChanged != null)
            {
                PageIndexChanged(senders, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }

        protected void lbPreviousJump_Click(object senders, EventArgs e)
        {
            CurrentPageIndex = (StartPage - TotalRange) < 1 ? 1 : (StartPage - TotalRange);
            if (PageIndexChanged != null)
            {
                PageIndexChanged(senders, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }

        protected void lbNextJump_Click(object senders, EventArgs e)
        {
            CurrentPageIndex = (EndPage + TotalRange) > TotalPage ? TotalPage : (EndPage + TotalRange);
            if (PageIndexChanged != null)
            {
                PageIndexChanged(senders, new PageEventArgs(CurrentPageIndex));
                BindData();
            }
        }
    }
}