using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace cafeMS
{
    public partial class LoginForm : Form
    {
        private readonly MySqlConnection cn;
        private readonly MySqlCommand cm;

        public LoginForm()
        {
            InitializeComponent();

            cn = new MySqlConnection("server=localhost; user id=root;password=; database=cafems;");
            cm = new MySqlCommand("", cn);
            MakePanelCircular(panel2);
        }
        private void MakePanelCircular(Panel panel)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, panel.Width, panel.Height);
            panel.Region = new System.Drawing.Region(path);
        }

        private void Button1Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTB.Text) || string.IsNullOrEmpty(passwordTb.Text))
            {
                MessageBox.Show("Please enter a username and password.");
                return;
            }
            
            try
				{
				    cn.Open();
				
				    cm.CommandText = "SELECT * FROM user WHERE username=@uname AND password=@upass";
				    cm.Parameters.AddWithValue("@uname", usernameTB.Text);
				    cm.Parameters.AddWithValue("@upass", passwordTb.Text);
				
				    using (MySqlDataReader dr = cm.ExecuteReader())
				    {
				        if (dr.HasRows)
				        {
				            MessageBox.Show("Login Successful.");
				            this.Hide();
				
				            MainForm mainForm = new MainForm();
				            mainForm.Show();
				        }
				        else
				        {
				            cm.Parameters.Clear();
				            cm.CommandText = "SELECT * FROM admin WHERE username=@uname AND password=@upass";
				            cm.Parameters.AddWithValue("@uname", usernameTB.Text);
				            cm.Parameters.AddWithValue("@upass", passwordTb.Text);
				
				            dr.Close(); // close the first data reader
				
				            using (MySqlDataReader dr2 = cm.ExecuteReader())
				            {
				                if (dr2.HasRows)
				                {
				                    MessageBox.Show("Login Successful.");
				                    this.Hide();
				
				                    AdminSection adminSection = new AdminSection();
				                    adminSection.Show();
				                }
				                else
				                {
				                    MessageBox.Show("Incorrect username or password.");
				                }
				            }
				        }
				    }
				}
				catch (MySqlException ex)
				{
				    MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
				}
				finally
				{
				    if (cn != null && cn.State == ConnectionState.Open)
				    {
				        cn.Close();
				    }
				}

        }

        private void Button2Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Label6Click(object sender, EventArgs e)
        {
            RegisterUser rUser = new RegisterUser();
            rUser.Show();
            this.Hide();
        }

        private void Panel2Paint(object sender, PaintEventArgs e)
        {

        }
		
        private void Label3Click(object sender, EventArgs e)
        {
            Fpass fform = new Fpass();
            fform.Show();
            this.Hide();
        }
    }
}
