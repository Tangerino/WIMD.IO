// confirm project references contains reference to WimdioApiProxy.v2, then you can use it:
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.SampleApp
{
    public partial class Main : Form
    {
        public static IWimdioApiClient ApiClient;

        public Main()
        {
            InitializeComponent();

            // this will be created only once when Main form get created
            ApiClient = new WimdioApiClient();
        }

        // this method is called from this Form.Dispose
        // i.e. every time the Main form get closed
        protected void DisposeWimdioApiClient()
        {
            ApiClient.Logout();
        }

        private async void btnLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                // same button is responsible for login and logout
                if (txtUsername.Enabled)
                {
                    var credentials = new Credentials { Email = txtUsername.Text, Password = txtPassword.Text };
                    await ApiClient.Login(credentials);
                    UpdateControlsAppearance(true);
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    await ApiClient.Logout();
                    UpdateControlsAppearance(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex != 1)
                return;

            await UpdateUserList(!txtUsername.Enabled);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            Permission permissions = 0;
            for (var i = 0; i < clbPermissions.Items.Count; i++)
            {
                var permission = clbPermissions.GetSelected(i)
                               ? (Permission)Enum.Parse(typeof(Permission), clbPermissions.Items[i].ToString(), true)
                               : 0;
                permissions |= permission;
            }

            var newUser = new NewUser
            {
                Email = txtUserEmail.Text,
                Password = txtUserPassword.Text,
                FirstName = txtFirstname.Text,
                LastName = txtLastname.Text,
                Permissions = permissions
            };

            try
            {
                var user = await ApiClient.CreateUser(newUser);

                await UpdateUserList(true);

                if (user == null)
                    throw new Exception("No error returned, but user has not been created");

                lbxUsers.SelectedValue = user?.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lbxUsers.SelectedValue == null)
                return;

            Permission permissions = 0;
            for (var i = 0; i < clbPermissions.Items.Count; i++)
                permissions |= (Permission)Enum.Parse(typeof(Permission), clbPermissions.Items[i].ToString(), true);

            try
            {
                var user = await ApiClient.ReadUser((Guid)lbxUsers.SelectedValue);

                var updateUser = new UpdateUser(user)
                {
                    FirstName = txtFirstname.Text,
                    LastName = txtLastname.Text,
                };

                user = await ApiClient.UpdateUser(user.Id, updateUser);

                lbxUsers.SelectedValue = user.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxUsers.SelectedValue == null)
                return;

            try
            {
                await ApiClient.DeleteUser((Guid)lbxUsers.SelectedValue);

                await UpdateUserList(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void lbxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxUsers.SelectedValue == null)
                return;

            try
            {
                var user = await ApiClient.ReadUser((Guid)lbxUsers.SelectedValue);

                txtUserEmail.Text = user.Email;
                txtUserPassword.Text = user.Password;
                txtFirstname.Text = user.FirstName;
                txtLastname.Text = user.LastName;

                for (var i = 0; i < clbPermissions.Items.Count; i++)
                {
                    var state = ((user.Permissions & (Permission)Enum.Parse(typeof(Permission), clbPermissions.Items[i].ToString(), true)) == 0)
                              ? CheckState.Unchecked
                              : CheckState.Checked;
                    clbPermissions.SetItemCheckState(i, state); 
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateControlsAppearance(bool isLoggedIn)
        {
            if (!isLoggedIn)
            {
                txtUsername.Enabled = true;

                txtPassword.Text = null;
                txtPassword.Enabled = true;

                btnLogin.Text = "Login";
            }
            else
            {
                txtUsername.Enabled = false;

                txtPassword.Text = null;
                txtPassword.Enabled = false;

                btnLogin.Text = "Logout";
            }
        }

        private async Task UpdateUserList(bool isLoggedIn)
        {
            lbxUsers.DataSource = null;
            txtUserEmail.Clear();
            txtUserPassword.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();

            if (!isLoggedIn)
                return;

            try
            {
                var users = await ApiClient.ReadUsers();
                lbxUsers.DataSource = users;
                lbxUsers.DisplayMember = "Email";
                lbxUsers.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
