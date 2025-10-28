using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Uap
{
    public partial class Login : Form
    {
        string database = "server=localhost;uid=root;database=cukimai;Pwd=;";
        public MySqlConnection koneksi;
        public MySqlCommand cmd;
        public MySqlDataAdapter adp;
        public Login()
        {
            InitializeComponent();
        }
        public void fQuery(string squery)
        {
            using (MySqlConnection koneksi = new MySqlConnection(database))
            {
                try
                {
                    koneksi.Open();
                    using (MySqlCommand cmd = new MySqlCommand(squery, koneksi))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ali)
                {
                    MessageBox.Show(ali.Message);
                }
            }
        }

        public void Ambildata()
        {
            //
        }

        private bool ValidateLogin(string email, string plainTextPassword)
            {
                // 1. Initialize variables
                string storedPasswordHash = string.Empty;

                // 2. Query: Select ONLY the password hash based on the email
                // This query is secure because we use parameters.
                string query = "SELECT password FROM accounts WHERE email = @email";

                using (MySqlConnection connection = new MySqlConnection(database))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Use parameter for the email to prevent SQL Injection
                        command.Parameters.AddWithValue("@email", email);

                        try
                        {
                            connection.Open();

                            // ExecuteScalar is efficient for retrieving a single value (the hash)
                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                // Convert the retrieved hash to a C# string
                                storedPasswordHash = result.ToString();
                            }
                            else
                            {
                                // No user found with that email
                                return false;
                            }
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show("A database error occurred.");
                            return false;
                        }
                    }
                }

        // 3. Password Verification (The automatic check)
        if (!string.IsNullOrEmpty(storedPasswordHash))
        {
            // **This is the critical step for checking the password.**
            // You MUST use a library function (like BCrypt.Verify) to compare 
            // the plain text input against the stored hash.

            // Example using BCrypt.Net (Requires NuGet package):
            // return BCrypt.Net.BCrypt.Verify(plainTextPassword, storedPasswordHash);

            // **If you are only using simple comparison (NOT recommended for production):**
            return storedPasswordHash.Equals(plainTextPassword);
        }

        return false;
    }


    private void BtnLogin_Click(object sender, EventArgs e)
        {
            string enteredEmail = txtUsernamelogin.Text;
            string enteredPassword = txtPWlogin.Text;

            if (ValidateLogin(enteredEmail, enteredPassword))
            {
                MessageBox.Show("Login Successful! Welcome.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register f = new Register();
            f.ShowDialog();
        }
    }
}
