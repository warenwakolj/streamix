using osum.Online;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;



   

namespace osum.GameModes
    {
        public class LoginForm : Form
        {
            private BanchoClient banchoClient;
        public TextBox sUsername;
        public TextBox sPassword;
        private Button btnLogin;
        private Button btnCancel;
        public LoginForm(BanchoClient client)
            {
                banchoClient = client;

                this.Text = "Login";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 200);

            Label lblUsername = new Label() { Left = 20, Top = 20, Text = "Username", Width = 100 };
            sUsername = new TextBox() { Left = 120, Top = 20, Width = 150 };

            Label lblPassword = new Label() { Left = 20, Top = 60, Text = "Password", Width = 100 };
            sPassword = new TextBox() { Left = 120, Top = 60, Width = 150, PasswordChar = '*' };

            btnLogin = new Button() { Text = "Login", Left = 120, Width = 70, Top = 100 };
            btnLogin.Click += BtnLogin_Click;

            btnCancel = new Button() { Text = "Cancel", Left = 200, Width = 70, Top = 100 };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.Add(lblUsername);
            this.Controls.Add(sUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(sPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancel);
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = sUsername.Text;
            string password = sPassword.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (!banchoClient.IsConnected)
                {
                    if (!banchoClient.Connect())
                    {
                        MessageBox.Show("Failed to connect to the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string hashedPassword = osum.Helpers.Hashmaker.GetMd5Hash(password);
                int loginResult = banchoClient.Login(username, hashedPassword);
                if (loginResult > 0)
                {
                    MessageBox.Show($"Logged in successfully! User ID: {loginResult}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    string errorMessage = GetErrorMessage(loginResult);
                    MessageBox.Show($"Login failed. Error code: {loginResult}\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter both username and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                case -8:
                    return "You need to verify your device on the website.";
                case -7:
                    return "You have reset your password. Please log in again.";
                case -6:
                    return "You are using a test build and are not a supporter.";
                case -5:
                    return "A server-side error occurred.";
                case -4:
                    return "Your account has not been activated yet.";
                case -3:
                    return "Your account has been banned.";
                case -2:
                    return "An update is needed. Please update your client.";
                case -1:
                    return "Incorrect username or password.";
                default:
                    return "An unknown error occurred.";
            }
        }
    }
}
