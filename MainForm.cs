using System;
using System.Windows.Forms;
using GradeManagement.Config;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace GradeManagement
{
  public class MainForm : Form
  {
    private Button btnCheckConnection;

    public MainForm()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.btnCheckConnection = new Button();
      this.SuspendLayout();

      // 
      // btnCheckConnection
      // 
      this.btnCheckConnection.Location = new System.Drawing.Point(50, 50);
      this.btnCheckConnection.Name = "btnCheckConnection";
      this.btnCheckConnection.Size = new System.Drawing.Size(200, 50);
      this.btnCheckConnection.TabIndex = 0;
      this.btnCheckConnection.Text = "Check DB Connection";
      this.btnCheckConnection.Click += new System.EventHandler(this.BtnCheckConnection_Click);

      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(300, 200);
      this.Controls.Add(this.btnCheckConnection);
      this.Name = "MainForm";
      this.Text = "Grade Management";
      this.ResumeLayout(false);
    }

    private async void BtnCheckConnection_Click(object sender, EventArgs e)
    {
      try
      {
        using (MySqlConnection conn = DatabaseConfig.GetConnection())
        {
          await conn.OpenAsync();
          MessageBox.Show("Database connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Database connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
