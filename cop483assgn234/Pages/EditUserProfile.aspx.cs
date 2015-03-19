using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using cop483assgn234;
using cop483assgn234.AppCode;


namespace cop483assgn234.Pages
{
    public partial class EditUserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            SetFormValues();
        }
        protected void SetFormValues()
        {
            MembershipUser mu = Membership.GetUser(Context.User.Identity.Name);
            tbEmail.Text = mu.Email;
        }
        protected void GetFormValues()
        {
            UserProfile up = new UserProfile();
            up.Address = tbAddress.Text;
            up.City = tbCity.Text;
            up.EMail = tbEmail.Text;
            up.FullName = tbFullName.Text;
            up.Phone = tbPhone.Text;
            up.State = tbState.Text;
            up.UserName = Context.User.Identity.Name;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }

}