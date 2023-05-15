/*
 * Created by SharpDevelop.
 * User: Ralph
 * Date: 5/10/2023
 * Time: 12:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace cafeMS
{
	/// <summary>
	/// Description of AddItem.
	/// </summary>
	public partial class AddItem : Form
	{
		MySqlConnection cn;
		MySqlCommand cm;
		public AddItem()
		{
			InitializeComponent();
			cn = new MySqlConnection();
			cn.ConnectionString = "server=localhost; user id=root;password=; database=cafems;";
		}
		void PictureBox1Click(object sender, EventArgs e)
		{
	
		}
		void TextBox1TextChanged(object sender, EventArgs e)
		{
	
		}
		void Button1Click(object sender, EventArgs e)
		{
			openFileDialog1.Filter = "Image files (*.png) | *.png | (*.jpg) | *.jpg | (*.gif) | *.gif";
			openFileDialog1.ShowDialog();
			pictureBox1.BackgroundImage = Image.FromFile(openFileDialog1.FileName);
		}
		void Button2Click(object sender, EventArgs e)
		{
			try
			{
				MemoryStream ms = new MemoryStream();
				pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				byte[] arrImage = ms.GetBuffer();
				
				cn.Open();
				cm = new MySqlCommand("Insert into item(name, price, qty, image) values(@name, @price, @qty, @image)", cn);
				cm.Parameters.AddWithValue("@name", txtName.Text);
				cm.Parameters.AddWithValue("@price", txtPrice.Text);
				cm.Parameters.AddWithValue("@qty", txtQty.Text);
				cm.Parameters.AddWithValue("@image", arrImage);
				cm.ExecuteNonQuery();
				MessageBox.Show("Item Saved");
				cn.Close();
			}
			
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message,"WARNING",MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			
		}
		void ClosePbClick(object sender, EventArgs e)
		{
			this.Close();
		}
		void TxtPriceKeyPress(object sender, KeyPressEventArgs e)
		{
			if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
			{
				e.Handled = true;
			}
			
			if((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > - 1))
			{
				e.Handled = true;
			}
			
		}
		void TxtQtyKeyPress(object sender, KeyPressEventArgs e)
		{
			if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
			{
				e.Handled = true;
			}
			
			if((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > - 1))
			{
				e.Handled = true;
			}
			
		}
	}
}
