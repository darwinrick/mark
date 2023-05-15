/*
 * Created by SharpDevelop.
 * User: Ralph
 * Date: 5/12/2023
 * Time: 2:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;

namespace cafeMS
{
	/// <summary>
	/// Description of RegisterAdmin.
	/// </summary>
	public partial class RegisterAdmin : Form
	{
		MySqlConnection cn;
		MySqlCommand cm;
		public RegisterAdmin()
		{
			cn = new MySqlConnection();
			cn.ConnectionString = "server=localhost; user id=root;password=; database=cafems;";
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			
			if (string.IsNullOrEmpty(UnameTb.Text) || string.IsNullOrEmpty(usernameTb.Text) || string.IsNullOrEmpty(UpasswordTb.Text) || questionCB.SelectedIndex < 0 || string.IsNullOrEmpty(answerTb.Text))
		    {
		        MessageBox.Show("Please fill in all fields.");
		    }
		    else
		    {
		        try
		        {
		            cn.Open();
		            cm = new MySqlCommand("Insert into admin(name, username, password, email, question, answer) values(@name, @uname, @upass, @email, @ques, @ans)", cn);
		            cm.Parameters.AddWithValue("@name", UnameTb.Text);
		            cm.Parameters.AddWithValue("@uname", usernameTb.Text);
		            cm.Parameters.AddWithValue("@upass", UpasswordTb.Text);
		            cm.Parameters.AddWithValue("@email", EmailTb.Text);
		            cm.Parameters.AddWithValue("@ques", questionCB.SelectedIndex >= 0 ? questionCB.SelectedItem.ToString() : (object)DBNull.Value);
		            cm.Parameters.AddWithValue("@ans", answerTb.Text);
		            cm.ExecuteNonQuery();
		            MessageBox.Show("Registered Successfully");
		        }
		        catch (Exception ex)
		        {
		            MessageBox.Show("Error: " + ex.Message);
		        }
		        finally
		        {
		            cn.Close();
		        }
		    }
			
		}
		void Button2Click(object sender, EventArgs e)
		{
			this.Hide();
		}
	}
}
