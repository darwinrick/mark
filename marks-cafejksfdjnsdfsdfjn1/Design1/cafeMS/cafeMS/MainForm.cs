/*
 * Created by SharpDevelop.
 * User: Ralph
 * Date: 5/10/2023
 * Time: 12:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;
using System.Drawing.Printing;


namespace cafeMS
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		MySqlConnection cn;
		MySqlCommand cm;
		MySqlDataReader dr;

		public MainForm()
		{
			InitializeComponent();
			cn = new MySqlConnection();
			cn.ConnectionString = "server=localhost; user id=root;password=; database=cafems;";
			dataGridView1.Columns.Add("No", "No");
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Qty", "Qty");
            
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(27, 2, 67);
		    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 255, 255);
		    dataGridView1.BackgroundColor = Color.FromArgb(255, 255, 255);
		    dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(OnCellDoubleClick);
		}
		
		private void UpdateTotal(decimal TotalAmount)
		{
			decimal subtotal = 0;
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                double price = double.Parse(row.Cells["Price"].Value.ToString().Replace(",", ""));
                int quantity = int.Parse(row.Cells["Qty"].Value.ToString());
                subtotal += (decimal)price * quantity;
            }
            total = subtotal;
            label1.Text = total.ToString("#,###,##0.00");
		}

		private void OnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
		    if (e.RowIndex >= 0)
			{
			    if (MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			    {
			        double price = double.Parse(dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString().Replace(",", ""));
			        int quantity = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString());
			        double subtotal = price * quantity;
			        dataGridView1.Rows.RemoveAt(e.RowIndex);
			        decimal newTotal = (decimal)(-subtotal);
			        UpdateTotal(newTotal);
			    }
			}
		}
		
		private PictureBox pic;
		private Label price;
		private Label name;
		void MainFormLoad(object sender, EventArgs e)
		{
			GetData();
			dateLabel.Text = DateTime.Now.ToString();
		}
		
		private void GetData()
		{
			cn.Open();
			
			cm = new MySqlCommand("SELECT image, ID, name, price from item", cn);
			dr = cm.ExecuteReader();
			while(dr.Read())
			{
				long len = dr.GetBytes(0,0, null, 0,0);
				byte[] array = new byte[Convert.ToInt32(len) + 1];
				dr.GetBytes(0,0, array, 0, Convert.ToInt32(len));
				pic = new PictureBox();
				pic.Width = 100;
				pic.Height = 150;
				pic.BackgroundImageLayout = ImageLayout.Zoom;
				pic.BorderStyle = BorderStyle.FixedSingle;
				pic.Tag = dr["ID"].ToString();
				
				price = new Label();
				price.Text = "₱ " + dr["price"].ToString();
				price.BackColor = Color.FromArgb(17, 0, 45);
				price.ForeColor = Color.FromArgb(255, 255, 255);
				price.TextAlign = ContentAlignment.MiddleCenter;
				price.Dock = DockStyle.Top;
				price.Font = new Font("Sequi UI", 12);
				
				
				
				name = new Label();
				name.Text = dr["name"].ToString();
				name.BackColor = Color.FromArgb(17, 0, 45);
				name.ForeColor = Color.FromArgb(255, 255, 255);
				name.TextAlign = ContentAlignment.MiddleCenter;
				name.Dock = DockStyle.Bottom;
				name.Font = new Font("Sequi UI", 9);
				
				
				MemoryStream ms = new MemoryStream(array);
				Bitmap bitmap = new Bitmap(ms);
				pic.BackgroundImage = bitmap;
				
				pic.Controls.Add(name);
				pic.Controls.Add(price);
				flowLayoutPanel1.Controls.Add(pic);
				
				pic.Click += new EventHandler(OnClick);
				
			}
			
			dr.Close();
			cn.Close();
		}
		
		public void OnClick(object sender, EventArgs e)
		{
		    if (sender is PictureBox)
	    {
	        string tag = (sender as PictureBox).Tag.ToString();
	        // rest of the code
		
		    cn.Open();
		
		    cm = new MySqlCommand("SELECT * FROM item WHERE ID LIKE '" + tag + "'", cn);
		    dr = cm.ExecuteReader();
		    dr.Read();
		    if (dr.HasRows)
		    {
		        // Check if the item already exists in the DataGridView
		        bool itemExists = false;
		        foreach (DataGridViewRow row in dataGridView1.Rows)
		        {
		            if (row.Cells["ID"].Value.ToString() == dr["ID"].ToString())
		            {
		                // If the item exists, update the quantity column by incrementing the quantity value
		                int quantity = int.Parse(row.Cells["Qty"].Value.ToString()) + 1;
		                row.Cells["Qty"].Value = quantity;
		                itemExists = true;
		                break;
		            }
		        }
		
		        // If the item does not exist in the DataGridView, add a new row with a quantity of 1
		        if (!itemExists)
		        {
		            dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, dr["ID"].ToString(), dr["name"].ToString(), double.Parse(dr["price"].ToString()).ToString("#,###,00"), 1);
		        }
		        
		    }
		    }
		
		    cn.Close();
		   	decimal subtotal = 0;
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
			    double price = double.Parse(row.Cells["Price"].Value.ToString().Replace(",", ""));
			    int quantity = int.Parse(row.Cells["Qty"].Value.ToString());
			    subtotal += (decimal)price * quantity;
			}
			decimal newTotal = subtotal;
			UpdateTotal(newTotal);
		}
		
		private decimal CalculateTotal()
		{
		    decimal total = 0;
		    foreach (DataGridViewRow row in dataGridView1.Rows)
		    {
		        int quantity = Convert.ToInt32(row.Cells["Qty"].Value);
		        decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
		        total += quantity * price;
		    }
		    return total;
		}
		
		void BtnPayClick(object sender, EventArgs e)
		{
			
			// loop through the rows of the datagridview
	    foreach (DataGridViewRow row in dataGridView1.Rows)
	    {
	        int qty = Convert.ToInt32(row.Cells["Qty"].Value);
	        string item_name = row.Cells["Name"].Value.ToString();
	        cn.Open();
	        {
	        	cm = new MySqlCommand("UPDATE item SET qty = qty - @qty WHERE name = @name", cn);
	            cm.Parameters.AddWithValue("@qty", qty);
	            cm.Parameters.AddWithValue("@name", item_name);
	            cm.ExecuteNonQuery();
	        }
	        cn.Close();
	    }
	    
	    {
			// Concatenate items from DataGridView
			string items = "";
			string quan = "";
			string price = "";
			// Get the ID of the inserted order
			long orderID = cm.LastInsertedId;
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
			    items += row.Cells["Name"].Value.ToString() + ",";
			    quan += row.Cells["Qty"].Value.ToString() + ",";
			    price += row.Cells["Price"].Value.ToString() + ",";
			    
			}
			items = items.TrimEnd(',');
			
			// Insert order details into database history table
			cn.Open();
			cm = new MySqlCommand("INSERT INTO history (itemID, date, qty, price, total, items) VALUES (@order_id, @date, @quantity, @price, @total, @items)", cn);
			cm.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
			cm.Parameters.AddWithValue("@total", label1.Text.Replace(",", ""));
			cm.Parameters.AddWithValue("@items", items);
			cm.Parameters.AddWithValue("@order_id", orderID);
			cm.Parameters.AddWithValue("@quantity", quan);
			cm.Parameters.AddWithValue("@price", price);
			cm.ExecuteNonQuery();
			cn.Close();
	    }

		
		    // create a new PrintDocument object
		    PrintDocument pd = new PrintDocument();
		
		    // set the document name
		    pd.DocumentName = "Order Receipt";
		
		    // handle the PrintPage event to draw the document
		    pd.PrintPage += new PrintPageEventHandler(PrintDocument1PrintPage);
		
		    // display the print dialog
		    PrintPreviewDialog printDialog = new PrintPreviewDialog();
		    printDialog.Document = pd;
		    if (printDialog.ShowDialog() == DialogResult.OK)
		    {
		        // print the document
		        pd.Print();
		    }
		
		    // clear the datagridview
		    dataGridView1.Rows.Clear();
		    label1.Text = "0.00";
		    }
		void PrintDocument1PrintPage(object sender, PrintPageEventArgs e)
			{
			    // set the font and brush
			    Font font = new Font("Arial", 12, FontStyle.Bold);
			    Brush brush = new SolidBrush(Color.Black);
			
			    // set the starting x and y coordinates
			    float x = 50, y = 50;
			
			    // set the string format to center align
			    StringFormat centerAlign = new StringFormat();
			    centerAlign.Alignment = StringAlignment.Center;
			
			    // draw the title
			    e.Graphics.DrawString("Order Receipt", font, brush, e.PageBounds.Width / 2, y, centerAlign);
			    y += 40;
			
			    // draw the column headers
			    e.Graphics.DrawString("Item", font, brush, x, y);
			    e.Graphics.DrawString("Quantity", font, brush, x + 200, y);
			    e.Graphics.DrawString("Price", font, brush, x + 400, y);
			    y += 20;
			
			    // draw a line separator
			    e.Graphics.DrawLine(new Pen(Color.Black), x, y, e.PageBounds.Width - x, y);
			    y += 10;
			
			    // loop through the rows of the datagridview and draw the items
			    foreach (DataGridViewRow row in dataGridView1.Rows)
			    {
			        string item = row.Cells["Name"].Value.ToString();
			        int quantity = Convert.ToInt32(row.Cells["Qty"].Value);
			        decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
			
			        e.Graphics.DrawString(item, font, brush, x, y);
			        e.Graphics.DrawString(quantity.ToString(), font, brush, x + 200, y);
			        e.Graphics.DrawString(price.ToString(), font, brush, x + 400, y);
			        y += 20;
			    }
			
			    // draw a line separator
			    e.Graphics.DrawLine(new Pen(Color.Black), x, y, e.PageBounds.Width - x, y);
			    y += 10;
			
			    // draw the total
			    decimal total = CalculateTotal();
			    e.Graphics.DrawString("Total:", font, brush, x + 200, y);
			    e.Graphics.DrawString(total.ToString(), font, brush, x + 400, y);
			}
		void Label3Click(object sender, EventArgs e)
		{
			LoginForm lform = new LoginForm();
			if (MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
			    lform.Show();
			    this.Hide();
			}
		}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
		
	}
