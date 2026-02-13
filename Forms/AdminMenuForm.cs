using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GradeManagement.Config;
using GradeManagement.Controller;
using GradeManagement.Entity;
using System.Collections.Generic;

namespace GradeManagement.Forms
{
  public class AdminMenuForm : Form
  {
    private User _currentUser;
    private AdminController _controller;
    
    // UI Components
    private Panel pnlSidebar;
    private Panel pnlContent;
    private Label lblWelcome;
    private Button btnLogout;
    private Button btnCourses;
    private Button btnEnroll;
    private Button btnImport;
    
    // Course Management Controls
    private DataGridView dgvCourses;
    private TextBox txtCourseName;
    private Button btnAddCourse;
    private Button btnRefreshCourses;
    
    // Enrollment Controls
    private DataGridView dgvCoursesForEnrollment;
    private CheckedListBox clbStudents;
    private Button btnEnrollSelected;
    private Label lblSelectedCourse;
    
    // Import Controls
    private TextBox txtFilePath;
    private Button btnBrowseFile;
    private Button btnImportFile;
    private TextBox txtImportResults;

    public AdminMenuForm(User user)
    {
      _currentUser = user;
      _controller = new AdminController();
      InitializeComponent();
      LoadInitialData();
    }

    private void InitializeComponent()
    {
      // Form settings
      this.Text = "Grade Management - Admin Panel";
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
      btnCourses = CreateMenuButton("📚 Courses", 60);
      btnCourses.Click += (s, e) => ShowCoursesPanel();
      
      btnEnroll = CreateMenuButton("👥 Enroll Students", 110);
      btnEnroll.Click += (s, e) => ShowEnrollmentPanel();
      
      btnImport = CreateMenuButton("📁 Import Courses", 160);
      btnImport.Click += (s, e) => ShowImportPanel();

      var btnUsers = CreateMenuButton("👤 Create Users", 210);
      btnUsers.Click += (s, e) => ShowUserManagementPanel();

      // Add controls to sidebar
      pnlSidebar.Controls.AddRange(new Control[] { lblWelcome, btnCourses, btnEnroll, btnImport, btnUsers, btnLogout });

      // Add panels to form
      this.Controls.Add(pnlContent);
      this.Controls.Add(pnlSidebar);

      // Show courses panel by default
      ShowCoursesPanel();
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
      await RefreshCourses();
    }

    #region Course Management Panel

    private void ShowCoursesPanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "Course Management",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      // Course Name
      var lblCourseName = new Label
      {
        Text = "Course Name:",
        Location = new Point(0, 50),
        Size = new Size(100, 25)
      };

      txtCourseName = new TextBox
      {
        Location = new Point(110, 50),
        Size = new Size(400, 25)
      };

      // Add Button
      btnAddCourse = new Button
      {
        Text = "Add Course",
        Location = new Point(530, 48),
        Size = new Size(120, 30),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnAddCourse.Click += BtnAddCourse_Click;

      // Refresh Button
      btnRefreshCourses = new Button
      {
        Text = "Refresh",
        Location = new Point(660, 48),
        Size = new Size(80, 30),
        BackColor = Color.FromArgb(100, 100, 100),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnRefreshCourses.Click += async (s, e) => await RefreshCourses();

      // DataGridView
      dgvCourses = new DataGridView
      {
        Location = new Point(0, 100),
        Size = new Size(750, 400),
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect
      };

      pnlContent.Controls.AddRange(new Control[] { 
        lblTitle, lblCourseName, txtCourseName, 
        btnAddCourse, btnRefreshCourses, dgvCourses 
      });
    }

    private async void BtnAddCourse_Click(object sender, EventArgs e)
    {
      string courseName = txtCourseName.Text.Trim();

      if (string.IsNullOrEmpty(courseName))
      {
        MessageBox.Show("Please enter Course Name", "Validation Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      var result = await _controller.AddCourse(courseName);
      
      if (result.success)
      {
        MessageBox.Show(result.message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        txtCourseName.Clear();
        await RefreshCourses();
      }
      else
      {
        MessageBox.Show(result.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private async System.Threading.Tasks.Task RefreshCourses()
    {
      var courses = await _controller.GetAllCourses();
      
      if (dgvCourses != null)
      {
        dgvCourses.DataSource = courses;
      }
      
      if (dgvCoursesForEnrollment != null)
      {
        dgvCoursesForEnrollment.DataSource = courses.ToList();
      }
    }

    #endregion

    #region Enrollment Panel

    private async void ShowEnrollmentPanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "Student Enrollment",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      var lblStep1 = new Label
      {
        Text = "Step 1: Select a Course",
        Location = new Point(0, 40),
        Size = new Size(200, 25),
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };

      // Courses DataGridView
      dgvCoursesForEnrollment = new DataGridView
      {
        Location = new Point(0, 70),
        Size = new Size(400, 200),
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false
      };
      dgvCoursesForEnrollment.SelectionChanged += DgvCoursesForEnrollment_SelectionChanged;

      var lblStep2 = new Label
      {
        Text = "Step 2: Select Students to Enroll",
        Location = new Point(420, 40),
        Size = new Size(250, 25),
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };

      lblSelectedCourse = new Label
      {
        Text = "No course selected",
        Location = new Point(420, 65),
        Size = new Size(350, 20),
        ForeColor = Color.Gray
      };

      // Students CheckedListBox
      clbStudents = new CheckedListBox
      {
        Location = new Point(420, 90),
        Size = new Size(350, 300),
        CheckOnClick = true
      };

      // Enroll Button
      btnEnrollSelected = new Button
      {
        Text = "Enroll Selected Students",
        Location = new Point(420, 400),
        Size = new Size(200, 35),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10, FontStyle.Bold),
        Enabled = false
      };
      btnEnrollSelected.Click += BtnEnrollSelected_Click;

      pnlContent.Controls.AddRange(new Control[] { 
        lblTitle, lblStep1, dgvCoursesForEnrollment, lblStep2, lblSelectedCourse, clbStudents, btnEnrollSelected 
      });

      await RefreshCourses();
      await LoadStudents();
    }

    private async void DgvCoursesForEnrollment_SelectionChanged(object sender, EventArgs e)
    {
      if (dgvCoursesForEnrollment.SelectedRows.Count > 0)
      {
        var selectedRow = dgvCoursesForEnrollment.SelectedRows[0];
        string courseId = selectedRow.Cells["id"].Value?.ToString() ?? "";
        string courseName = selectedRow.Cells["name"].Value?.ToString() ?? "";
        
        lblSelectedCourse.Text = $"Selected: {courseId} - {courseName}";
        btnEnrollSelected.Enabled = true;
        
        // Highlight already enrolled students
        await HighlightEnrolledStudents(courseId);
      }
      else
      {
        lblSelectedCourse.Text = "No course selected";
        btnEnrollSelected.Enabled = false;
      }
    }

    private async System.Threading.Tasks.Task LoadStudents()
    {
      var students = await _controller.GetAllStudents();
      clbStudents.Items.Clear();
      
      foreach (var student in students)
      {
        clbStudents.Items.Add(new StudentItem { Id = student.id, Name = student.name });
      }
    }

    private async System.Threading.Tasks.Task HighlightEnrolledStudents(string courseId)
    {
      var enrolledStudents = await _controller.GetCourseEnrollments(courseId);
      var enrolledIds = enrolledStudents.Select(s => s.id).ToHashSet();
      
      for (int i = 0; i < clbStudents.Items.Count; i++)
      {
        var item = (StudentItem)clbStudents.Items[i];
        if (enrolledIds.Contains(item.Id))
        {
          clbStudents.SetItemChecked(i, true);
        }
        else
        {
          clbStudents.SetItemChecked(i, false);
        }
      }
    }

    private async void BtnEnrollSelected_Click(object sender, EventArgs e)
    {
      if (dgvCoursesForEnrollment.SelectedRows.Count == 0)
      {
        MessageBox.Show("Please select a course first", "Validation Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      var selectedStudents = clbStudents.CheckedItems.Cast<StudentItem>().ToList();
      
      if (selectedStudents.Count == 0)
      {
        MessageBox.Show("Please select at least one student", "Validation Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      string courseId = dgvCoursesForEnrollment.SelectedRows[0].Cells["id"].Value?.ToString() ?? "";
      var studentIds = selectedStudents.Select(s => s.Id).ToList();

      var result = await _controller.EnrollStudents(courseId, studentIds);
      
      if (result.success)
      {
        string message = result.message;
        if (result.errors.Count > 0)
        {
          message += "\n\nErrors:\n" + string.Join("\n", result.errors);
        }
        MessageBox.Show(message, "Enrollment Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else
      {
        string message = result.message + "\n\nErrors:\n" + string.Join("\n", result.errors);
        MessageBox.Show(message, "Enrollment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion

    #region User Management Panel

    private async void ShowUserManagementPanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "User Management",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      // Create Faculty Manager Section
      var lblFMSection = new Label
      {
        Text = "Create Faculty Manager",
        Location = new Point(0, 50),
        Size = new Size(300, 25),
        Font = new Font("Segoe UI", 12, FontStyle.Bold)
      };

      var lblFMName = new Label { Text = "Name:", Location = new Point(0, 85), Size = new Size(100, 25) };
      var txtFMName = new TextBox { Location = new Point(110, 85), Size = new Size(250, 25) };

      var lblFMUsername = new Label { Text = "Username:", Location = new Point(0, 120), Size = new Size(100, 25) };
      var txtFMUsername = new TextBox { Location = new Point(110, 120), Size = new Size(250, 25) };

      var lblFMPassword = new Label { Text = "Password:", Location = new Point(0, 155), Size = new Size(100, 25) };
      var txtFMPassword = new TextBox { Location = new Point(110, 155), Size = new Size(250, 25) };

      var lblFMFaculty = new Label { Text = "Faculty:", Location = new Point(0, 190), Size = new Size(100, 25) };
      var cmbFMFaculty = new ComboBox 
      { 
        Location = new Point(110, 190), 
        Size = new Size(250, 25),
        DropDownStyle = ComboBoxStyle.DropDownList
      };

      var btnCreateFM = new Button
      {
        Text = "Create Faculty Manager",
        Location = new Point(110, 225),
        Size = new Size(180, 35),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };

      btnCreateFM.Click += async (s, e) =>
      {
        if (string.IsNullOrWhiteSpace(txtFMName.Text) || 
            string.IsNullOrWhiteSpace(txtFMUsername.Text) || 
            string.IsNullOrWhiteSpace(txtFMPassword.Text) ||
            cmbFMFaculty.SelectedItem == null)
        {
          MessageBox.Show("Please fill all fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        var faculty = (FacultyItem)cmbFMFaculty.SelectedItem;
        var result = await _controller.CreateFacultyManager(
          txtFMName.Text.Trim(),
          txtFMUsername.Text.Trim(),
          txtFMPassword.Text,
          faculty.Id
        );

        if (result.success)
        {
          MessageBox.Show(result.message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          txtFMName.Clear();
          txtFMUsername.Clear();
          txtFMPassword.Clear();
          cmbFMFaculty.SelectedIndex = -1;
        }
        else
        {
          MessageBox.Show(result.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      };

      // Create Student Section
      var lblStudentSection = new Label
      {
        Text = "Create Student",
        Location = new Point(400, 50),
        Size = new Size(300, 25),
        Font = new Font("Segoe UI", 12, FontStyle.Bold)
      };

      var lblStudentName = new Label { Text = "Name:", Location = new Point(400, 85), Size = new Size(100, 25) };
      var txtStudentName = new TextBox { Location = new Point(510, 85), Size = new Size(250, 25) };

      var lblStudentUsername = new Label { Text = "Username:", Location = new Point(400, 120), Size = new Size(100, 25) };
      var txtStudentUsername = new TextBox { Location = new Point(510, 120), Size = new Size(250, 25) };

      var lblStudentPassword = new Label { Text = "Password:", Location = new Point(400, 155), Size = new Size(100, 25) };
      var txtStudentPassword = new TextBox { Location = new Point(510, 155), Size = new Size(250, 25) };

      var lblStudentFaculty = new Label { Text = "Faculty:", Location = new Point(400, 190), Size = new Size(100, 25) };
      var cmbStudentFaculty = new ComboBox 
      { 
        Location = new Point(510, 190), 
        Size = new Size(250, 25),
        DropDownStyle = ComboBoxStyle.DropDownList
      };

      var btnCreateStudent = new Button
      {
        Text = "Create Student",
        Location = new Point(510, 225),
        Size = new Size(150, 35),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };

      btnCreateStudent.Click += async (s, e) =>
      {
        if (string.IsNullOrWhiteSpace(txtStudentName.Text) || 
            string.IsNullOrWhiteSpace(txtStudentUsername.Text) || 
            string.IsNullOrWhiteSpace(txtStudentPassword.Text) ||
            cmbStudentFaculty.SelectedItem == null)
        {
          MessageBox.Show("Please fill all fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        var faculty = (FacultyItem)cmbStudentFaculty.SelectedItem;
        var result = await _controller.CreateStudent(
          txtStudentName.Text.Trim(),
          txtStudentUsername.Text.Trim(),
          txtStudentPassword.Text,
          faculty.Id
        );

        if (result.success)
        {
          MessageBox.Show(result.message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          txtStudentName.Clear();
          txtStudentUsername.Clear();
          txtStudentPassword.Clear();
          cmbStudentFaculty.SelectedIndex = -1;
        }
        else
        {
          MessageBox.Show(result.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      };

      pnlContent.Controls.AddRange(new Control[] { 
        lblTitle,
        lblFMSection, lblFMName, txtFMName, lblFMUsername, txtFMUsername, lblFMPassword, txtFMPassword, lblFMFaculty, cmbFMFaculty, btnCreateFM,
        lblStudentSection, lblStudentName, txtStudentName, lblStudentUsername, txtStudentUsername, lblStudentPassword, txtStudentPassword, lblStudentFaculty, cmbStudentFaculty, btnCreateStudent
      });

      // Faculty Management Section (at the bottom)
      var lblFacultySection = new Label
      {
        Text = "Create Faculty",
        Location = new Point(0, 280),
        Size = new Size(300, 25),
        Font = new Font("Segoe UI", 12, FontStyle.Bold)
      };

      var lblFacultyName = new Label { Text = "Faculty Name:", Location = new Point(0, 315), Size = new Size(100, 25) };
      var txtFacultyName = new TextBox { Location = new Point(110, 315), Size = new Size(400, 25) };

      var btnCreateFaculty = new Button
      {
        Text = "Create Faculty",
        Location = new Point(110, 350),
        Size = new Size(150, 35),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };

      btnCreateFaculty.Click += async (s, e) =>
      {
        if (string.IsNullOrWhiteSpace(txtFacultyName.Text))
        {
          MessageBox.Show("Please enter Faculty Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        var result = await _controller.CreateFaculty(txtFacultyName.Text.Trim());

        if (result.success)
        {
          MessageBox.Show(result.message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          txtFacultyName.Clear();
          
          // Refresh faculty dropdowns
          var faculties = await _controller.GetAllFaculties();
          cmbFMFaculty.Items.Clear();
          cmbStudentFaculty.Items.Clear();
          foreach (var faculty in faculties)
          {
            cmbFMFaculty.Items.Add(new FacultyItem { Id = faculty.id, Name = faculty.name });
            cmbStudentFaculty.Items.Add(new FacultyItem { Id = faculty.id, Name = faculty.name });
          }
        }
        else
        {
          MessageBox.Show(result.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      };

      pnlContent.Controls.AddRange(new Control[] { lblFacultySection, lblFacultyName, txtFacultyName, btnCreateFaculty });

      // Load faculties
      var faculties = await _controller.GetAllFaculties();
      foreach (var faculty in faculties)
      {
        cmbFMFaculty.Items.Add(new FacultyItem { Id = faculty.id, Name = faculty.name });
        cmbStudentFaculty.Items.Add(new FacultyItem { Id = faculty.id, Name = faculty.name });
      }
    }

    #endregion

    #region Import Panel

    private void ShowImportPanel()
    {
      pnlContent.Controls.Clear();

      var lblTitle = new Label
      {
        Text = "Import Courses from CSV",
        Location = new Point(0, 0),
        Size = new Size(400, 30),
        Font = new Font("Segoe UI", 14, FontStyle.Bold)
      };

      var lblInstructions = new Label
      {
        Text = "CSV Format: courseId,courseName (one per line)",
        Location = new Point(0, 40),
        Size = new Size(500, 20),
        ForeColor = Color.Gray
      };

      var lblFilePath = new Label
      {
        Text = "File Path:",
        Location = new Point(0, 80),
        Size = new Size(80, 25)
      };

      txtFilePath = new TextBox
      {
        Location = new Point(90, 80),
        Size = new Size(500, 25),
        ReadOnly = true
      };

      btnBrowseFile = new Button
      {
        Text = "Browse...",
        Location = new Point(600, 78),
        Size = new Size(100, 30),
        BackColor = Color.FromArgb(100, 100, 100),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
      };
      btnBrowseFile.Click += BtnBrowseFile_Click;

      btnImportFile = new Button
      {
        Text = "Import",
        Location = new Point(710, 78),
        Size = new Size(100, 30),
        BackColor = Color.FromArgb(0, 120, 215),
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };
      btnImportFile.Click += BtnImportFile_Click;

      var lblResults = new Label
      {
        Text = "Import Results:",
        Location = new Point(0, 130),
        Size = new Size(150, 25),
        Font = new Font("Segoe UI", 10, FontStyle.Bold)
      };

      txtImportResults = new TextBox
      {
        Location = new Point(0, 160),
        Size = new Size(750, 300),
        Multiline = true,
        ScrollBars = ScrollBars.Vertical,
        ReadOnly = true
      };

      pnlContent.Controls.AddRange(new Control[] { 
        lblTitle, lblInstructions, lblFilePath, txtFilePath, btnBrowseFile, btnImportFile, lblResults, txtImportResults 
      });
    }

    private void BtnBrowseFile_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
        openFileDialog.Title = "Select Course Import File";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          txtFilePath.Text = openFileDialog.FileName;
        }
      }
    }

    private async void BtnImportFile_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtFilePath.Text))
      {
        MessageBox.Show("Please select a file first", "Validation Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      btnImportFile.Enabled = false;
      btnImportFile.Text = "Importing...";

      var result = await _controller.ImportCoursesFromFile(txtFilePath.Text);

      string resultText = $"Import completed:\n";
      resultText += $"Success: {result.successCount}\n";
      resultText += $"Failed: {result.failureCount}\n\n";

      if (result.errors.Count > 0)
      {
        resultText += "Errors:\n";
        resultText += string.Join("\n", result.errors);
      }

      txtImportResults.Text = resultText;

      if (result.success)
      {
        MessageBox.Show(result.message, "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await RefreshCourses();
      }
      else
      {
        MessageBox.Show(result.message, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      btnImportFile.Enabled = true;
      btnImportFile.Text = "Import";
    }

    #endregion

    private void BtnLogout_Click(object sender, EventArgs e)
    {
      var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      
      if (result == DialogResult.Yes)
      {
        this.Close();
        Application.Restart();
      }
    }

    // Helper class for student items in CheckedListBox
    private class StudentItem
    {
      public string Id { get; set; }
      public string Name { get; set; }

      public override string ToString()
      {
        return $"{Id} - {Name}";
      }
    }

    // Helper class for faculty items in ComboBox
    private class FacultyItem
    {
      public string Id { get; set; }
      public string Name { get; set; }

      public override string ToString()
      {
        return Name;
      }
    }
  }
}
