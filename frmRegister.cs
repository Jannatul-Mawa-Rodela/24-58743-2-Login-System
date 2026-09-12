using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

using System.Security.Cryptography;


namespace Login_and_Register
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private static string myConn =
            ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        //Add for SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();

                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text == "" || txtConPassword.Text == "")
            {
                MessageBox.Show("Username and password fields cannot be empty.", "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtPassword.Text != txtConPassword.Text)
            {
                MessageBox.Show("Passwords do not match, please re-enter.", "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtConPassword.Text = "";
                txtPassword.Focus();
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(myConn))
                {
                    con.Open();

                    // is the username already taken?
                    using (SqlCommand check =
                        new SqlCommand("SELECT COUNT(*) FROM tbl_users WHERE username = @username", con))
                    {

                        check.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("That username is already taken.");
                            txtUsername.Focus();
                            return;
                        }
                    }
                    // insert the new user

                    string register = "INSERT INTO tbl_users (username, password) VALUES (@username," +
                        " @password)";
                    using (SqlCommand cmd = new SqlCommand(register, con))
                    {
                        /*
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                        cmd.ExecuteNonQuery();
                        */

                        //for SHA-256
                        string hashedPassword = HashPassword(txtPassword.Text);

                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Your account has been successfully created.",
                    "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtConPassword.Text = "";
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error:\n\n" + ex.Message);
            }
        }
        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbxShowPas.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtConPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
                txtConPassword.PasswordChar = '•';
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConPassword.Text = "";
            txtUsername.Focus();
        }

        private void clickLogin_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            Application.Exit();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmRegister_Load(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}