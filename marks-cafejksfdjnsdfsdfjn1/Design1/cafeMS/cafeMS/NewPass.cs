/*
 * Created by SharpDevelop.
 * User: Ralph
 * Date: 5/13/2023
 * Time: 12:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

namespace cafeMS
{
	/// <summary>
	/// Description of NewPass.
	/// </summary>
	public partial class NewPass : Form
	{
		MySqlConnection cn;
		MySqlCommand cm;
		public NewPass()
		{
			cn = new MySqlConnection();
			cn.ConnectionString = "server=localhost; user id=root;password=; database=cafems;";
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void Button2Click(object sender, EventArgs e)
		{
			        cn.Open();
			        cm = new MySqlCommand();
			        cm.Connection = cn;
			        cm.CommandText = "UPDATE user SET password=@pass WHERE Email=@email";
			        cm.Parameters.AddWithValue("@pass", textBox2.Text);
			        cm.Parameters.AddWithValue("@email", Fpass.to);
			        cm.ExecuteNonQuery();
			        MessageBox.Show("Password Changed");
			        cn.Close();
			        
			        
			        LoginForm lform = new LoginForm();
			        lform.Show();
			        this.Hide();
		}
		void NewPassLoad(object sender, EventArgs e)
		{
	
		}
	}
}
