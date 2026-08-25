using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Globalization;
using Newtonsoft.Json.Linq;
namespace BTLGK
{
    public partial class Form1 : Form
    {
        double nhietDoC = 0;
        double[] duBaoC = new double[5];
        bool laDoC = true;
        public Form1()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                picIcon.Visible = false;
            }
            else
            {
                picIcon.Visible = true;
            }
        }

        private void btnLM_Click(object sender, EventArgs e)
        {
            txtTP.Text = "Cao Lãnh";
            LayThoiTiet("Cao Lãnh");
            LayDuBao5Ngay("Cao Lãnh");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picIcon.Visible = true;
            lblCapNhat.Text = "Cập nhật: " + DateTime.Now.ToString("dd/MM/yyyy - HH:mm");

            txtTP.Text = "Cao Lãnh";
            LayThoiTiet("Cao Lãnh");
            LayDuBao5Ngay("Cao Lãnh");

            lblTP.Parent = picIcon;
            lblNgay.Parent = picIcon;
            lblDo.Parent = picIcon;
            lblTT.Parent = picIcon;

            lblTP.BackColor = Color.Transparent;
            lblNgay.BackColor = Color.Transparent;
            lblDo.BackColor = Color.Transparent;
            lblTT.BackColor = Color.Transparent;

            // Tên thành phố
            lblTP.AutoSize = false;
            lblTP.Size = new Size(200, 30);
            lblTP.Location = new Point(330, 20);
            lblTP.TextAlign = ContentAlignment.MiddleCenter;

            // Ngày giờ
            lblNgay.AutoSize = false;
            lblNgay.Size = new Size(200, 30);
            lblNgay.Location = new Point(330, 55);
            lblNgay.TextAlign = ContentAlignment.MiddleCenter;

            // Nhiệt độ
            lblDo.AutoSize = false;
            lblDo.Size = new Size(200, 90);
            lblDo.Location = new Point(330, 90);
            lblDo.TextAlign = ContentAlignment.MiddleCenter;

            // Trạng thái
            lblTT.AutoSize = false;
            lblTT.Size = new Size(200, 30);
            lblTT.Location = new Point(330, 235);
            lblTT.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void btnVT_Click(object sender, EventArgs e)
        {
            txtTP.Text = "Cao Lãnh";
            LayThoiTiet(txtTP.Text.Trim());
            LayDuBao5Ngay(txtTP.Text.Trim());
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            if (txtTP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên thành phố!");
                return;
            }

            LayThoiTiet(txtTP.Text.Trim());
            LayDuBao5Ngay(txtTP.Text.Trim());
        }

        public void LayThoiTiet(string thanhPho)
        {
            try
            {
                string apiKey = "02a05b274d0f289abb5227fbcc7c5a11";

                string url = $"https://api.openweathermap.org/data/2.5/weather?q={thanhPho}&appid={apiKey}&units=metric&lang=vi";

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(url);

                JObject data = JObject.Parse(json);
                int timezone = data["timezone"].Value<int>();
                DateTime gioThanhPho =
                    DateTimeOffset.UtcNow
                    .ToOffset(TimeSpan.FromSeconds(timezone))
                    .DateTime;
                if (thanhPho.ToLower() == "Cao Lãnh" || thanhPho.ToLower() == "Cao Lãnh")
                {
                    lblTP.Text = data["name"].ToString();
                }
                else
                {
                    lblTP.Text = data["name"].ToString();
                }

                nhietDoC = Convert.ToDouble(data["main"]["temp"]);
                lblDo.Text = Math.Round(nhietDoC).ToString() + "°C";

                string weather = data["weather"][0]["description"].ToString().ToLower();
                if (weather.Contains("mây đen u ám"))
                    weather = "Nhiều mây";
                else if (weather.Contains("mây rải rác"))
                    weather = "Mây rải rác";
                else if (weather.Contains("mây thưa"))
                    weather = "Ít mây";

                lblTT.Text = weather;
                string duongDan = Application.StartupPath + @"\Images\";
                lblTT.Text = weather;
                long sunrise = Convert.ToInt64(data["sys"]["sunrise"]);
                long sunset = Convert.ToInt64(data["sys"]["sunset"]);
                long now = Convert.ToInt64(data["dt"]);
                bool isNight = now < sunrise || now > sunset;
                switch (weather)
                {
                    case "bầu trời quang đãng":
                        {
                            if (now >= sunrise && now <= sunset)
                            {
                                picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\Sun.png");
                 
                                lblTP.ForeColor = Color.Black;
                                lblNgay.ForeColor = Color.Black;
                                lblDo.ForeColor = Color.Black;
                                lblTT.ForeColor = Color.Black;
                            }
                            else
                            {
                                picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\Toi.png");
                                
                                lblTP.ForeColor = Color.White;
                                lblNgay.ForeColor = Color.White;
                                lblDo.ForeColor = Color.White;
                                lblTT.ForeColor = Color.White;
                            }
                            break;
                        }

                    case "mưa":
                    case "mưa nhẹ":
                    case "mưa vừa":
                    case "mưa lớn":
                    case "mưa rất to":
                    case "mưa phùn":
                        picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\Mua.png");
                        lblTP.ForeColor = Color.Black;
                        lblNgay.ForeColor = Color.Black;
                        lblDo.ForeColor = Color.Black;
                        lblTT.ForeColor = Color.Black;
                        break;

                    case "dông":
                    case "dông kèm mưa":

                        picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\Bao.png");

                        lblTP.ForeColor = Color.White;
                        lblNgay.ForeColor = Color.White;
                        lblDo.ForeColor = Color.White;
                        lblTT.ForeColor = Color.White;
                        break;

                    case "sương mù":
                    case "sương":
                    case "khói mù":

                        picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\suongmu.png");
                        
                        lblTP.ForeColor = Color.Black;
                        lblNgay.ForeColor = Color.Black;
                        lblDo.ForeColor = Color.Black;
                        lblTT.ForeColor = Color.Black;
                        break;

                    default:

                        if (isNight)
                        {
                            picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\Toi.png");
                            
                            lblTP.ForeColor = Color.White;
                            lblNgay.ForeColor = Color.White;
                            lblDo.ForeColor = Color.White;
                            lblTT.ForeColor = Color.White;
                        }
                        else
                        {
                            picIcon.Image = Image.FromFile(Application.StartupPath + @"\Images\Sun.png");
                            
                            lblTP.ForeColor = Color.Black;
                            lblNgay.ForeColor = Color.Black;
                            lblDo.ForeColor = Color.Black;
                            lblTT.ForeColor = Color.Black;
                        }

                        break;
                }

                lblDA.Text = data["main"]["humidity"].ToString() + "%";
                lblAS.Text = data["main"]["pressure"].ToString() + " hPa";
                double gio = Convert.ToDouble(data["wind"]["speed"]);
                lblGio.Text = (gio * 3.6).ToString("0.0") + " km/h";
                lblTN.Text = (Convert.ToDouble(data["visibility"]) / 1000) + " km";

                DateTime moc = DateTimeOffset.FromUnixTimeSeconds(sunrise).ToLocalTime().DateTime;
                DateTime lan = DateTimeOffset.FromUnixTimeSeconds(sunset).ToLocalTime().DateTime;

                lblMoc.Text = moc.ToString("HH:mm");
                lblLan.Text = lan.ToString("HH:mm");

                lblNgay.Text = gioThanhPho.ToString(
                    "dddd, dd/MM/yyyy - HH:mm",
                    new CultureInfo("vi-VN"));

                // Lượng mưa
                if (data["rain"] != null && data["rain"]["1h"] != null)
                {
                    lblMua.Text = data["rain"]["1h"].ToString() + " mm";
                }
                else
                {
                    lblMua.Text = "0 mm";
                }
                lblCapNhat.Text = "Cập nhật: " + gioThanhPho.ToString("dd/MM/yyyy - HH:mm");
            }
            catch (WebException)
            {
                MessageBox.Show("Không tìm thấy thành phố. Vui lòng nhập lại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private void LayDuBao5Ngay(string thanhPho)
        {
            try
            {
                string apiKey = "02a05b274d0f289abb5227fbcc7c5a11";

                string url = $"https://api.openweathermap.org/data/2.5/forecast?q={thanhPho}&appid={apiKey}&units=metric&lang=vi";

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;

                string json = client.DownloadString(url);

                JObject data = JObject.Parse(json);

                JArray list = (JArray)data["list"];

                int dem = 0;

                HashSet<string> ngayDaLay = new HashSet<string>();

                foreach (JObject item in list)
                {
                    DateTime ngay = DateTime.Parse(item["dt_txt"].ToString());
                    // Bỏ qua ngày hôm nay
                    if (ngay.Date <= DateTime.Now.Date)
                        continue;

                    string ngayKey = ngay.ToString("yyyy-MM-dd");

                    if (ngayDaLay.Contains(ngayKey))
                        continue;

                    ngayDaLay.Add(ngayKey);

                    string thu = ngay.ToString("dddd", new CultureInfo("vi-VN"));
                    string ngayText = ngay.ToString("dd/MM");
                    double temp = Convert.ToDouble(item["main"]["temp"]);
                    string nhietDo = Math.Round(temp) + "°C";
                    string trangThai = item["weather"][0]["description"].ToString();
                    if (trangThai.Contains("mây đen u ám"))
                    trangThai = "Nhiều mây";
                    else if (trangThai.Contains("mây rải rác"))
                        trangThai = "Mây rải rác";
                    else if (trangThai.Contains("mây thưa"))
                        trangThai = "Ít mây";
                    else if (trangThai.Contains("mưa nhẹ"))
                        trangThai = "Mưa nhẹ";
                    else if (trangThai.Contains("mưa vừa"))
                        trangThai = "Mưa vừa";
                    else if (trangThai.Contains("bầu trời quang đãng"))
                        trangThai = "Nắng";
                    else if (trangThai.Contains("sương mù"))
                        trangThai = "Sương mù";
                    string weather = item["weather"][0]["main"].ToString();

                    dem++;
                    duBaoC[dem - 1] = temp;

                    switch (dem)
                    {
                        case 1:
                            lblThu1.Text = thu;
                            lblNgay1.Text = ngayText;
                            lblTT1.Text = trangThai;
                            lblDo1.Text = nhietDo;
                            HienThiIcon(hinh1, weather);
                            break;

                        case 2:
                            lblThu2.Text = thu;
                            lblNgay2.Text = ngayText;
                            lblTT2.Text = trangThai;
                            lblDo2.Text = nhietDo;
                            HienThiIcon(hinh2, weather);
                            break;

                        case 3:
                            lblThu3.Text = thu;
                            lblNgay3.Text = ngayText;
                            lblTT3.Text = trangThai;
                            lblDo3.Text = nhietDo;
                            HienThiIcon(hinh3, weather);
                            break;

                        case 4:
                            lblThu4.Text = thu;
                            lblNgay4.Text = ngayText;
                            lblTT4.Text = trangThai;
                            lblDo4.Text = nhietDo;
                            HienThiIcon(hinh4, weather);
                            break;

                        case 5:
                            lblThu5.Text = thu;
                            lblNgay5.Text = ngayText;
                            lblTT5.Text = trangThai;
                            lblDo5.Text = nhietDo;
                            HienThiIcon(hinh5, weather);
                            break;
                    }

                    if (dem == 5)
                        break;
                }
            }
            catch (WebException)
            {
                MessageBox.Show("Không tìm thấy thành phố. Vui lòng nhập lại!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private void HienThiIcon(PictureBox pic, string weather)
        {
            string duongDan = Application.StartupPath + @"\Images\";

            switch (weather)
            {
                case "Clear":
                    pic.Image = Image.FromFile(duongDan + "iconNang.png");
                    break;

                case "Rain":
                case "Drizzle":
                    pic.Image = Image.FromFile(duongDan + "iconMua.png");
                    break;

                case "Clouds":
                    pic.Image = Image.FromFile(duongDan + "iconMay.png");
                    break;

            }
        }

        private void btnDoiDonVi_Click(object sender, EventArgs e)
        {
            if (laDoC)
            {
                // Đổi sang °F
                lblDo.Text = Math.Round(nhietDoC * 9 / 5 + 32) + "°F";

                lblDo1.Text = Math.Round(duBaoC[0] * 9 / 5 + 32) + "°F";
                lblDo2.Text = Math.Round(duBaoC[1] * 9 / 5 + 32) + "°F";
                lblDo3.Text = Math.Round(duBaoC[2] * 9 / 5 + 32) + "°F";
                lblDo4.Text = Math.Round(duBaoC[3] * 9 / 5 + 32) + "°F";
                lblDo5.Text = Math.Round(duBaoC[4] * 9 / 5 + 32) + "°F";

                btnDoiDonVi.Text = "°F";
                laDoC = false;
            }
            else
            {
                // Đổi về °C
                lblDo.Text = Math.Round(nhietDoC) + "°C";

                lblDo1.Text = Math.Round(duBaoC[0]) + "°C";
                lblDo2.Text = Math.Round(duBaoC[1]) + "°C";
                lblDo3.Text = Math.Round(duBaoC[2]) + "°C";
                lblDo4.Text = Math.Round(duBaoC[3]) + "°C";
                lblDo5.Text = Math.Round(duBaoC[4]) + "°C";

                btnDoiDonVi.Text = "°C";
                laDoC = true;
            }
        } 
    }
}
