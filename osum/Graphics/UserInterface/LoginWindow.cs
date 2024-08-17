using System;
using System.Windows.Forms;

namespace osum.GameModes
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;

        public LoginForm()
        {
            this.Text = "Login";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 200);

            Label lblUsername = new Label() { Left = 20, Top = 20, Text = "Username", Width = 100 };
            txtUsername = new TextBox() { Left = 120, Top = 20, Width = 150 };

            Label lblPassword = new Label() { Left = 20, Top = 60, Text = "Password", Width = 100 };
            txtPassword = new TextBox() { Left = 120, Top = 60, Width = 150, PasswordChar = '*' };

            btnLogin = new Button() { Text = "Login", Left = 120, Width = 70, Top = 100 };
            btnLogin.Click += BtnLogin_Click;

            btnCancel = new Button() { Text = "Cancel", Left = 200, Width = 70, Top = 100 };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancel);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Logged in successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Please enter both username and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
