using System;
using System.Drawing;
using System.Windows.Forms;
using GradeManagement.Config;
using GradeManagement.DTO.Request;
using GradeManagement.Entity;
using GradeManagement.Service;

namespace GradeManagement
{
  public class LoginForm : Form
  {
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;
    private Label lblUsername;
    private Label lblPassword;
    private Label lblTitle;
    private CheckBox chkShowPassword;

    public User? LoggedInUser { get; private set; }
    public bool LoginSuccessful { get; private set; }

    public LoginForm()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      // Form settings
      this.Text = "Grade Management - Login";
      this.Size = new Size(400, 300);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;

      // Title label
      lblTitle = new Label();
      lblTitle.Text = "Grade Management System";
      lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
      lblTitle.Location = new Point(50, 20);
      lblTitle.AutoSize = true;

      // Username label
      lblUsername = new Label();
      lblUsername.Text = "Username:";
      lblUsername.Location = new Point(50, 80);
      lblUsername.AutoSize = true;

      // Username textbox
      txtUsername = new TextBox();
      txtUsername.Location = new Point(50, 105);
      txtUsername.Size = new Size(280, 25);
      txtUsername.Font = new Font("Segoe UI", 10);

      // Password label
      lblPassword = new Label();
      lblPassword.Text = "Password:";
      lblPassword.Location = new Point(50, 140);
      lblPassword.AutoSize = true;

      // Password textbox
      txtPassword = new TextBox();
      txtPassword.Location = new Point(50, 165);
      txtPassword.Size = new Size(280, 25);
      txtPassword.Font = new Font("Segoe UI", 10);
      txtPassword.UseSystemPasswordChar = true;

      // Show password checkbox
      chkShowPassword = new CheckBox();
      chkShowPassword.Text = "Show password";
      chkShowPassword.Location = new Point(50, 195);
      chkShowPassword.AutoSize = true;
      chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

      // Login button
      btnLogin = new Button();
      btnLogin.Text = "Login";
      btnLogin.Location = new Point(130, 220);
      btnLogin.Size = new Size(120, 35);
      btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
      btnLogin.BackColor = Color.FromArgb(0, 120, 215);
      btnLogin.ForeColor = Color.White;
      btnLogin.FlatStyle = FlatStyle.Flat;
      btnLogin.Cursor = Cursors.Hand;
      btnLogin.Click += BtnLogin_Click;

      // Add controls to form
      this.Controls.Add(lblTitle);
      this.Controls.Add(lblUsername);
      this.Controls.Add(txtUsername);
      this.Controls.Add(lblPassword);
      this.Controls.Add(txtPassword);
      this.Controls.Add(chkShowPassword);
      this.Controls.Add(btnLogin);

      // Handle Enter key for login
      txtPassword.KeyPress += TxtPassword_KeyPress;
      txtUsername.KeyPress += TxtPassword_KeyPress;
    }

    private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
    {
      txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
    }

    private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == (char)Keys.Enter)
      {
        BtnLogin_Click(sender, e);
      }
    }

    private async void BtnLogin_Click(object sender, EventArgs e)
    {
      string username = txtUsername.Text.Trim();
      string password = txtPassword.Text;

      // Validation
      if (string.IsNullOrEmpty(username))
      {
        MessageBox.Show("Please enter username", "Validation Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtUsername.Focus();
        return;
      }

      if (string.IsNullOrEmpty(password))
      {
        MessageBox.Show("Please enter password", "Validation Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtPassword.Focus();
        return;
      }

      // Disable button during login
      btnLogin.Enabled = false;
      btnLogin.Text = "Logging in...";

      try
      {
        using (var context = new AppDbContext())
        {
          var authService = new AuthenticationService(context, new IdGeneratorService(context));
          var loginRequest = new LoginRequestDTO 
          { 
            username = username, 
            password = password 
          };

          var response = await authService.Login(loginRequest);

          if (response.success && response.userId != null)
          {
            // Create User object from response
            LoggedInUser = new User
            {
              id = response.userId,
              username = response.username ?? "",
              name = response.name ?? "",
              role = response.role ?? Role.STUDENT
            };

            LoginSuccessful = true;
            MessageBox.Show($"Welcome, {response.name}!", "Login Successful", 
              MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
          else
          {
            MessageBox.Show(response.message, "Login Failed", 
              MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtPassword.Clear();
            txtPassword.Focus();
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"An error occurred: {ex.Message}", "Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        btnLogin.Enabled = true;
        btnLogin.Text = "Login";
      }
    }
  }
}
