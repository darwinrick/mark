
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;

namespace cafeMS
{
	public partial class AdminSection : Form
	{
		MySqlConnection cn;
		MySqlCommand cm;
		MySqlDataReader dr;
		
		
		public AdminSection()
		{
			cn = new MySqlConnection();
			cn.ConnectionString = "server=localhost; user id=root;password=; database=cafems;";
			InitializeComponent();
		}
		
		private void DeleteItem()
		{
		    if (stockDGV.SelectedCells.Count == 0)
			{
			    MessageBox.Show("Please select a user to delete.");
			    return;
			}
			
			int selectedRowIndex = stockDGV.SelectedCells[0].RowIndex;
			int id = (int)stockDGV.Rows[selectedRowIndex].Cells["ID"].Value;
			
			DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo);
			
			if (result == DialogResult.Yes)
			{
			    try
			    {
			        cn.Open();
			        cm = new MySqlCommand("DELETE FROM item WHERE ID=@id", cn);
			        cm.Parameters.AddWithValue("@id", id);
			        int rowsAffected = cm.ExecuteNonQuery();
			
			        if (rowsAffected > 0)
			        {
			            MessageBox.Show("Item deleted successfully.");
			        }
			        else
			        {
			            MessageBox.Show("No Item deleted.");
			        }
			    }
			    catch (Exception ex)
			    {
			        MessageBox.Show(ex.Message);
			    }
			    finally
			    {
			        cn.Close();
			        Populate();
			        clearTb();
			    }
			}
			
		}
		
		private void UpdateItem()
		{
		    if (string.IsNullOrWhiteSpace(nameTb.Text) || string.IsNullOrWhiteSpace(priceTb.Text) || string.IsNullOrWhiteSpace(stocksTb.Text))
		    {
		        MessageBox.Show("Please select an item to update and fill in all fields.");
		        return;
		    }
		
		    int qty;
		    if (!int.TryParse(stocksTb.Text, out qty))
		    {
		        MessageBox.Show("Please enter a valid quantity.");
		        return;
		    }
		
		    decimal price;
		    if (!decimal.TryParse(priceTb.Text, out price))
		    {
		        MessageBox.Show("Please enter a valid price.");
		        return;
		    }
		
		    if (stockDGV.SelectedCells.Count == 0)
		    {
		        MessageBox.Show("Please select an item to update.");
		        return;
		    }
		
		    int selectedRowIndex = stockDGV.SelectedCells[0].RowIndex;
		    int id = (int)stockDGV.Rows[selectedRowIndex].Cells["ID"].Value;
		
		    try
		    {
		        cn.Open();
		        cm = new MySqlCommand("UPDATE item SET name=@name, price=@price, qty=@qty WHERE ID=@id", cn);
		        cm.Parameters.AddWithValue("@id", id);
		        cm.Parameters.AddWithValue("@name", nameTb.Text);
		        cm.Parameters.AddWithValue("@price", price);
		        cm.Parameters.AddWithValue("@qty", qty);
		        int rowsAffected = cm.ExecuteNonQuery();
		
		        if (rowsAffected > 0)
		        {
		            MessageBox.Show("Data updated successfully.");
		        }
		        else
		        {
		            MessageBox.Show("No data updated.");
		        }
		    }
		    catch (Exception ex)
		    {
		        MessageBox.Show(ex.Message);
		    }
		    finally
		    {
		    	cn.Close();
		    	Populate();
		        clearTb();
		    }
		}
		
		void AddPbClick(object sender, EventArgs e)
		{
			AddItem itemFrm = new AddItem();
			itemFrm.ShowDialog();
		}
		
		private void PopulateAdmin()
		{
			cm = new MySqlCommand("SELECT ID, name,password, username, email from admin", cn);
	        cn.Open();
	        using (dr = cm.ExecuteReader())
	        {
	            if (dr.HasRows)
	            {
	                DataTable dt = new DataTable();
	                dt.Load(dr);
	                adminGV.DataSource = dt;
	            }
	        }
	        cn.Close();
		}
		
		private void Populate()
		{
			cm = new MySqlCommand("SELECT ID, name, price, qty from item", cn);
	        cn.Open();
	        using (dr = cm.ExecuteReader())
	        {
	            if (dr.HasRows)
	            {
	                DataTable dt = new DataTable();
	                dt.Load(dr);
	                stockDGV.DataSource = dt;
	            }
	        }
	        cn.Close();
		}
		
		private void PopulateHistory()
		{
			cm = new MySqlCommand("SELECT ID, date, items, price, qty, total from history", cn);
	        cn.Open();
	        using (dr = cm.ExecuteReader())
	        {
	            if (dr.HasRows)
	            {
	                DataTable dt = new DataTable("history");
	                dt.Load(dr);
	                dataGridView2.DataSource = dt;
	            }
	        }
	        cn.Close();
		}
		
		private void users()
		{
			cm = new MySqlCommand("SELECT ID, name, username, password, Email from user", cn);
	        cn.Open();
	        using (dr = cm.ExecuteReader())
	        {
	            if (dr.HasRows)
	            {
	                DataTable dt = new DataTable();
	                dt.Load(dr);
	                usersDGV.DataSource = dt;
	            }
	        }
	        cn.Close();
		}
		                 
		
		private void clearTb()
		{
			nameTb.Text = "";
			priceTb.Text = "";
			stocksTb.Text = "";
		}
		
		void AdminSectionLoad(object sender, EventArgs e)
		{
			Populate();
			PopulateHistory();
			users();
			PopulateAdmin();
		}
		void TabPage1Click(object sender, EventArgs e)
		{
	
		}
		void StockDGVCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
			 if (e.RowIndex >= 0 && e.RowIndex < stockDGV.Rows.Count)
	        {
	            DataGridViewRow row = stockDGV.Rows[e.RowIndex];
	            if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null)
	            {
	                nameTb.Text = row.Cells[1].Value.ToString();
	                priceTb.Text = row.Cells[2].Value.ToString();
	                stocksTb.Text = row.Cells[3].Value.ToString();
	            }
	        }
			
		}
		void Button1Click(object sender, EventArgs e)
		{
			UpdateItem();
		}
		void DataGridView2CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
	
		}
		void Label11Click(object sender, EventArgs e)
		{
			LoginForm lform = new LoginForm();
			if (MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
			    lform.Show();
			    this.Hide();
			}
		}
		void UsersDGVCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.RowIndex < usersDGV.Rows.Count)
	        {
	            DataGridViewRow row = usersDGV.Rows[e.RowIndex];
	            if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null)
	            {
	                unameTb.Text = row.Cells[1].Value.ToString();
	                uUsernameTb.Text = row.Cells[2].Value.ToString();
	                upassTb.Text = row.Cells[3].Value.ToString();
	                uemailTb.Text = row.Cells[4].Value.ToString();
	            }
	        }
		}
		
		
		private void DeleteUser()
		{
			
			if (usersDGV.SelectedCells.Count == 0)
			{
			    MessageBox.Show("Please select a user to delete.");
			    return;
			}
			
			DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
			    int selectedRowIndex = usersDGV.SelectedCells[0].RowIndex;
			    int id = (int)usersDGV.Rows[selectedRowIndex].Cells["ID"].Value;
			
			    try
			    {
			        cn.Open();
			        cm = new MySqlCommand("DELETE FROM user WHERE ID=@id", cn);
			        cm.Parameters.AddWithValue("@id", id);
			        int rowsAffected = cm.ExecuteNonQuery();
			
			        if (rowsAffected > 0)
			        {
			            MessageBox.Show("User deleted successfully.");
			        }
			        else
			        {
			            MessageBox.Show("No user deleted.");
			        }
			    }
			    catch (Exception ex)
			    {
			        MessageBox.Show(ex.Message);
			    }
			    finally
			    {
			        cn.Close();
			        users();
			    }
			}
			
		}
		
		
		void Button3Click(object sender, EventArgs e)
		{
			DeleteUser();
		}
		void PictureBox1Click(object sender, EventArgs e)
		{
			RegisterAdmin admin = new RegisterAdmin();
			admin.Show();
			
		}
		void Button4Click(object sender, EventArgs e)
		{
			
			if (adminGV.SelectedCells.Count == 0)
			{
			    MessageBox.Show("Please select a user to delete.");
			    return;
			}
			
			int selectedRowIndex = adminGV.SelectedCells[0].RowIndex;
			int id = (int)adminGV.Rows[selectedRowIndex].Cells["ID"].Value;
			
			DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
			    try
			    {
			        cn.Open();
			        cm = new MySqlCommand("DELETE FROM admin WHERE ID=@id", cn);
			        cm.Parameters.AddWithValue("@id", id);
			        int rowsAffected = cm.ExecuteNonQuery();
			
			        if (rowsAffected > 0)
			        {
			            MessageBox.Show("User deleted successfully.");
			        }
			        else
			        {
			            MessageBox.Show("No user deleted.");
			        }
			    }
			    catch (Exception ex)
			    {
			        MessageBox.Show(ex.Message);
			    }
			    finally
			    {
			        cn.Close();
			        PopulateAdmin();
			    }
			}
			
		}
		void BtnupdateClick(object sender, EventArgs e)
		{
			
			
			if (string.IsNullOrWhiteSpace(unameTb.Text) || string.IsNullOrWhiteSpace(uUsernameTb.Text) || string.IsNullOrWhiteSpace(upassTb.Text) || string.IsNullOrWhiteSpace(uemailTb.Text))
		    {
		        MessageBox.Show("Please select an item to update and fill in all fields.");
		        return;
		    }
		
		    if (usersDGV.SelectedCells.Count == 0)
		    {
		        MessageBox.Show("Please select an item to update.");
		        return;
		    }
		
		    int selectedRowIndex = usersDGV.SelectedCells[0].RowIndex;
		    int id = (int)usersDGV.Rows[selectedRowIndex].Cells["ID"].Value;
		
		    try
		    {
		        cn.Open();
		        cm = new MySqlCommand("UPDATE user SET name=@name, username=@user, password=@pass, Email=@email WHERE ID=@id", cn);
		        cm.Parameters.AddWithValue("@id", id);
		        cm.Parameters.AddWithValue("@name", unameTb.Text);
		        cm.Parameters.AddWithValue("@user", uUsernameTb.Text);
		        cm.Parameters.AddWithValue("@pass", upassTb.Text);
		        cm.Parameters.AddWithValue("@email", uemailTb.Text);
		        int rowsAffected = cm.ExecuteNonQuery();
		
		        if (rowsAffected > 0)
		        {
		            MessageBox.Show("Data updated successfully.");
		        }
		        else
		        {
		            MessageBox.Show("No data updated.");
		        }
		    }
		    catch (Exception ex)
		    {
		        MessageBox.Show(ex.Message);
		    }
		    finally
		    {
		    	cn.Close();
		    	users();
		    }
			
		}
		void AdminGVCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
			if (e.RowIndex >= 0 && e.RowIndex < adminGV.Rows.Count)
	        {
	            DataGridViewRow row = adminGV.Rows[e.RowIndex];
	            if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null)
	            {
	                anametb.Text = row.Cells[1].Value.ToString();
	                ausertb.Text = row.Cells[2].Value.ToString();
	                apasstb.Text = row.Cells[3].Value.ToString();
	                aemailtb.Text = row.Cells[4].Value.ToString();
	            }
	        }
			
		}
		void Button5Click(object sender, EventArgs e)
		{
			
			if (string.IsNullOrWhiteSpace(anametb.Text) || string.IsNullOrWhiteSpace(ausertb.Text) || string.IsNullOrWhiteSpace(apasstb.Text) || string.IsNullOrWhiteSpace(aemailtb.Text))
		    {
		        MessageBox.Show("Please select an item to update and fill in all fields.");
		        return;
		    }
		
		    if (adminGV.SelectedCells.Count == 0)
		    {
		        MessageBox.Show("Please select an item to update.");
		        return;
		    }
		
		    int selectedRowIndex = adminGV.SelectedCells[0].RowIndex;
		    int id = (int)adminGV.Rows[selectedRowIndex].Cells["ID"].Value;
		
		    try
		    {
		        cn.Open();
		        cm = new MySqlCommand("UPDATE admin SET name=@name, username=@user, password=@pass, Email=@email WHERE ID=@id", cn);
		        cm.Parameters.AddWithValue("@id", id);
		        cm.Parameters.AddWithValue("@name", anametb.Text);
		        cm.Parameters.AddWithValue("@user", ausertb.Text);
		        cm.Parameters.AddWithValue("@pass", apasstb.Text);
		        cm.Parameters.AddWithValue("@email", aemailtb.Text);
		        int rowsAffected = cm.ExecuteNonQuery();
		
		        if (rowsAffected > 0)
		        {
		            MessageBox.Show("Data updated successfully.");
		        }
		        else
		        {
		            MessageBox.Show("No data updated.");
		        }
		    }
		    catch (Exception ex)
		    {
		        MessageBox.Show(ex.Message);
		    }
		    finally
		    {
		    	cn.Close();
		    	PopulateAdmin();
		    }
			
		}
		void Label14Click(object sender, EventArgs e)
		{
	
		}
		void PriceTbKeyPress(object sender, KeyPressEventArgs e)
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
		void StocksTbKeyPress(object sender, KeyPressEventArgs e)
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
		void Button2Click(object sender, EventArgs e)
		{
			DeleteItem();
		}
		
	}
}
