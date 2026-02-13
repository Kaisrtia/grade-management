using System;
using System.Windows.Forms;
using GradeManagement.Config;
using GradeManagement.Entity;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace GradeManagement
{
  public class MainForm : Form
  {
    private Button btnCheckConnection;
    private Label lblWelcome;
    private User loggedInUser;

    public MainForm(User user)
    {
      this.loggedInUser = user;
      InitializeComponent();
      DisplayWelcomeMessage();
    }

    private void InitializeComponent()
    {
      this.btnCheckConnection = new Button();
      this.lblWelcome = new Label();
      this.SuspendLayout();

      // 
      // lblWelcome
      // 
      this.lblWelcome.AutoSize = true;
      this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
      this.lblWelcome.Location = new System.Drawing.Point(50, 20);
      this.lblWelcome.Name = "lblWelcome";
      this.lblWelcome.Size = new System.Drawing.Size(200, 21);
      this.lblWelcome.Text = "Welcome!";

      // 
      // btnCheckConnection
      // 
      this.btnCheckConnection.Location = new System.Drawing.Point(50, 80);
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
      this.ClientSize = new System.Drawing.Size(500, 300);
      this.Controls.Add(this.lblWelcome);
      this.Controls.Add(this.btnCheckConnection);
      this.Name = "MainForm";
      this.Text = "Grade Management System";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void DisplayWelcomeMessage()
    {
      lblWelcome.Text = $"Welcome, {loggedInUser.name}! (Role: {loggedInUser.role})";
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
