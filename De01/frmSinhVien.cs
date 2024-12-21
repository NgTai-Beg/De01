using De01.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Lop> LopSV = context.Lops.ToList(); //l y các khoa
                List<Sinhvien> SV = context.Sinhviens.ToList(); //l y sinh viên
                FillCombobox(LopSV);
                BindGrid(SV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillCombobox(List<Lop> LopSV)
        {
            this.cboLop.DataSource = LopSV;
            this.cboLop.DisplayMember = "TenLop";
            this.cboLop.ValueMember = "MaLop";
        }
        //Hàm binding gridView t list sinh viên
        private void BindGrid(List<Sinhvien> SV)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in SV)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.MaSV;             
                dgvSinhVien.Rows[index].Cells[1].Value = item.HoTenSV;          
                dgvSinhVien.Rows[index].Cells[2].Value = item.NgaySinh.ToString("dd/MM/yyyy"); 
                dgvSinhVien.Rows[index].Cells[3].Value = item.Lop.TenLop;
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
           
                txtMaSV.Text = dgvSinhVien.Rows[e.RowIndex].Cells[0].Value?.ToString(); 
                txtHoTenSV.Text = dgvSinhVien.Rows[e.RowIndex].Cells[1].Value?.ToString();
                var ngaySinhValue = dgvSinhVien.Rows[e.RowIndex].Cells[2].Value?.ToString();
                if (DateTime.TryParseExact(ngaySinhValue, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinh))
                {
                    dtNgaySinh.Value = ngaySinh;
                }
                else
                {
                    dtNgaySinh.Value = DateTime.Now;
                }
                cboLop.Text = dgvSinhVien.Rows[e.RowIndex].Cells[3].Value?.ToString(); 
            }
        }
        private void LoadData()
        {
            try
            {
                using (Model1 context = new Model1())
                {
                    // Lấy danh sách lớp và sinh viên
                    List<Lop> listLop = context.Lops.ToList();
                    List<Sinhvien> listSinhVien = context.Sinhviens.ToList();

                    // Điền dữ liệu vào ComboBox Lớp
                    FillCombobox(listLop);

                    // Điền dữ liệu vào DataGridView
                    BindGrid(listSinhVien);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 context = new Model1())
                {
                    // Tạo đối tượng Sinhvien mới
                    Sinhvien sv = new Sinhvien
                    {
                        MaSV = txtMaSV.Text,
                        HoTenSV = txtHoTenSV.Text,
                        NgaySinh = dtNgaySinh.Value,
                        MaLop = cboLop.SelectedValue.ToString()
                    };

                    // Thêm đối tượng vào cơ sở dữ liệu
                    context.Sinhviens.Add(sv);
                    context.SaveChanges();

                    // Làm mới dữ liệu
                    LoadData();

                    MessageBox.Show("Thêm sinh viên thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 context = new Model1())
                {
                    // Tìm sinh viên theo Mã SV
                    string maSV = txtMaSV.Text;
                    Sinhvien sv = context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);

                    if (sv != null)
                    {
                        // Cập nhật thông tin
                        sv.HoTenSV = txtHoTenSV.Text;
                        sv.NgaySinh = dtNgaySinh.Value;
                        sv.MaLop = cboLop.SelectedValue.ToString();

                        context.SaveChanges();

                        // Làm mới dữ liệu
                        LoadData();

                        MessageBox.Show("Cập nhật sinh viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để sửa!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa sinh viên: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (Model1 context = new Model1())
                    {
                        string maSV = txtMaSV.Text;
                        Sinhvien sv = context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);

                        if (sv != null)
                        {
                            context.Sinhviens.Remove(sv);
                            context.SaveChanges();
                            LoadData();
                            MessageBox.Show("Xóa sinh viên thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để xóa!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa sinh viên: {ex.Message}");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 context = new Model1())
                {
                    string keyword = txtTimKiem.Text.Trim();
                    var listSinhVien = context.Sinhviens
                        .Where(s => s.HoTenSV.Contains(keyword))
                        .ToList();

                    if (listSinhVien.Count > 0)
                    {
                        BindGrid(listSinhVien);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên nào!");
                        LoadData(); // Hiển thị lại toàn bộ danh sách nếu không tìm thấy
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}");
            }
        }
    }
}
