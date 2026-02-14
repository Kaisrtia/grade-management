using System;
using System.Drawing;
using System.Windows.Forms;
using GradeManagement.Config;
using GradeManagement.Controller;
using GradeManagement.DTO.Request;
using GradeManagement.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeManagement.Forms
{
  public class StudentMenuForm : Form
  {
    private User _currentUser;
    private StudentController _controller;
    
    // UI Components
    private Panel pnlSidebar;
    private Panel pnlContent;
    private Label lblWelcome;
    private Button btnLogout;
    private Button btnGrades;
    private Button btnProfile;
    
    // Grades Controls
    private DataGridView dgvGrades;
    
    // Profile Controls
    private TextBox txtName;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnUpdateProfile;

    public StudentMenuForm(User user)
    {
      _currentUser = user;
      _controller = new StudentController();
      InitializeComponent();
      LoadInitialData();
    }

    private void InitializeComponent()
    {
      // Form settings
      this.Text = "Grade Management - Student Dashboard";
      this.Size = new Size(900, 600);
      this.StartPosition = FormStartPosition.CenterScreen;

      // Sidebar Panel
      pnlSidebar = new Panel
      {
        Dock = DockStyle.Left,
        Width = 200,
        BackColor = Color.FromArgb(45, 45, 48)
      };

      // Content Panel
      pnlContent = new Panel
      {
        Dock = DockStyle.Fill,
        BackColor = Color.White,
        Padding = new Padding(20)
      };

      // Welcome Label
      lblWelcome = new Label
      {
        Text = $"Welcome, {_currentUser.name}",
        Location = new Point(10, 10),
        Size = new Size(180, 40),
        ForeColor = Color.White,
        Font = new Font("Segoe UI", 11, FontStyle.Bold),
        TextAlign = ContentAlignment.MiddleCenter
      };

      // Logout Button
      btnLogout = new Button
      {
        Text = "Logout",
        Location = new Point(10, 520),
        Size = new Size(180, 40),
        BackColor = Color.FromArgb(200, 50, 50),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10)
      };
      btnLogout.Click += BtnLogout_Click;

      // Menu Buttons
      btnGrades = CreateMenuButton("📊 My Grades", 60);
      btnGrades.Click += (s, e) => ShowGradesPanel();
      
      btnProfile = CreateMenuButton("👤 My Profile", 110);
      btnProfile.Click += (s, e) => ShowProfilePanel();
      
      // Add controls to sidebar
      pnlSidebar.Controls.AddRange(new Control[] { lblWelcome, btnGrades, btnProfile, btnLogout });

      // Add panels to form
      this.Controls.Add(pnlContent);
      this.Controls.Add(pnlSidebar);

      // Show grades panel by default
      ShowGradesPanel();
    }

    private Button CreateMenuButton(string text, int y)
    {
      return new Button
      {
        Text = text,
        Location = new Point(10, y),
        Size = new Size(180, 40),
        BackColor = Color.FromArgb(60, 60, 65),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10),
        TextAlign = ContentAlignment.MiddleLeft,
        Cursor = Cursors.Hand
      };
    }

    private async void LoadInitialData()
    {
      await RefreshGrades();
    }

    #region Grades Panel

    private void ShowGradesPanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "My Grades",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      // Refresh Button
      var btnRefresh = new Button
      {
        Text = "Refresh",
        Location = new Point(750, 0),
        Size = new Size(80, 30),
        BackColor = Color.FromArgb(100, 100, 100),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnRefresh.Click += async (s, e) => await RefreshGrades();

      // DataGridView
      dgvGrades = new DataGridView
      {
        Location = new Point(0, 50),
        Size = new Size(840, 480),
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect
      };

      pnlContent.Controls.AddRange(new Control[] { lblTitle, btnRefresh, dgvGrades });
      
      // Load data if not already loaded
      if (dgvGrades.DataSource == null)
      {
        // Fire and forget
        _ = RefreshGrades();
      }
    }

    private async System.Threading.Tasks.Task RefreshGrades()
    {
      try 
      {
          var results = await _controller.GetStudentGrades(_currentUser.id);
          
          if (dgvGrades != null)
          {
            dgvGrades.DataSource = results;
            
            // Hide columns that might not be relevant or duplicated
            if (dgvGrades.Columns["studentId"] != null) dgvGrades.Columns["studentId"].Visible = false;
            if (dgvGrades.Columns["studentName"] != null) dgvGrades.Columns["studentName"].Visible = false;
          }
      }
      catch (Exception ex)
      {
          MessageBox.Show($"Error loading grades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion

    #region Profile Panel

    private void ShowProfilePanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "My Profile",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      var lblUserInfo = new Label
      {
        Text = "Update your personal information below.",
        Location = new Point(0, 40),
        Size = new Size(500, 20),
        ForeColor = Color.Gray
      };

      // Username (Read-only)
      var lblUsername = new Label { Text = "Username:", Location = new Point(0, 80), Size = new Size(100, 25) };
      txtUsername = new TextBox 
      { 
        Location = new Point(110, 80), 
        Size = new Size(300, 25),
        ReadOnly = true,
        Text = _currentUser.username
      };

      // Name
      var lblName = new Label { Text = "Full Name:", Location = new Point(0, 120), Size = new Size(100, 25) };
      txtName = new TextBox 
      { 
        Location = new Point(110, 120), 
        Size = new Size(300, 25),
        Text = _currentUser.name
      };

      // Password
      var lblPassword = new Label { Text = "New Password:", Location = new Point(0, 160), Size = new Size(100, 25) };
      txtPassword = new TextBox 
      { 
        Location = new Point(110, 160), 
        Size = new Size(300, 25),
        UseSystemPasswordChar = true,
        PlaceholderText = "Leave blank to keep current password"
      };

      // Update Button
      btnUpdateProfile = new Button
      {
        Text = "Update Profile",
        Location = new Point(110, 200),
        Size = new Size(150, 35),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };
      btnUpdateProfile.Click += BtnUpdateProfile_Click;

      pnlContent.Controls.AddRange(new Control[] { 
        lblTitle, lblUserInfo, 
        lblUsername, txtUsername, 
        lblName, txtName, 
        lblPassword, txtPassword, 
        btnUpdateProfile 
      });
    }

    private async void BtnUpdateProfile_Click(object sender, EventArgs e)
    {
      string newName = txtName.Text.Trim();
      string newPassword = txtPassword.Text;

      if (string.IsNullOrEmpty(newName))
      {
        MessageBox.Show("Name cannot be empty", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      var request = new UserChangeInfoRequestDTO
      {
        id = _currentUser.id,
        username = _currentUser.username, // Username doesn't change
        name = newName,
        password = newPassword // Can be empty if not changing
      };

      // If password is empty, we must handle it. 
      // However, UserChangeInfoRequestDTO has [Required] on password.
      // Checking DTO: [Required] public string password { get; set; }
      // The current DTO requires password. We should check if we can suppress this or if we need to send current password.
      // But we don't have the current raw password (only hash). 
      // SO: We should either relax the DTO validation or require the user to enter a password to update.
      // Given the prompt "can partially change name and password", let's assume if they leave it blank they don't want to change it.
      // But since the backend DTO validation requires it, we might need to change the DTO or use a different one.
      // Looking at `UserChangeInfoRequestDTO`:
      // [Required(ErrorMessage = "Password cannot be empty")]
      // This is a problem if we want "optional" password update.
      // Let's modify the local Request object to satisfy the validator if it's empty, 
      // OR we update `StudentService.updateStudent` to handle empty password if the DTO allows it.
      // BUT `Service` uses `request.password` to hash.
      
      // FIX: If password is empty in UI, we should probably NOT send it to update, but the DTO requires it.
      // Strategy: Since I cannot easily change the DTO validation annotations without potentially breaking other things (or maybe I should check if it's used elsewhere),
      // I will assume for now that if the user leaves it blank, we DO NOT update it.
      // BUT `StudentService` logic I wrote:
      // if (!string.IsNullOrEmpty(request.password)) { update }
      // So the Service handles empty password.
      // The problem is `ValidationContext` in `updateStudent` or Controller might fail if I use the DTO with validation attributes.
      // The `StudentService` does NOT explicitly call `Validator.TryValidateObject` on the DTO. It calls it on the Entity in `BaseRepository`.
      // The `UserChangeInfoRequestDTO` annotations are likely used by ASP.NET Core automated validation or manual validation if implemented.
      // I am manually calling `updateStudent`.
      
      // Let's pass it as is. If it's empty, we might need a dummy value if strict validation is enforced somewhere I missed.
      // Wait, `UserChangeInfoRequestDTO` has `[Required]`.
      // If I create this object, it's just an object. Validation happens when `Validator` is called.
      // In `StudentService`, I didn't add DTO validation, only Entity validation in Repo.
      // So passing empty password in DTO to Service is fine, as long as Service handles it (which I did).
      
      // However, to be safe and clean, let's allow it.
      
      var result = await _controller.UpdateStudent(request);
      
      if (result.success)
      {
        MessageBox.Show(result.message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _currentUser.name = newName; // Update local user object
        lblWelcome.Text = $"Welcome, {_currentUser.name}";
        txtPassword.Clear(); // Clear password field
      }
      else
      {
        MessageBox.Show(result.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion

    private void BtnLogout_Click(object sender, EventArgs e)
    {
      var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      
      if (result == DialogResult.Yes)
      {
        this.Close();
        // Program.cs loop will generally just close this form. 
        // We need a way to say "Logout" vs "Exit".
        // In Program.cs: `if (loginForm.ShowDialog() == DialogResult.OK)`
        // If we close this form, `Application.Run(mainForm)` ends.
        // Then `Main` ends.
        
        // To restart login, `Program.cs` needs a loop or we restart app.
        // AdminForm uses `Application.Restart()`. I will do the same.
        Application.Restart();
      }
    }
  }
}
