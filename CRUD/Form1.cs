using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

using Dapper;

namespace CRUD
{
    public partial class CRUD : MetroFramework.Forms.MetroForm
    {
        EntityState objstate = EntityState.Unchanged;
        public CRUD()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|JPG|*.jpg|PNG|*.png", ValidateNames = true })
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    Customer obj = customerBindingSource1.Current as Customer;
                    if (obj != null)
                        obj.ImageUrl1 = ofd.FileName;
                }
            }
        }

        void ClearInput()
        {
            txtUserName.Text = null;
            txtUserPseudo.Text = null;
            txtBirthday.Text = null;
            txtUserAdress.Text = null;
            chkGender.Checked = false;
            pictureBox1.Image = null;

        }

        private void CRUD_Load(object sender, EventArgs e)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    customerBindingSource1.DataSource = db.Query<Customer>("select *from Customers", commandType: CommandType.Text);
                    pContainer.Enabled = false;
                    Customer obj = customerBindingSource1.Current as Customer;
                    if (obj != null)
                    {
                        if (!string.IsNullOrEmpty(obj.ImageUrl1))
                            pictureBox1.Image = Image.FromFile(obj.ImageUrl1);
                    }
                }
            }
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            objstate = EntityState.Added;
            pictureBox1.Image = null;
            pContainer.Enabled = true;
            customerBindingSource1.Add(new Customer());
            customerBindingSource1.MoveLast();
            txtUserName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pContainer.Enabled = false;
            customerBindingSource1.ResetBindings(false);
            // ClearInput();
            this.CRUD_Load(sender, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            objstate = EntityState.Changed;
            pContainer.Enabled = true;
            txtUserName.Focus();
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Customer obj = customerBindingSource1.Current as Customer;
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.ImageUrl1))
                        pictureBox1.Image = Image.FromFile(obj.ImageUrl1);
                }
            }
            catch(Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            objstate = EntityState.Deleted;
            if (MetroFramework.MetroMessageBox.Show(this, "La suppression est définitive, veuillez confirmer !", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {


                try
                {
                    Customer obj = customerBindingSource1.Current as Customer;
                    if (obj != null)
                    {
                        using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                        {
                            if (db.State == ConnectionState.Closed)
                                db.Open();
                            int result = db.Execute("delete from Customers where Id = @Id", new { Id = obj.Id1 }, commandType: CommandType.Text);
                            if (result != 0)
                            {
                                customerBindingSource1.RemoveCurrent();
                                pContainer.Enabled = false;
                                pictureBox1.Image = null;
                                objstate = EntityState.Unchanged;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                customerBindingSource1.EndEdit();
                Customer obj = customerBindingSource1.Current as Customer;
                if (obj != null)
                {
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();
                        if(objstate == EntityState.Added)
                        {
                            DynamicParameters p = new DynamicParameters();
                            p.Add("@Id",dbType: DbType.Int32, direction: ParameterDirection.Output);
                            p.AddDynamicParams(new { Name = obj.Name1, Pseudo = obj.Pseudo1, Gender = obj.Gender1, BirtDay = obj.BirthDay1 ,Email = obj.Email1, Adress = obj.Adress1, ImageUrl = obj.ImageUrl1 });
                            db.Execute("sp_Customers_Insert", p, commandType: CommandType.StoredProcedure);
                            obj.Id1 = p.Get<int>("@Id");
                        }
                        else if(objstate== EntityState.Changed)
                        {
                            db.Execute("sp_Customers_IUpdate", new {Id = obj.Id1, Name = obj.Name1, Pseudo = obj.Pseudo1, Gender = obj.Gender1, BirtDay = obj.BirthDay1, Email = obj.Email1, Adress = obj.Adress1, ImageUrl = obj.ImageUrl1 },commandType: CommandType.StoredProcedure);

                        }
                        metroGrid1.Refresh();
                        pContainer.Enabled = false;
                        objstate = EntityState.Unchanged;
                    }
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkGender_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkGender.CheckState == CheckState.Unchecked)
            {
                chkGender.Text = "Féminin";
            }
            else if (chkGender.CheckState == CheckState.Checked)
            {
                chkGender.Text = "Masculin";
            }
            else
            {
                chkGender.Text = "WTF !!!";
            }
        }
    }
}
