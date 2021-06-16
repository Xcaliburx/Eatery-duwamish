using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EateryDuwamish.UserControl
{
    public partial class NotificationControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Show(string Message, NotificationType Type)
        {
            string TypeString = "info", IconString = "glyphicon glyphicon-info-sign";
            switch (Type)
            {
                case NotificationType.Success:
                    TypeString = "success";
                    IconString = "glyphicon glyphicon glyphicon-ok-sign";
                    break;
                case NotificationType.Info:
                    TypeString = "info";
                    IconString = "glyphicon glyphicon-info-sign";
                    break;
                case NotificationType.Warning:
                    TypeString = "warning";
                    IconString = "glyphicon glyphicon-exclamation-sign";
                    break;
                case NotificationType.Danger:
                    TypeString = "danger";
                    IconString = "glyphicon glyphicon-exclamation-sign";
                    break;
            }
            //Message = "$(document).ready(function(){$('#" + pnlNotification.ClientID + "').notify({message: { text: '" + Message + "', type: '" + TypeString + "' }}).show();});";
            Message = Message.Replace(System.Environment.NewLine, " ");
            Message = String.Format(@"$(document).ready(function(){{
                                        $.growl({{
                                            icon: '{0}',
                                            message: '&nbsp;&nbsp;{1}'
                                        }}, {{                                             
                                            type: '{2}',
                                            placement: {{
		                                        from: 'bottom',
		                                        align: 'right'
	                                        }}
                                        }});
                                    }});", IconString, Message, TypeString);
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), Message, true);
        }
    }
}