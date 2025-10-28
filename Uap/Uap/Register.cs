using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Uap
{
    public partial class Register : Form
    {
        string database = "server=localhost;uid=root;database=cukimai;Pwd=;";
        public MySqlConnection koneksi;
        public MySqlCommand cmd;
        public MySqlDataAdapter adp;
        public Register()
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

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        public string GenerateGuidDigitString()
        {
            Guid guid = Guid.NewGuid();
            string digitString = Regex.Replace(guid.ToString(), "[^0-9]", "");
            return digitString;
        }
        private void ExecuteRegistration(string email, string password, string uid)
        {
            string hashedPassword = password;
            string query = "INSERT INTO accounts (email, password, uid) VALUES (@email, @pass, @uid)";

            using (MySqlConnection connection = new MySqlConnection(database))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@pass", hashedPassword);
                    command.Parameters.AddWithValue("@uid", uid); 

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Register Berhasil! Cek email anda untuk verifikasi.");
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Database Error: " + ex.Message);
                    }
                }
            }
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPW.Text;
            string uid = GenerateGuidDigitString();
            ExecuteRegistration(email, password, uid);
            MessageBox.Show("Register Berhasil! Cek email anda untuk verifikasi");
        }
    }
}
