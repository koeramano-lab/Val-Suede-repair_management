
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RepairManagement
{
    public partial class Form1 : Form
    {
        private TextBox txtRepairID = new TextBox();
        private TextBox txtReporterName = new TextBox();
        private TextBox txtDepartment = new TextBox();
        private TextBox txtProblemDetails = new TextBox();
        private TextBox txtSearch = new TextBox();

        private ComboBox cmbEquipmentType = new ComboBox();
        private ComboBox cmbRepairStatus = new ComboBox();

        private DateTimePicker dtpReportDate = new DateTimePicker();
        private DataGridView dgvRepairs = new DataGridView();

        private bool isBuilding = false;

        public Form1()
        {
            InitializeComponent();
            BuildUI();
            LoadRepairs();
        }

        private void BuildUI()
        {
            if (isBuilding) return;
            isBuilding = true;

            SuspendLayout();

            Text = "Computer Repair Management System";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 650);
            Size = new Size(1200, 800);
            BackColor = Color.White;
            Font = new Font("Tahoma", 10F);
            AutoScaleMode = AutoScaleMode.Font;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(16),
                BackColor = Color.White,
                Margin = Padding.Empty
            };

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            Controls.Add(mainLayout);

            Label lblTitle = new Label
            {
                Text = "ระบบจัดการข้อมูลแจ้งซ่อมอุปกรณ์คอมพิวเตอร์",
                Font = new Font("Tahoma", 17F, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 18)
            };

            mainLayout.Controls.Add(lblTitle, 0, 0);

            TableLayoutPanel formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 4,
                Padding = new Padding(8),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 12)
            };

            formLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 125F));
            formLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));
            formLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 125F));
            formLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));

            for (int i = 0; i < 3; i++)
            {
                formLayout.RowStyles.Add(
                    new RowStyle(SizeType.AutoSize));
            }

            formLayout.RowStyles.Add(
                new RowStyle(SizeType.AutoSize));

            mainLayout.Controls.Add(formLayout, 0, 1);

            txtRepairID.ReadOnly = true;
            txtRepairID.TabStop = false;
            txtRepairID.Cursor = Cursors.Default;
            txtRepairID.ShortcutsEnabled = false;
            txtRepairID.GotFocus += (s, e) =>
            {
                ActiveControl = null;
            };

            cmbEquipmentType.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbEquipmentType.Items.AddRange(new object[]
            {
                "คอมพิวเตอร์ตั้งโต๊ะ",
                "โน้ตบุ๊ก",
                "เครื่องพิมพ์",
                "จอภาพ",
                "อุปกรณ์เครือข่าย",
                "อื่น ๆ"
            });

            cmbEquipmentType.SelectedIndex = 0;

            cmbRepairStatus.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbRepairStatus.Items.AddRange(new object[]
            {
                "รอดำเนินการ",
                "กำลังซ่อม",
                "ซ่อมเสร็จแล้ว"
            });

            cmbRepairStatus.SelectedIndex = 0;

            dtpReportDate.Format = DateTimePickerFormat.Short;
            dtpReportDate.Value = DateTime.Today;
            dtpReportDate.MaxDate = DateTime.Today;

            txtProblemDetails.Multiline = true;
            txtProblemDetails.ScrollBars = ScrollBars.Vertical;
            txtProblemDetails.Height = 70;

            StyleInput(txtRepairID);
            StyleInput(txtReporterName);
            StyleInput(txtDepartment);
            StyleInput(txtProblemDetails);
            StyleInput(txtSearch);
            StyleInput(cmbEquipmentType);
            StyleInput(cmbRepairStatus);

            dtpReportDate.Dock = DockStyle.Fill;
            dtpReportDate.Margin = new Padding(4, 5, 12, 5);

            AddFormRow(formLayout, 0,
                "รหัสแจ้งซ่อม", txtRepairID,
                "ชื่อผู้แจ้ง", txtReporterName);

            AddFormRow(formLayout, 1,
                "แผนก", txtDepartment,
                "ประเภทอุปกรณ์", cmbEquipmentType);

            AddFormRow(formLayout, 2,
                "วันที่แจ้ง", dtpReportDate,
                "สถานะ", cmbRepairStatus);

            Label lblDetails = CreateLabel("รายละเอียดปัญหา");

            formLayout.Controls.Add(lblDetails, 0, 3);
            formLayout.Controls.Add(txtProblemDetails, 1, 3);
            formLayout.SetColumnSpan(txtProblemDetails, 3);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10),
                Padding = Padding.Empty
            };

            mainLayout.Controls.Add(buttonPanel, 0, 2);

            AddActionButton(buttonPanel, "เพิ่มข้อมูล", AddRepair);
            AddActionButton(buttonPanel, "แก้ไข", UpdateRepair);
            AddActionButton(buttonPanel, "ลบ", DeleteRepair);
            AddActionButton(buttonPanel, "ล้างฟอร์ม", ClearForm);
            AddActionButton(buttonPanel, "แสดงทั้งหมด", LoadRepairs);

            FlowLayoutPanel searchPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10),
                Padding = Padding.Empty
            };

            mainLayout.Controls.Add(searchPanel, 0, 3);

            Label lblSearch = CreateLabel("ค้นหา");
            lblSearch.Width = 70;
            lblSearch.Margin = new Padding(0, 8, 8, 4);

            txtSearch.Width = 300;
            txtSearch.Margin = new Padding(0, 4, 8, 4);

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);

            AddActionButton(searchPanel, "ค้นหา", SearchRepairs);

            txtSearch.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SearchRepairs();
                    e.SuppressKeyPress = true;
                }
            };

            dgvRepairs.Dock = DockStyle.Fill;
            dgvRepairs.BackgroundColor = Color.White;
            dgvRepairs.GridColor = Color.LightGray;
            dgvRepairs.BorderStyle = BorderStyle.FixedSingle;

            dgvRepairs.ReadOnly = true;
            dgvRepairs.AllowUserToAddRows = false;
            dgvRepairs.AllowUserToDeleteRows = false;
            dgvRepairs.AllowUserToResizeRows = false;
            dgvRepairs.MultiSelect = false;

            dgvRepairs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvRepairs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvRepairs.RowHeadersVisible = false;
            dgvRepairs.AutoGenerateColumns = true;
            dgvRepairs.RowTemplate.Height = 30;

            dgvRepairs.DefaultCellStyle.BackColor = Color.White;
            dgvRepairs.DefaultCellStyle.ForeColor = Color.Black;
            dgvRepairs.DefaultCellStyle.SelectionBackColor =
                Color.LightBlue;
            dgvRepairs.DefaultCellStyle.SelectionForeColor =
                Color.Black;

            dgvRepairs.ColumnHeadersDefaultCellStyle.BackColor =
                Color.LightSteelBlue;
            dgvRepairs.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.Black;
            dgvRepairs.EnableHeadersVisualStyles = false;

            mainLayout.Controls.Add(dgvRepairs, 0, 4);

            dgvRepairs.CellClick += DgvRepairs_CellClick;
            dgvRepairs.DataBindingComplete +=
                DgvRepairs_DataBindingComplete;

            ResumeLayout(true);
        }
        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(4, 8, 8, 8)
            };
        }

        private void StyleInput(Control control)
        {
            control.Font = new Font("Tahoma", 10F);
            control.Margin = new Padding(4, 5, 12, 5);

            if (control is TextBox textBox)
            {
                textBox.Dock = DockStyle.Fill;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.Dock = DockStyle.Fill;
            }
        }

        private void AddFormRow(
            TableLayoutPanel layout,
            int row,
            string label1,
            Control control1,
            string label2,
            Control control2)
        {
            layout.Controls.Add(CreateLabel(label1), 0, row);
            layout.Controls.Add(control1, 1, row);
            layout.Controls.Add(CreateLabel(label2), 2, row);
            layout.Controls.Add(control2, 3, row);
        }

        private void AddActionButton(
            FlowLayoutPanel panel,
            string text,
            Action action)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(115, 38),
                Margin = new Padding(0, 2, 10, 5),
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true
            };

            button.Click += (sender, e) => action();
            panel.Controls.Add(button);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtReporterName.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อผู้แจ้ง");
                txtReporterName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDepartment.Text))
            {
                MessageBox.Show("กรุณากรอกแผนก");
                txtDepartment.Focus();
                return false;
            }

            if (cmbEquipmentType.SelectedIndex < 0)
            {
                MessageBox.Show("กรุณาเลือกประเภทอุปกรณ์");
                cmbEquipmentType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProblemDetails.Text))
            {
                MessageBox.Show("กรุณากรอกรายละเอียดปัญหา");
                txtProblemDetails.Focus();
                return false;
            }

            if (cmbRepairStatus.SelectedIndex < 0)
            {
                MessageBox.Show("กรุณาเลือกสถานะการซ่อม");
                cmbRepairStatus.Focus();
                return false;
            }

            if (dtpReportDate.Value.Date > DateTime.Today)
            {
                MessageBox.Show("วันที่แจ้งซ่อมห้ามเป็นวันในอนาคต");
                return false;
            }

            return true;
        }

        private void LoadRepairs()
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql =
                        "SELECT * FROM repair_requests " +
                        "ORDER BY repair_id DESC";

                    using (MySqlDataAdapter adapter =
                        new MySqlDataAdapter(sql, conn))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgvRepairs.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "โหลดข้อมูลไม่สำเร็จ\n" + ex.Message,
                    "ข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DgvRepairs_DataBindingComplete(
            object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            SetHeader("repair_id", "รหัสแจ้งซ่อม");
            SetHeader("reporter_name", "ชื่อผู้แจ้ง");
            SetHeader("department", "แผนก");
            SetHeader("equipment_type", "ประเภทอุปกรณ์");
            SetHeader("problem_details", "รายละเอียดปัญหา");
            SetHeader("report_date", "วันที่แจ้ง");
            SetHeader("repair_status", "สถานะ");

            if (dgvRepairs.Columns.Contains("report_date"))
            {
                dgvRepairs.Columns["report_date"]
                    .DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        private void SetHeader(string columnName, string header)
        {
            if (dgvRepairs.Columns.Contains(columnName))
            {
                dgvRepairs.Columns[columnName].HeaderText = header;
            }
        }

        private void DgvRepairs_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvRepairs.Rows[e.RowIndex];

            txtRepairID.Text =
                Convert.ToString(row.Cells["repair_id"].Value) ?? "";

            txtReporterName.Text =
                Convert.ToString(row.Cells["reporter_name"].Value) ?? "";

            txtDepartment.Text =
                Convert.ToString(row.Cells["department"].Value) ?? "";

            cmbEquipmentType.Text =
                Convert.ToString(row.Cells["equipment_type"].Value) ?? "";

            txtProblemDetails.Text =
                Convert.ToString(row.Cells["problem_details"].Value) ?? "";

            if (DateTime.TryParse(
                Convert.ToString(row.Cells["report_date"].Value),
                out DateTime reportDate))
            {
                dtpReportDate.Value = reportDate > DateTime.Today
                    ? DateTime.Today
                    : reportDate;
            }

            cmbRepairStatus.Text =
                Convert.ToString(row.Cells["repair_status"].Value) ?? "";
        }

        private void AddRepair()
        {
            if (!ValidateInput()) return;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO repair_requests
                        (
                            reporter_name,
                            department,
                            equipment_type,
                            problem_details,
                            report_date,
                            repair_status
                        )
                        VALUES
                        (
                            @name,
                            @department,
                            @equipment,
                            @details,
                            @date,
                            @status
                        )";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        AddParameters(cmd);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("เพิ่มข้อมูลแจ้งซ่อมสำเร็จ");

                ClearForm();
                LoadRepairs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "เพิ่มข้อมูลไม่สำเร็จ\n" + ex.Message);
            }
        }

        private void UpdateRepair()
        {
            if (!int.TryParse(txtRepairID.Text, out int id))
            {
                MessageBox.Show("กรุณาเลือกข้อมูลจากตารางก่อนแก้ไข");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        UPDATE repair_requests SET
                            reporter_name = @name,
                            department = @department,
                            equipment_type = @equipment,
                            problem_details = @details,
                            report_date = @date,
                            repair_status = @status
                        WHERE repair_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        AddParameters(cmd);
                        cmd.Parameters.AddWithValue("@id", id);

                        int affected = cmd.ExecuteNonQuery();

                        MessageBox.Show(
                            affected > 0
                                ? "แก้ไขข้อมูลสำเร็จ"
                                : "ไม่พบข้อมูลที่ต้องการแก้ไข");
                    }
                }

                ClearForm();
                LoadRepairs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "แก้ไขข้อมูลไม่สำเร็จ\n" + ex.Message);
            }
        }

        private void DeleteRepair()
        {
            if (!int.TryParse(txtRepairID.Text, out int id))
            {
                MessageBox.Show("กรุณาเลือกข้อมูลจากตารางก่อนลบ");
                return;
            }

            DialogResult result = MessageBox.Show(
                "คุณต้องการลบรายการแจ้งซ่อมนี้หรือไม่?",
                "ยืนยันการลบ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql =
                        "DELETE FROM repair_requests " +
                        "WHERE repair_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int affected = cmd.ExecuteNonQuery();

                        MessageBox.Show(
                            affected > 0
                                ? "ลบข้อมูลสำเร็จ"
                                : "ไม่พบข้อมูลที่ต้องการลบ");
                    }
                }

                ClearForm();
                LoadRepairs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "ลบข้อมูลไม่สำเร็จ\n" + ex.Message);
            }
        }

        private void SearchRepairs()
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT * FROM repair_requests
                        WHERE CAST(repair_id AS CHAR) LIKE @keyword
                           OR reporter_name LIKE @keyword
                           OR department LIKE @keyword
                           OR equipment_type LIKE @keyword
                           OR repair_status LIKE @keyword
                        ORDER BY repair_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@keyword",
                            "%" + txtSearch.Text.Trim() + "%");

                        using (MySqlDataAdapter adapter =
                            new MySqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            dgvRepairs.DataSource = table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "ค้นหาข้อมูลไม่สำเร็จ\n" + ex.Message);
            }
        }

        private void AddParameters(MySqlCommand cmd)
        {
            cmd.Parameters.AddWithValue(
                "@name", txtReporterName.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@department", txtDepartment.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@equipment", cmbEquipmentType.Text);

            cmd.Parameters.AddWithValue(
                "@details", txtProblemDetails.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@date", dtpReportDate.Value.Date);

            cmd.Parameters.AddWithValue(
                "@status", cmbRepairStatus.Text);
        }

        private void ClearForm()
        {
            txtRepairID.Clear();
            txtReporterName.Clear();
            txtDepartment.Clear();
            txtProblemDetails.Clear();
            txtSearch.Clear();

            cmbEquipmentType.SelectedIndex = 0;
            cmbRepairStatus.SelectedIndex = 0;
            dtpReportDate.Value = DateTime.Today;

            dgvRepairs.ClearSelection();
            txtReporterName.Focus();
        }
    }
}
