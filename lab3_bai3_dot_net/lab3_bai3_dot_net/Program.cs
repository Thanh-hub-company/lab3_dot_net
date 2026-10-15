using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab03_OOPAdvanced
{
    #region 1. BÀI 1, 2, 4: KẾ THỪA, GHI ĐÈ PHƯƠNG THỨC & INTERFACE

    // Interface quy định hợp đồng kiểm tra dữ liệu hợp lệ
    public interface IValidatable
    {
        bool IsValid(out string message);
    }

    // Interface quy định hợp đồng định dạng dữ liệu hiển thị
    public interface IPrintable
    {
        string ToDisplayText();
    }

    // LỚP CƠ SỞ (BASE CLASS) NGUOI
    public class Nguoi
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }

        public Nguoi(string hoTen, DateTime ngaySinh)
        {
            HoTen = hoTen;
            NgaySinh = ngaySinh;
        }

        // Phương thức ảo (virtual) cho phép lớp con ghi đè (override)
        public virtual string MoTa()
        {
            return $"{HoTen} - {NgaySinh:dd/MM/yyyy}";
        }
    }

    // LỚP DẪN XUẤT (DERIVED CLASS) SINHVIEN
    public class SinhVien : Nguoi, IValidatable, IPrintable
    {
        public string MaSV { get; set; }
        public double DiemTB { get; set; }

        // Gọi Constructor lớp cha bằng từ khóa 'base'
        public SinhVien(string ma, string ten, DateTime ngaySinh, double diem)
            : base(ten, ngaySinh)
        {
            MaSV = ma;
            DiemTB = diem;
        }

        // Ghi đè (override) phương thức MoTa()
        public override string MoTa()
        {
            return $"{base.MoTa()} - Mã SV: {MaSV} - ĐTB: {DiemTB:N2}";
        }

        // Thực thi phương thức từ IValidatable
        public bool IsValid(out string message)
        {
            if (string.IsNullOrWhiteSpace(MaSV))
            {
                message = "Mã sinh viên không được để rỗng.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                message = "Họ tên sinh viên không được để rỗng.";
                return false;
            }
            if (DiemTB < 0 || DiemTB > 10)
            {
                message = "Điểm trung bình phải nằm trong khoảng [0, 10].";
                return false;
            }
            message = "Dữ liệu hợp lệ.";
            return true;
        }

        // Thực thi phương thức từ IPrintable
        public string ToDisplayText()
        {
            return $"[SV] {MaSV,-8} | {HoTen,-20} | Ngày sinh: {NgaySinh:dd/MM/yyyy} | Điểm: {DiemTB,4:N2}";
        }
    }

    #endregion

    #region 2. BÀI 3 & BÀI 5: LỚP TRỪU TƯỢNG HỌC PHÍ & ĐA HÌNH

    public abstract class NguoiHoc
    {
        public string HoTen { get; set; }
        public int SoTinChi { get; set; }

        // Phương thức trừu tượng (abstract method)
        public abstract decimal TinhHocPhi();

        public virtual void HienThiThongTin()
        {
            Console.WriteLine($"Họ tên: {HoTen,-20} | Tín chỉ: {SoTinChi,2} | Học phí: {TinhHocPhi(),12:N0} VNĐ");
        }
    }

    public class SinhVienChinhQuy : NguoiHoc
    {
        public override decimal TinhHocPhi()
        {
            return SoTinChi * 450000m;
        }
    }

    public class SinhVienLienThong : NguoiHoc
    {
        public override decimal TinhHocPhi()
        {
            return SoTinChi * 520000m;
        }
    }

    #endregion

    #region 3. BÀI 6: HỆ THỐNG NHÂN VIÊN (TỰ THỰC HIỆN)

    public abstract class NhanVien
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public decimal LuongCoBan { get; set; }

        public NhanVien(string ma, string ten, decimal luongCB)
        {
            MaNV = ma;
            HoTen = ten;
            LuongCoBan = luongCB;
        }

        public abstract decimal TinhLuong();

        public virtual void HienThi()
        {
            Console.WriteLine($"{MaNV,-8} | {HoTen,-20} | Lương thực lĩnh: {TinhLuong(),14:N0} VNĐ");
        }
    }

    public class NhanVienVanPhong : NhanVien
    {
        public int SoNgayCong { get; set; }

        public NhanVienVanPhong(string ma, string ten, decimal luongCB, int ngayCong)
            : base(ma, ten, luongCB)
        {
            SoNgayCong = ngayCong;
        }

        public override decimal TinhLuong()
        {
            return LuongCoBan + (SoNgayCong * 200000m);
        }
    }

    public class NhanVienKinhDoanh : NhanVien
    {
        public decimal DoanhSo { get; set; }
        public double TiLeHoaHong { get; set; }

        public NhanVienKinhDoanh(string ma, string ten, decimal luongCB, decimal doanhSo, double hoaHong)
            : base(ma, ten, luongCB)
        {
            DoanhSo = doanhSo;
            TiLeHoaHong = hoaHong;
        }

        public override decimal TinhLuong()
        {
            return LuongCoBan + (DoanhSo * (decimal)TiLeHoaHong);
        }
    }

    public class GiangVien : NhanVien
    {
        public int SoTietDay { get; set; }

        public GiangVien(string ma, string ten, decimal luongCB, int tietDay)
            : base(ma, ten, luongCB)
        {
            SoTietDay = tietDay;
        }

        public override decimal TinhLuong()
        {
            return LuongCoBan + (SoTietDay * 150000m);
        }
    }

    #endregion

    #region 4. BÀI 7: QUẢN LÝ PHƯƠNG TIỆN (TỰ THỰC HIỆN)

    public interface IMove
    {
        void TangToc(double delta);
        void Phanh(double delta);
        double GetTocDo();
    }

    public abstract class PhuongTien : IMove
    {
        public string TenPhuongTien { get; set; }
        public double TocDoHienTai { get; protected set; }

        public PhuongTien(string ten)
        {
            TenPhuongTien = ten;
            TocDoHienTai = 0;
        }

        public void TangToc(double delta)
        {
            TocDoHienTai += delta;
            Console.WriteLine($"[{TenPhuongTien}] Tăng tốc thêm {delta} km/h. Tốc độ hiện tại: {TocDoHienTai} km/h");
        }

        public void Phanh(double delta)
        {
            TocDoHienTai = Math.Max(0, TocDoHienTai - delta);
            Console.WriteLine($"[{TenPhuongTien}] Phanh giảm {delta} km/h. Tốc độ hiện tại: {TocDoHienTai} km/h");
        }

        public double GetTocDo() => TocDoHienTai;

        public abstract double TinhChiPhiVanHanh(double soKm);
    }

    public class XeMay : PhuongTien
    {
        public XeMay(string ten) : base(ten) { }

        public override double TinhChiPhiVanHanh(double soKm)
        {
            return (soKm / 100.0) * 2.2 * 23000;
        }
    }

    public class OTo : PhuongTien
    {
        public OTo(string ten) : base(ten) { }

        public override double TinhChiPhiVanHanh(double soKm)
        {
            return (soKm / 100.0) * 8.0 * 23000;
        }
    }

    public class XeDap : PhuongTien
    {
        public XeDap(string ten) : base(ten) { }

        public override double TinhChiPhiVanHanh(double soKm)
        {
            return 0;
        }
    }

    #endregion

    #region 5. BÀI 8: QUẢN LÝ TÀI LIỆU THƯ VIỆN (TỰ THỰC HIỆN)

    public abstract class TaiLieu
    {
        public string MaTaiLieu { get; set; }
        public string TenNhaXuatBan { get; set; }
        public int SoBanPhatHanh { get; set; }

        public TaiLieu(string ma, string nxb, int soBan)
        {
            MaTaiLieu = ma;
            TenNhaXuatBan = nxb;
            SoBanPhatHanh = soBan;
        }

        public abstract void HienThi();
    }

    public class Sach : TaiLieu
    {
        public string TenTacGia { get; set; }
        public int SoTrang { get; set; }

        public Sach(string ma, string nxb, int soBan, string tacGia, int soTrang)
            : base(ma, nxb, soBan)
        {
            TenTacGia = tacGia;
            SoTrang = soTrang;
        }

        public override void HienThi()
        {
            Console.WriteLine($"[SÁCH] Mã: {MaTaiLieu,-8} | NXB: {TenNhaXuatBan,-15} | Tác giả: {TenTacGia,-15} | Số trang: {SoTrang}");
        }
    }

    public class TapChi : TaiLieu
    {
        public int SoPhatHanh { get; set; }
        public int ThangPhatHanh { get; set; }

        public TapChi(string ma, string nxb, int soBan, int soPH, int thangPH)
            : base(ma, nxb, soBan)
        {
            SoPhatHanh = soPH;
            ThangPhatHanh = thangPH;
        }

        public override void HienThi()
        {
            Console.WriteLine($"[TẠP CHÍ] Mã: {MaTaiLieu,-8} | NXB: {TenNhaXuatBan,-15} | Số PH: {SoPhatHanh,-5} | Tháng PH: {ThangPhatHanh}");
        }
    }

    public class LuanVan : TaiLieu
    {
        public string TenSinhVien { get; set; }
        public string NganhHoc { get; set; }

        public LuanVan(string ma, string nxb, int soBan, string sv, string nganh)
            : base(ma, nxb, soBan)
        {
            TenSinhVien = sv;
            NganhHoc = nganh;
        }

        public override void HienThi()
        {
            Console.WriteLine($"[LUẬN VĂN] Mã: {MaTaiLieu,-8} | NXB: {TenNhaXuatBan,-15} | SV: {TenSinhVien,-15} | Ngành: {NganhHoc}");
        }
    }

    #endregion

    #region 6. BÀI 9: HỆ THỐNG THANH TOÁN (TỰ THỰC HIỆN)

    public interface IPayment
    {
        bool ThanhToan(decimal soTien, out string log);
        string GetTenPhuongThuc();
    }

    public class ThanhToanTienMat : IPayment
    {
        public decimal TienMatHienCo { get; set; }

        public ThanhToanTienMat(decimal tienMat)
        {
            TienMatHienCo = tienMat;
        }

        public string GetTenPhuongThuc() => "Tiền mặt";

        public bool ThanhToan(decimal soTien, out string log)
        {
            if (TienMatHienCo >= soTien)
            {
                TienMatHienCo -= soTien;
                log = $"Thanh toán {soTien:N0} VNĐ bằng Tiền mặt thành công. Tiền thừa: {TienMatHienCo:N0} VNĐ.";
                return true;
            }
            log = $"Thanh toán Tiền mặt thất bại! Khách đưa {TienMatHienCo:N0} VNĐ không đủ trả {soTien:N0} VNĐ.";
            return false;
        }
    }

    public class ChuyenKhoan : IPayment
    {
        public string SoTaiKhoan { get; set; }
        public decimal SoDuNganHang { get; set; }

        public ChuyenKhoan(string stk, decimal soDu)
        {
            SoTaiKhoan = stk;
            SoDuNganHang = soDu;
        }

        public string GetTenPhuongThuc() => "Chuyển khoản Ngân hàng";

        public bool ThanhToan(decimal soTien, out string log)
        {
            if (SoDuNganHang >= soTien)
            {
                SoDuNganHang -= soTien;
                log = $"Chuyển khoản {soTien:N0} VNĐ từ STK {SoTaiKhoan} thành công. Số dư còn: {SoDuNganHang:N0} VNĐ.";
                return true;
            }
            log = $"Giao dịch chuyển khoản từ STK {SoTaiKhoan} thất bại! Số dư không đủ.";
            return false;
        }
    }

    public class ViDienTu : IPayment
    {
        public string SoDienThoai { get; set; }
        public decimal SoDuVi { get; set; }

        public ViDienTu(string sdt, decimal soDu)
        {
            SoDienThoai = sdt;
            SoDuVi = soDu;
        }

        public string GetTenPhuongThuc() => "Ví điện tử (Momo/ZaloPay)";

        public bool ThanhToan(decimal soTien, out string log)
        {
            if (SoDuVi >= soTien)
            {
                SoDuVi -= soTien;
                log = $"Thanh toán {soTien:N0} VNĐ qua Ví {SoDienThoai} thành công. Dư: {SoDuVi:N0} VNĐ.";
                return true;
            }
            log = $"Thanh toán Ví điện tử thất bại! Số dư Ví không đủ.";
            return false;
        }
    }

    #endregion

    #region 7. BÀI 10: LỚP TRỪU TƯỢNG SINH VIÊN NÂNG CẠO (SỬA HOÀN CHỈNH LỖI CS0535 & CS0161)

    public abstract class SinhVienNangCao : Nguoi, IValidatable, IPrintable
    {
        public string MaSV { get; set; }
        public double DiemTB { get; set; }

        public SinhVienNangCao(string ma, string ten, DateTime ngaySinh, double diem)
            : base(ten, ngaySinh)
        {
            MaSV = ma;
            DiemTB = diem;
        }

        public abstract decimal TinhHocPhi();
        public abstract string GetLoaiSinhVien();

        // Thực thi đầy đủ IValidatable với giá trị trả về hợp lệ
        public bool IsValid(out string message)
        {
            if (string.IsNullOrWhiteSpace(MaSV))
            {
                message = "Mã SV không được rỗng.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                message = "Họ tên không được rỗng.";
                return false;
            }
            if (DiemTB < 0 || DiemTB > 10)
            {
                message = "Điểm TB phải thuộc [0, 10].";
                return false;
            }

            message = "Dữ liệu hợp lệ.";
            return true;
        }

        // Thực thi đầy đủ IPrintable
        public virtual string ToDisplayText()
        {
            return $"[{GetLoaiSinhVien(),-10}] Mã: {MaSV,-8} | Tên: {HoTen,-18} | Ngày sinh: {NgaySinh:dd/MM/yyyy} | ĐTB: {DiemTB,4:N2} | HP: {TinhHocPhi(),12:N0} VNĐ";
        }
    }

    public class SVChinhQuy10 : SinhVienNangCao
    {
        public int SoTinChi { get; set; }
        public SVChinhQuy10(string ma, string ten, DateTime ngaySinh, double diem, int stc)
            : base(ma, ten, ngaySinh, diem) { SoTinChi = stc; }

        public override decimal TinhHocPhi() => SoTinChi * 450000m;
        public override string GetLoaiSinhVien() => "Chính quy";
    }

    public class SVLienThong10 : SinhVienNangCao
    {
        public int SoTinChi { get; set; }
        public SVLienThong10(string ma, string ten, DateTime ngaySinh, double diem, int stc)
            : base(ma, ten, ngaySinh, diem) { SoTinChi = stc; }

        public override decimal TinhHocPhi() => SoTinChi * 520000m;
        public override string GetLoaiSinhVien() => "Liên thông";
    }

    public class SVQuocTe10 : SinhVienNangCao
    {
        public decimal HocPhiCoDinh { get; set; }
        public SVQuocTe10(string ma, string ten, DateTime ngaySinh, double diem, decimal hp)
            : base(ma, ten, ngaySinh, diem) { HocPhiCoDinh = hp; }

        public override decimal TinhHocPhi() => HocPhiCoDinh;
        public override string GetLoaiSinhVien() => "Quốc tế";
    }

    #endregion

    #region 8. CHƯƠNG TRÌNH CHÍNH & MENU ĐIỀU HƯỚNG

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int chon = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("==================================================================");
                Console.WriteLine("       TRƯỜNG ĐẠI HỌC CÔNG NGHỆ ĐÔNG Á - BỘ MÔN CNPM               ");
                Console.WriteLine("    LAB 03: KẾ THỪA, ĐA HÌNH, LỚP TRỪU TƯỢNG VÀ INTERFACE        ");
                Console.WriteLine("==================================================================");
                Console.WriteLine("1.  Bài 1 - Kế thừa Nguoi & SinhVien (sử dụng base)");
                Console.WriteLine("2.  Bài 2 - Ghi đè phương thức MoTa() (virtual & override)");
                Console.WriteLine("3.  Bài 3 - Lớp trừu tượng NguoiHoc & TinhHocPhi()");
                Console.WriteLine("4.  Bài 4 - Interface IValidatable & IPrintable");
                Console.WriteLine("5.  Bài 5 - Danh sách đa hình List<NguoiHoc>");
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("6.  Bài 6 - Hệ thống Quản lý Nhân viên (Kế thừa & Đa hình)");
                Console.WriteLine("7.  Bài 7 - Quản lý Phương tiện giao thông (IMove & PhuongTien)");
                Console.WriteLine("8.  Bài 8 - Quản lý Tài liệu thư viện (Sach, TapChi, LuanVan)");
                Console.WriteLine("9.  Bài 9 - Hệ thống Thanh toán (IPayment đa phương thức)");
                Console.WriteLine("10. Bài 10 - Nâng cấp Quản lý Sinh viên hoàn chỉnh (Console Menu)");
                Console.WriteLine("0.  Thoát chương trình");
                Console.WriteLine("==================================================================");
                Console.Write("Mời chọn bài tập (0-10): ");

                if (!int.TryParse(Console.ReadLine(), out chon)) chon = -1;

                Console.WriteLine("\n------------------------------------------------------------------");
                switch (chon)
                {
                    case 1: ChayBaiTap1(); break;
                    case 2: ChayBaiTap2(); break;
                    case 3: ChayBaiTap3(); break;
                    case 4: ChayBaiTap4(); break;
                    case 5: ChayBaiTap5(); break;
                    case 6: ChayBaiTap6(); break;
                    case 7: ChayBaiTap7(); break;
                    case 8: ChayBaiTap8(); break;
                    case 9: ChayBaiTap9(); break;
                    case 10: ChayBaiTap10(); break;
                    case 0: Console.WriteLine("Cảm ơn bạn đã sử dụng chương trình!"); break;
                    default: Console.WriteLine("Lựa chọn không hợp lệ!"); break;
                }

                if (chon != 0)
                {
                    Console.WriteLine("\nNhấn phím bất kỳ để quay lại Menu chính...");
                    Console.ReadKey();
                }

            } while (chon != 0);
        }

        // --- DEMO BÀI TẬP HƯỚNG DẪN (1 - 5) ---

        static void ChayBaiTap1()
        {
            Console.WriteLine("=== BÀI 1: KẾ THỪA NGUOI - SINHVIEN ===");
            SinhVien sv = new SinhVien("SV001", "Nguyễn Văn Anh", new DateTime(2004, 3, 15), 8.5);
            Console.WriteLine($"Khởi tạo thành công SinhVien qua constructor 'base':");
            Console.WriteLine($"Họ tên (lớp cha Nguoi): {sv.HoTen}");
            Console.WriteLine($"Ngày sinh (lớp cha Nguoi): {sv.NgaySinh:dd/MM/yyyy}");
            Console.WriteLine($"Mã SV (lớp con SinhVien): {sv.MaSV}");
            Console.WriteLine($"Điểm TB (lớp con SinhVien): {sv.DiemTB}");
        }

        static void ChayBaiTap2()
        {
            Console.WriteLine("=== BÀI 2: GHI ĐÈ PHƯƠNG THỨC MOTA() ===");
            Nguoi ng = new Nguoi("Trần Văn Bảng", new DateTime(1995, 10, 20));
            SinhVien sv = new SinhVien("SV002", "Lê Thị Cúc", new DateTime(2005, 5, 12), 9.1);

            Console.WriteLine("1. Gọi MoTa() từ đối tượng Nguoi:");
            Console.WriteLine("   " + ng.MoTa());

            Console.WriteLine("\n2. Gọi MoTa() ghi đè (override) từ đối tượng SinhVien:");
            Console.WriteLine("   " + sv.MoTa());
        }

        static void ChayBaiTap3()
        {
            Console.WriteLine("=== BÀI 3: LỚP TRỪU TƯỢNG NGUOIHOC ===");
            NguoiHoc svCQ = new SinhVienChinhQuy { HoTen = "Phạm Đăng Khoa", SoTinChi = 16 };
            NguoiHoc svLT = new SinhVienLienThong { HoTen = "Hoàng Thanh Mai", SoTinChi = 16 };

            Console.WriteLine("Học phí Sinh viên Chính quy (450k/TC):");
            svCQ.HienThiThongTin();

            Console.WriteLine("\nHọc phí Sinh viên Liên thông (520k/TC):");
            svLT.HienThiThongTin();
        }

        static void ChayBaiTap4()
        {
            Console.WriteLine("=== BÀI 4: INTERFACE IVALIDATABLE & IPRINTABLE ===");
            SinhVien svHople = new SinhVien("SV004", "Vũ Minh Tuấn", new DateTime(2004, 8, 1), 7.5);
            SinhVien svLoi = new SinhVien("", "Trần Lỗi", new DateTime(2004, 1, 1), 12.0);

            IPrintable printable = svHople;
            Console.WriteLine("In dữ liệu từ IPrintable:");
            Console.WriteLine(printable.ToDisplayText());

            Console.WriteLine("\nKiểm tra dữ liệu đối tượng hợp lệ từ IValidatable:");
            if (svHople.IsValid(out string msg1))
                Console.WriteLine("-> Hợp lệ: " + msg1);

            Console.WriteLine("\nKiểm tra dữ liệu đối tượng lỗi từ IValidatable:");
            if (!svLoi.IsValid(out string msg2))
                Console.WriteLine("-> Bắt lỗi thành công: " + msg2);
        }

        static void ChayBaiTap5()
        {
            Console.WriteLine("=== BÀI 5: DANH SÁCH ĐA HÌNH LIST<NGUOIHOC> ===");
            List<NguoiHoc> ds = new List<NguoiHoc>
            {
                new SinhVienChinhQuy { HoTen = "Nguyễn An", SoTinChi = 18 },
                new SinhVienLienThong { HoTen = "Trần Bình", SoTinChi = 15 },
                new SinhVienChinhQuy { HoTen = "Lê Cường", SoTinChi = 20 },
                new SinhVienLienThong { HoTen = "Phạm Dung", SoTinChi = 12 }
            };

            decimal tong = 0;
            Console.WriteLine("--- BẢNG TÍNH HỌC PHÍ ĐA HÌNH ---");
            foreach (NguoiHoc item in ds)
            {
                decimal hp = item.TinhHocPhi();
                tong += hp;
                Console.WriteLine($"Họ tên: {item.HoTen,-15} | Loại: {item.GetType().Name,-18} | Tín chỉ: {item.SoTinChi,2} | Học phí: {hp,12:N0} VNĐ");
            }
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine($"=> TỔNG HỌC PHÍ THU ĐƯỢC: {tong:N0} VNĐ");
        }

        // --- DEMO BÀI TẬP TỰ THỰC HIỆN (6 - 10) ---

        static void ChayBaiTap6()
        {
            Console.WriteLine("=== BÀI 6: HỆ THỐNG QUẢN LÝ NHÂN VIÊN ===");
            List<NhanVien> dsNV = new List<NhanVien>
            {
                new NhanVienVanPhong("NV01", "Nguyễn Văn Trưởng", 5000000, 22),
                new NhanVienKinhDoanh("NV02", "Trần Thị Sale", 4000000, 150000000, 0.05),
                new GiangVien("GV01", "TS. Lê Hoàng Nam", 6000000, 45)
            };

            Console.WriteLine("--- BẢNG LƯƠNG NHÂN VIÊN ĐA HÌNH ---");
            decimal tongLuong = 0;
            foreach (var nv in dsNV)
            {
                nv.HienThi();
                tongLuong += nv.TinhLuong();
            }
            Console.WriteLine($"=> TỔNG QUỸ LƯƠNG CÔNG TY: {tongLuong:N0} VNĐ");
        }

        static void ChayBaiTap7()
        {
            Console.WriteLine("=== BÀI 7: QUẢN LÝ PHƯƠNG TIỆN GIAO THÔNG ===");
            List<PhuongTien> dsPT = new List<PhuongTien>
            {
                new XeMay("Xe Honda Wave"),
                new OTo("Xe Mazda 3"),
                new XeDap("Xe đạp Thống Nhất")
            };

            Console.WriteLine("--- TĂNG TỐC & PHANH MÔ PHỎNG ---");
            dsPT[0].TangToc(40);
            dsPT[1].TangToc(80);
            dsPT[1].Phanh(30);

            Console.WriteLine("\n--- TÍNH CHI PHÍ VẬN HÀNH CHO QUÃNG ĐƯỜNG 150 KM ---");
            foreach (var pt in dsPT)
            {
                Console.WriteLine($"Phương tiện: {pt.TenPhuongTien,-20} | Chi phí: {pt.TinhChiPhiVanHanh(150),10:N0} VNĐ");
            }
        }

        static void ChayBaiTap8()
        {
            Console.WriteLine("=== BÀI 8: QUẢN LÝ TÀI LIỆU THƯ VIỆN ===");
            List<TaiLieu> dsTL = new List<TaiLieu>
            {
                new Sach("TL01", "NXB Giáo Dục", 1000, "Nguyễn Du", 350),
                new TapChi("TL02", "NXB Trẻ", 500, 12, 10),
                new LuanVan("TL03", "ĐH Công Nghệ Đông Á", 10, "Trần Văn A", "CNTT")
            };

            Console.WriteLine("--- DANH SÁCH TẤT CẢ TÀI LIỆU ---");
            foreach (var tl in dsTL) tl.HienThi();

            Console.WriteLine("\n--- LỌC TÀI LIỆU LÀ SÁCH ---");
            foreach (var tl in dsTL.OfType<Sach>()) tl.HienThi();
        }

        static void ChayBaiTap9()
        {
            Console.WriteLine("=== BÀI 9: HỆ THỐNG THANH TOÁN ĐA PHƯƠNG THỨC ===");
            decimal soTienHoaDon = 1500000m;

            List<IPayment> dsThanhToan = new List<IPayment>
            {
                new ThanhToanTienMat(1000000m),
                new ChuyenKhoan("1020304050", 5000000m),
                new ViDienTu("0987654321", 2000000m)
            };

            Console.WriteLine($"Hóa đơn cần thanh toán: {soTienHoaDon:N0} VNĐ\n");

            foreach (var pt in dsThanhToan)
            {
                Console.WriteLine($"Thử thanh toán qua: {pt.GetTenPhuongThuc()}");
                if (pt.ThanhToan(soTienHoaDon, out string log))
                {
                    Console.WriteLine("-> THÀNH CÔNG: " + log);
                    break;
                }
                else
                {
                    Console.WriteLine("-> THẤT BẠI: " + log);
                }
                Console.WriteLine();
            }
        }

        // --- BÀI 10: QUẢN LÝ SINH VIÊN CONSOLE APP ---

        static List<SinhVienNangCao> dsSV10 = new List<SinhVienNangCao>
        {
            new SVChinhQuy10("SV01", "Nguyễn Văn An", new DateTime(2005, 3, 10), 8.5, 18),
            new SVLienThong10("SV02", "Trần Thị Bình", new DateTime(2003, 7, 22), 7.2, 15),
            new SVQuocTe10("SV03", "John Terry", new DateTime(2004, 11, 5), 9.0, 15000000m)
        };

        static void ChayBaiTap10()
        {
            int subChon = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("   BÀI 10: NÂNG CẤP QUẢN LÝ SINH VIÊN (LAB 3)     ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Xem danh sách tất cả sinh viên");
                Console.WriteLine("2. Thêm sinh viên mới (Chính quy / Liên thông / Quốc tế)");
                Console.WriteLine("3. Tìm kiếm theo Mã, Tên hoặc Loại sinh viên");
                Console.WriteLine("4. Sắp xếp danh sách theo Học phí (Giảm dần)");
                Console.WriteLine("5. Thống kê tổng học phí theo từng loại sinh viên");
                Console.WriteLine("0. Quay lại Menu chính");
                Console.WriteLine("==================================================");
                Console.Write("Lựa chọn chức năng: ");

                if (!int.TryParse(Console.ReadLine(), out subChon)) subChon = -1;

                Console.WriteLine("\n--------------------------------------------------");
                switch (subChon)
                {
                    case 1: XemDanhSach10(); break;
                    case 2: ThemSinhVien10(); break;
                    case 3: TimKiem10(); break;
                    case 4: SapXep10(); break;
                    case 5: ThongKe10(); break;
                    case 0: break;
                    default: Console.WriteLine("Lựa chọn không hợp lệ."); break;
                }

                if (subChon != 0)
                {
                    Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục trong Bài 10...");
                    Console.ReadKey();
                }

            } while (subChon != 0);
        }

        static void XemDanhSach10()
        {
            Console.WriteLine("--- DANH SÁCH SINH VIÊN ---");
            if (dsSV10.Count == 0) { Console.WriteLine("Danh sách trống!"); return; }
            foreach (var sv in dsSV10) Console.WriteLine(sv.ToDisplayText());
        }

        static void ThemSinhVien10()
        {
            Console.WriteLine("--- THÊM SINH VIÊN MỚI ---");
            Console.WriteLine("Chọn loại: 1. Chính quy | 2. Liên thông | 3. Quốc tế");
            Console.Write("Lựa chọn: ");
            string loai = Console.ReadLine();

            try
            {
                Console.Write("Mã SV: "); string ma = Console.ReadLine()?.Trim();
                if (dsSV10.Any(x => x.MaSV.Equals(ma, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Lỗi: Mã SV đã tồn tại!");
                    return;
                }

                Console.Write("Họ tên: "); string ten = Console.ReadLine()?.Trim();
                Console.Write("Ngày sinh (dd/MM/yyyy): "); DateTime ns = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
                Console.Write("Điểm TB: "); double diem = double.Parse(Console.ReadLine());

                SinhVienNangCao svMoi = null;
                if (loai == "1")
                {
                    Console.Write("Số tín chỉ: "); int tc = int.Parse(Console.ReadLine());
                    svMoi = new SVChinhQuy10(ma, ten, ns, diem, tc);
                }
                else if (loai == "2")
                {
                    Console.Write("Số tín chỉ: "); int tc = int.Parse(Console.ReadLine());
                    svMoi = new SVLienThong10(ma, ten, ns, diem, tc);
                }
                else if (loai == "3")
                {
                    Console.Write("Học phí cố định (VNĐ): "); decimal hp = decimal.Parse(Console.ReadLine());
                    svMoi = new SVQuocTe10(ma, ten, ns, diem, hp);
                }
                else
                {
                    Console.WriteLine("Loại sinh viên không hợp lệ!");
                    return;
                }

                if (svMoi.IsValid(out string msg))
                {
                    dsSV10.Add(svMoi);
                    Console.WriteLine("Thêm sinh viên thành công!");
                }
                else
                {
                    Console.WriteLine("Lỗi dữ liệu: " + msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Thêm thất bại. Lỗi: " + ex.Message);
            }
        }

        static void TimKiem10()
        {
            Console.Write("Nhập từ khóa (Mã, Tên hoặc Loại sinh viên): ");
            string kw = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(kw)) return;

            var kq = dsSV10.Where(x =>
                x.MaSV.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.HoTen.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.GetLoaiSinhVien().IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            Console.WriteLine($"Tìm thấy {kq.Count} kết quả:");
            foreach (var sv in kq) Console.WriteLine(sv.ToDisplayText());
        }

        static void SapXep10()
        {
            dsSV10 = dsSV10.OrderByDescending(x => x.TinhHocPhi()).ToList();
            Console.WriteLine("Đã sắp xếp danh sách theo học phí giảm dần!");
            XemDanhSach10();
        }

        static void ThongKe10()
        {
            Console.WriteLine("--- THỐNG KÊ THEO LOẠI SINH VIÊN ---");
            var thongKe = dsSV10.GroupBy(x => x.GetLoaiSinhVien())
                                .Select(g => new
                                {
                                    Loai = g.Key,
                                    SoLuong = g.Count(),
                                    TongHocPhi = g.Sum(x => x.TinhHocPhi())
                                });

            foreach (var item in thongKe)
            {
                Console.WriteLine($"- Loại [{item.Loai,-10}]: Số lượng = {item.SoLuong,2} | Tổng học phí = {item.TongHocPhi,14:N0} VNĐ");
            }
        }

        #endregion
    }
}