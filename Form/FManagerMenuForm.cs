using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GradeManagement.Config;
using GradeManagement.Controller;
using GradeManagement.DTO.Request;
using GradeManagement.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeManagement.Forms
{
  public class FManagerMenuForm : Form
  {
    private User _currentUser;
    private FManagerController _controller;
    
    // UI Components
    private Panel pnlSidebar;
    private Panel pnlContent;
    private Label lblWelcome;
    private Button btnLogout;
    private Button btnManageGrades;
    
    // Grade Management Controls
    private DataGridView dgvGrades;
    private Button btnRefresh;
    private Button btnEditGrade;
    private TextBox txtSearchStudent;
    private Button btnSearch;

    public FManagerMenuForm(User user)
    {
      _currentUser = user;
      _controller = new FManagerController();
      InitializeComponent();
      LoadInitialData();
    }

    private void InitializeComponent()
    {
      // Form settings
      this.Text = "Grade Management - Faculty Manager Dashboard";
      this.Size = new Size(1000, 600);
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
      btnManageGrades = CreateMenuButton("📊 Manage Grades", 60);
      btnManageGrades.Click += (s, e) => ShowGradeManagementPanel();
      
      // Add controls to sidebar
      pnlSidebar.Controls.AddRange(new Control[] { lblWelcome, btnManageGrades, btnLogout });

      // Add panels to form
      this.Controls.Add(pnlContent);
      this.Controls.Add(pnlSidebar);

      // Show grade management panel by default
      ShowGradeManagementPanel();
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

    #region Grade Management Panel

    private void ShowGradeManagementPanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "Faculty Student Grades",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      var lblSubtitle = new Label
      {
        Text = "View and modify grades for students in your faculty",
        Location = new Point(0, 35),
        Size = new Size(500, 20),
        ForeColor = Color.Gray,
        Font = new Font("Segoe UI", 9)
      };

      // Search Box
      var lblSearch = new Label
      {
        Text = "Search Student:",
        Location = new Point(0, 70),
        Size = new Size(100, 25),
        TextAlign = ContentAlignment.MiddleLeft
      };

      txtSearchStudent = new TextBox
      {
        Location = new Point(110, 70),
        Size = new Size(200, 25),
        PlaceholderText = "Enter student name or ID"
      };
      txtSearchStudent.TextChanged += (s, e) => FilterGrades();

      // Refresh Button
      btnRefresh = new Button
      {
        Text = "🔄 Refresh",
        Location = new Point(650, 70),
        Size = new Size(100, 30),
        BackColor = Color.FromArgb(100, 100, 100),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnRefresh.Click += async (s, e) => await RefreshGrades();

      // Edit Grade Button
      btnEditGrade = new Button
      {
        Text = "✏️ Edit Grade",
        Location = new Point(760, 70),
        Size = new Size(120, 30),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnEditGrade.Click += BtnEditGrade_Click;

      // DataGridView
      dgvGrades = new DataGridView
      {
        Location = new Point(0, 110),
        Size = new Size(880, 430),
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false
      };

      pnlContent.Controls.AddRange(new Control[] { 
        lblTitle, lblSubtitle, lblSearch, txtSearchStudent, 
        btnRefresh, btnEditGrade, dgvGrades 
      });
      
      // Load data if not already loaded
      if (dgvGrades.DataSource == null)
      {
        _ = RefreshGrades();
      }
    }

    private async Task RefreshGrades()
    {
      try 
      {
        var results = await _controller.GetFacultyStudentGrades(_currentUser.id);
        
        if (dgvGrades != null)
        {
          dgvGrades.DataSource = results;
          
          // Configure column headers
          if (dgvGrades.Columns["studentId"] != null)
          {
            dgvGrades.Columns["studentId"].HeaderText = "Student ID";
            dgvGrades.Columns["studentId"].Width = 120;
          }
          if (dgvGrades.Columns["studentName"] != null)
          {
            dgvGrades.Columns["studentName"].HeaderText = "Student Name";
            dgvGrades.Columns["studentName"].Width = 200;
          }
          if (dgvGrades.Columns["courseId"] != null)
          {
            dgvGrades.Columns["courseId"].HeaderText = "Course ID";
            dgvGrades.Columns["courseId"].Width = 120;
          }
          if (dgvGrades.Columns["courseName"] != null)
          {
            dgvGrades.Columns["courseName"].HeaderText = "Course Name";
            dgvGrades.Columns["courseName"].Width = 250;
          }
          if (dgvGrades.Columns["grade"] != null)
          {
            dgvGrades.Columns["grade"].HeaderText = "Grade";
            dgvGrades.Columns["grade"].Width = 100;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error loading grades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void FilterGrades()
    {
      if (dgvGrades.DataSource == null || string.IsNullOrWhiteSpace(txtSearchStudent?.Text))
      {
        return;
      }

      var searchText = txtSearchStudent.Text.ToLower();
      var bindingSource = dgvGrades.DataSource as List<DTO.Response.ResultResponseDTO>;
      
      if (bindingSource != null)
      {
        var filtered = bindingSource.Where(r => 
          r.studentId.ToLower().Contains(searchText) ||
          r.studentName.ToLower().Contains(searchText)
        ).ToList();
        
        dgvGrades.DataSource = filtered;
      }
    }

    private void BtnEditGrade_Click(object sender, EventArgs e)
    {
      if (dgvGrades.SelectedRows.Count == 0)
      {
        MessageBox.Show("Please select a grade record to edit", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      var selectedRow = dgvGrades.SelectedRows[0];
      var studentId = selectedRow.Cells["studentId"].Value?.ToString();
      var studentName = selectedRow.Cells["studentName"].Value?.ToString();
      var courseId = selectedRow.Cells["courseId"].Value?.ToString();
      var courseName = selectedRow.Cells["courseName"].Value?.ToString();
      var currentGrade = Convert.ToSingle(selectedRow.Cells["grade"].Value);

      // Show edit dialog
      var editForm = new EditGradeForm(studentId, studentName, courseId, courseName, currentGrade);
      if (editForm.ShowDialog() == DialogResult.OK)
      {
        UpdateGrade(studentId, courseId, editForm.NewGrade);
      }
    }

    private async void UpdateGrade(string studentId, string courseId, float newGrade)
    {
      try
      {
        var request = new ResultRequestDTO
        {
          sid = studentId,
          cid = courseId,
          grade = newGrade
        };

        var (success, message) = await _controller.UpdateStudentGrade(_currentUser.id, request);
        
        if (success)
        {
          MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          await RefreshGrades();
        }
        else
        {
          MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error updating grade: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion

    private void BtnLogout_Click(object sender, EventArgs e)
    {
      var result = MessageBox.Show(
        "Are you sure you want to logout?",
        "Confirm Logout",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
      );

      if (result == DialogResult.Yes)
      {
        this.Close();
      }
    }
  }

  // Edit Grade Dialog Form
  public class EditGradeForm : Form
  {
    private TextBox txtGrade;
    private Button btnSave;
    private Button btnCancel;

    public float NewGrade { get; private set; }

    public EditGradeForm(string studentId, string studentName, string courseId, string courseName, float currentGrade)
    {
      InitializeEditForm(studentId, studentName, courseId, courseName, currentGrade);
    }

    private void InitializeEditForm(string studentId, string studentName, string courseId, string courseName, float currentGrade)
    {
      this.Text = "Edit Grade";
      this.Size = new Size(400, 250);
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;

      var lblTitle = new Label
      {
        Text = "Edit Student Grade",
        Location = new Point(20, 20),
        Size = new Size(350, 25),
        Font = new Font("Segoe UI", 12, FontStyle.Bold)
      };

      var lblStudent = new Label
      {
        Text = $"Student: {studentName} ({studentId})",
        Location = new Point(20, 55),
        Size = new Size(350, 20)
      };

      var lblCourse = new Label
      {
        Text = $"Course: {courseName} ({courseId})",
        Location = new Point(20, 80),
        Size = new Size(350, 20)
      };

      var lblGrade = new Label
      {
        Text = "New Grade (0-100):",
        Location = new Point(20, 115),
        Size = new Size(120, 25)
      };

      txtGrade = new TextBox
      {
        Location = new Point(150, 115),
        Size = new Size(200, 25),
        Text = currentGrade.ToString("F2")
      };

      btnSave = new Button
      {
        Text = "Save",
        Location = new Point(150, 160),
        Size = new Size(100, 35),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnSave.Click += BtnSave_Click;

      btnCancel = new Button
      {
        Text = "Cancel",
        Location = new Point(260, 160),
        Size = new Size(100, 35),
        BackColor = Color.Gray,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

      this.Controls.AddRange(new Control[] { lblTitle, lblStudent, lblCourse, lblGrade, txtGrade, btnSave, btnCancel });
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
      if (float.TryParse(txtGrade.Text, out float grade))
      {
        if (grade >= 0 && grade <= 100)
        {
          NewGrade = grade;
          this.DialogResult = DialogResult.OK;
          this.Close();
        }
        else
        {
          MessageBox.Show("Grade must be between 0 and 100", "Invalid Grade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
      else
      {
        MessageBox.Show("Please enter a valid numeric grade", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}
