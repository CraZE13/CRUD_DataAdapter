using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;

namespace DataAdapterPrac
{
    public partial class Form1 : Form
    {
        OleDbConnection con = null;
        OleDbDataAdapter da;
        DataGridView gridView;
        Button btnUpdate;
        DataTable dt;
        public Form1()
        {
            InitializeComponent();
            CreateGridView();
            FillGridView();
            AddUpdateButton();
        }

        private void CreateGridView()
        {
            gridView = new DataGridView()
            {
                Size = new Size(260, 450),
                Location = new Point(20, 50),
            };
            Controls.Add(gridView);
        }

        private void FillGridView()
        {
            try
            {
                con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Anand.Sinha\Documents\ComponentOne Samples\Common\C1NWind.mdb");
                con.Open();

                da = new OleDbDataAdapter()
                {
                    UpdateCommand = new OleDbCommand(@"Update Cities Set City = @City where ID = @ID", con),
                    InsertCommand = new OleDbCommand(@"Insert into Cities values (@ID, @City)", con),
                    DeleteCommand = new OleDbCommand(@"Delete from cities where ID = @ID", con),
                    SelectCommand = new OleDbCommand("Select * From Cities", con),
                };

                da.InsertCommand.Parameters.Add("@ID", OleDbType.Integer, 3, "ID");
                da.InsertCommand.Parameters.Add("@City", OleDbType.Char, 10, "City");
                da.UpdateCommand.Parameters.Add("@City", OleDbType.Char, 10, "City");
                da.UpdateCommand.Parameters.Add("@ID", OleDbType.Integer, 3, "ID");
                da.DeleteCommand.Parameters.Add("@ID", OleDbType.Integer, 3, "ID");

                OleDbDataReader dr = da.SelectCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                gridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void AddUpdateButton()
        {
            btnUpdate = new Button()
            {
                Size = new Size(100, 25),
                Location = new Point(20, 20),
                Text = "Update Database",
            };
            btnUpdate.Click += BtnUpdate_Click;
            Controls.Add(btnUpdate);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                DialogResult result = MessageBox.Show("Are you sure you want to commit changes?", "Update Database", MessageBoxButtons.YesNoCancel);
                
                if (result == DialogResult.Yes)
                {
                    da.Update(dt);
                    dt.AcceptChanges();
                    MessageBox.Show("Updated Database Successfully");
                }
                else
                {
                    dt.RejectChanges();
                    MessageBox.Show("Changed reverted back to original");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
