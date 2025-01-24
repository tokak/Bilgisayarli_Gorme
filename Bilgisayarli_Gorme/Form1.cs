using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Bilgisayarli_Gorme
{
    public partial class Form1 : Form
    {
        private Bitmap orijinalResim; // Orijinal resmi saklamak için global bir değişken
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Varsayılan resim ayarlama
            pictureBoxResimGoster.Image = Image.FromFile("C:\\Users\\Murat\\source\\repos\\Bilgisayarli_Gorme\\Bilgisayarli_Gorme\\AfterLife.jpg");
            pictureBoxResimGoster.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            orijinalResim = new Bitmap("C:\\Users\\Murat\\source\\repos\\Bilgisayarli_Gorme\\Bilgisayarli_Gorme\\AfterLife.jpg");
            // ComboBox'a eleman ekleme
            comboBox1.Items.Add("Gri Yap");
            comboBox1.Items.Add("Histogram");
            comboBox1.Items.Add("KM intensty");
            comboBox1.Items.Add("KM Öklit RGB");
            comboBox1.Items.Add("KM Mahalanobis");
            comboBox1.Items.Add("KM Mahalanobis ND");
            comboBox1.Items.Add("Kenar Bulma");


            // Diziyi tanımla
            string[] dizi = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "50" };

            // ComboBox'a diziyi ekle
            foreach (string item in dizi)
            {
                comboBox2.Items.Add(item);
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            // Seçilen öğeyi al
            string selectedItem = comboBox1.SelectedItem.ToString();
            
            // Seçilen öğeye göre ilgili metodu çağır
            switch (selectedItem)
            {
                case "Gri Yap":
                    GriYap();
                    break;
                case "Histogram":
                    Histogram();
                    break;
                case "KM intensty":
                    KMintensty();
                    break;
                case "KM Öklit RGB":
                    KMOklidRgb();
                    break;
                case "KM Mahalanobis":
                    KMMahalanobis();
                    break;

                case "KM Mahalanobis ND":
                    KMMahalanobisND();
                    break;
                case "Kenar Bulma":
                    KenarBulma();
                    break;

                default:
                    MessageBox.Show("Geçerli bir seçim yapınız.");
                    break;
            }
        }

        private void ResimYukleBtn_Click(object sender, EventArgs e)
        {


            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tüm Dosyalar|*.*"; // Yalnızca resim formatlarını filtrele
                openFileDialog.Title = "Bir resim dosyası seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Seçilen dosyayı PictureBox kontrolüne yükle
                        pictureBoxResimGoster.Image = Image.FromFile(openFileDialog.FileName);
                        pictureBoxResimGoster.SizeMode = PictureBoxSizeMode.Zoom;

                        orijinalResim = new Bitmap(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        // Resim dışında bir dosya seçilmişse hata mesajı göster
                        MessageBox.Show("Geçerli bir resim dosyası seçiniz. \nHata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void GriYap()
        {
            if (orijinalResim == null)
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
                return;
            }

            Bitmap griResim = new Bitmap(orijinalResim.Width, orijinalResim.Height);

            for (int y = 0; y < orijinalResim.Height; y++)
            {
                for (int x = 0; x < orijinalResim.Width; x++)
                {
                    // Orijinal resimden pikseli al
                    Color pikselRenk = orijinalResim.GetPixel(x, y);

                    // Gri ton hesaplama (ortalama yöntemi)
                    int griDeger = (int)((pikselRenk.R + pikselRenk.G + pikselRenk.B) / 3);

                    // Gri tonlama rengi oluştur
                    Color griRenk = Color.FromArgb(griDeger, griDeger, griDeger);

                    // Pikseli gri resme ayarla
                    griResim.SetPixel(x, y, griRenk);
                }
            }

            // Gri resmi pictureBox2'de göster
            pictureBox2.Image = griResim;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void Histogram()
        {
            Bitmap resim;

            // Resim kontrolü
            if (pictureBox2.Image != null)
            {
                resim = new Bitmap(pictureBox2.Image); // Gri tonlamalı resim
            }
            else
            {
                MessageBox.Show("Lütfen gri formatlı resim yükleyin.");
                return;
            }

            // Histogram dizisi (0-255 aralığındaki gri tonlar)
            int[] histogram = new int[256];

            // Piksel değerlerini dolaşarak histogramı hesapla
            for (int y = 0; y < resim.Height; y++)
            {
                for (int x = 0; x < resim.Width; x++)
                {
                    Color pikselRenk = resim.GetPixel(x, y);
                    int griDeger = pikselRenk.R; // Gri tonlamalı resimde R, G, B aynı
                    histogram[griDeger]++;
                }
            }

            // Chart1 ayarları
            chart1.Series.Clear();
            chart1.Series.Add("GriTonHistogramı");

            // Histogram değerlerini grafiğe ekle
            for (int i = 0; i < histogram.Length; i++)
            {
                chart1.Series["GriTonHistogramı"].Points.AddXY(i, histogram[i]);
            }

            // X ekseni ayarları
            chart1.ChartAreas[0].AxisX.Minimum = 0; // X ekseninin minimum değeri
            chart1.ChartAreas[0].AxisX.Maximum = histogram.Length - 1; // X ekseninin maksimum değeri
            chart1.ChartAreas[0].AxisX.Interval = 50; // X eksenindeki aralıklar (isteğe bağlı)

            // Y ekseni ayarları
            chart1.ChartAreas[0].AxisY.Minimum = 0; // Y ekseninin minimum değeri
            chart1.ChartAreas[0].AxisY.Maximum = histogram.Max(); // Y ekseninin maksimum değeri (histogramın en büyük değeri)
            chart1.ChartAreas[0].AxisY.Interval = histogram.Max() / 10; // Y eksenindeki aralıklar (isteğe bağlı)

            // Grafik alanını daha iyi göstermek için padding ayarları (isteğe bağlı)
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0"; // X ekseni etiket formatı
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0"; // Y ekseni etiket formatı
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray; // X ekseni grid rengi
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray; // Y ekseni grid rengi
        }

        private void KMintensty()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir K değeri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int k = int.Parse(comboBox2.SelectedItem.ToString()); // Kullanıcıdan seçilen K
            int maxIterations = 100; // Maksimum iterasyon sayısı
            Bitmap grayImage = new Bitmap(pictureBoxResimGoster.Image); // pictureBoxResimGoster kullanıldı

            // Gri tonlama
            for (int x = 0; x < grayImage.Width; x++)
            {
                for (int y = 0; y < grayImage.Height; y++)
                {
                    Color pixel = grayImage.GetPixel(x, y);
                    int grayValue = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                    grayImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }

            // Başlangıç merkezlerinin belirlenmesi
            Random rand = new Random();
            List<int> centers = new List<int>();
            for (int i = 0; i < k; i++)
            {
                centers.Add(rand.Next(0, 256)); // 0-255 arası yoğunluk değerlerinden merkez seç
            }

            // Başlangıç merkezlerini ListBox'a yazdırma
            listBox1.Items.Add("Başlangıç T Durumları :");
            for (int i = 0; i < centers.Count; i++)
            {
                listBox1.Items.Add($"İlk T{i + 1}: {centers[i]}");
            }

            int[] labels = new int[grayImage.Width * grayImage.Height]; // Piksel kümelerini tutar
            bool centersUpdated;
            int iterations = 0;

            // clusterCounts'i döngü dışında tanımla
            int[] clusterCounts = new int[k];

            do
            {
                centersUpdated = false;

                // Piksel yoğunluklarını en yakın merkeze atama
                for (int x = 0; x < grayImage.Width; x++)
                {
                    for (int y = 0; y < grayImage.Height; y++)
                    {
                        int gray = grayImage.GetPixel(x, y).R;
                        int minDistance = int.MaxValue;
                        int clusterIndex = 0;

                        for (int i = 0; i < k; i++)
                        {
                            int distance = Math.Abs(gray - centers[i]);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                clusterIndex = i;
                            }
                        }
                        labels[y * grayImage.Width + x] = clusterIndex;
                    }
                }

                // Yeni merkezlerin hesaplanması
                int[] clusterSums = new int[k];
                Array.Clear(clusterSums, 0, clusterSums.Length);
                Array.Clear(clusterCounts, 0, clusterCounts.Length); // Her iterasyonda sıfırla

                for (int i = 0; i < labels.Length; i++)
                {
                    int clusterIndex = labels[i];
                    int gray = grayImage.GetPixel(i % grayImage.Width, i / grayImage.Width).R;
                    clusterSums[clusterIndex] += gray;
                    clusterCounts[clusterIndex]++;
                }

                for (int i = 0; i < k; i++)
                {
                    if (clusterCounts[i] > 0)
                    {
                        int newCenter = clusterSums[i] / clusterCounts[i];
                        if (centers[i] != newCenter)
                        {
                            centers[i] = newCenter;
                            centersUpdated = true;
                        }
                    }
                }

                // Iterasyon bilgilerini listBox'a yazdırma
                listBox1.Items.Add($"İterasyon {iterations + 1} :");
                for (int i = 0; i < centers.Count; i++)
                {
                    listBox1.Items.Add($"T{i + 1}: {centers[i]}");
                }

                iterations++;
            } while (centersUpdated && iterations < maxIterations);

            // Segmentlenmiş resmi oluşturma
            Bitmap segmentedImage = new Bitmap(grayImage.Width, grayImage.Height);
            for (int x = 0; x < grayImage.Width; x++)
            {
                for (int y = 0; y < grayImage.Height; y++)
                {
                    int clusterIndex = labels[y * grayImage.Width + x];
                    int grayValue = centers[clusterIndex];
                    segmentedImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }

            // Sonuçları ekrana yazdırma
            listBox2.Items.Clear(); // ListBox2'yi temizle
            listBox2.Items.Add("Son T Değerleri ve Piksel Sayıları:");
            for (int i = 0; i < k; i++)
            {
                listBox2.Items.Add($"Son T{i + 1}: {centers[i]} - Piksel Sayısı: {clusterCounts[i]}");
            }

            pictureBox2.Image = segmentedImage; // Segmentlenmiş resmi göster


            // K-Means Histogram Oluşturma
            chart1.Series.Clear();
            chart1.Series.Add("KmIntensityHistogramı");
            int[] histogram = new int[256];

            for (int x = 0; x < segmentedImage.Width; x++)
            {
                for (int y = 0; y < segmentedImage.Height; y++)
                {
                    int grayValue = segmentedImage.GetPixel(x, y).R;
                    histogram[grayValue]++;
                }
            }

            for (int i = 0; i < histogram.Length; i++)
            {
                chart1.Series["KmIntensityHistogramı"].Points.AddXY(i, histogram[i]);
            }

            int totalPixels = grayImage.Width * grayImage.Height;
            label15.Text = totalPixels.ToString();
            label14.Text = iterations.ToString();
        }

        private async void KMOklidRgb()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir K değeri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int K = int.Parse(comboBox2.SelectedItem.ToString());
            int maxIterations = 100;
            Bitmap originalImage = new Bitmap(pictureBoxResimGoster.Image);

            // Tüm pikselleri oku
            List<Color> pixels = new List<Color>();
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    pixels.Add(originalImage.GetPixel(x, y));
                }
            }

            // Rastgele merkez seçimi
            Random rand = new Random();
            List<Color> centroids = pixels.OrderBy(x => rand.Next()).Take(K).ToList();

            // İterasyon bilgilerini göster
            listBox1.Items.Add("Başlangıç Merkezleri:");
            for (int i = 0; i < centroids.Count; i++)
            {
                listBox1.Items.Add($"İlk T{i + 1}: R={centroids[i].R} G={centroids[i].G} B={centroids[i].B}");
            }

            int iteration = 0;
            bool changed;
            int[] labels = new int[pixels.Count];

            do
            {
                changed = false;

                // 1. Adım: Pikselleri en yakın merkeze ata
                for (int i = 0; i < pixels.Count; i++)
                {
                    double minDistance = double.MaxValue;
                    int closestCluster = 0;

                    for (int j = 0; j < centroids.Count; j++)
                    {
                        double distance = EuclideanDistance(pixels[i], centroids[j]);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestCluster = j;
                        }
                    }

                    if (labels[i] != closestCluster)
                    {
                        labels[i] = closestCluster;
                        changed = true;
                    }
                }

                // 2. Adım: Yeni merkezleri hesapla
                List<Color> newCentroids = new List<Color>();
                for (int j = 0; j < centroids.Count; j++)
                {
                    int totalR = 0, totalG = 0, totalB = 0;
                    int count = 0;

                    for (int i = 0; i < pixels.Count; i++)
                    {
                        if (labels[i] == j)
                        {
                            totalR += pixels[i].R;
                            totalG += pixels[i].G;
                            totalB += pixels[i].B;
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        newCentroids.Add(Color.FromArgb(
                            totalR / count,
                            totalG / count,
                            totalB / count));
                    }
                    else
                    {
                        newCentroids.Add(centroids[j]); // Boş kümeye yeni merkez atama
                    }
                }

                // Değişiklik kontrolü
                if (!AreCentroidsEqual(centroids, newCentroids))
                {
                    centroids = newCentroids;
                    changed = true;
                }

                // İterasyon bilgilerini göster
                listBox1.Items.Add($"\nIterasyon {iteration + 1}:");
                for (int i = 0; i < centroids.Count; i++)
                {
                    listBox1.Items.Add($"T{i + 1}: R={centroids[i].R} G={centroids[i].G} B={centroids[i].B}");
                }

                iteration++;
            } while (changed && iteration < maxIterations);

            // Sonuç resmini oluştur
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    int index = y * originalImage.Width + x;
                    resultImage.SetPixel(x, y, centroids[labels[index]]);
                }
            }

            // Piksel sayılarını ve merkez renklerini göster
            listBox2.Items.Add("\nSonuçlar:");
            for (int j = 0; j < centroids.Count; j++)
            {
                int count = labels.Count(l => l == j);

                // Merkezin RGB değerlerini al
                Color centroidColor = centroids[j];
                string clusterInfo = $"Son İterasyon T{j + 1}: " +
                                     $"R={centroidColor.R} " +
                                     $"G={centroidColor.G} " +
                                     $"B={centroidColor.B} - " +
                                     $"{count} piksel";

                listBox2.Items.Add(clusterInfo);
            }
            // İterasyon ve piksel sayısını göster
            label14.Text = iteration.ToString();
            int totalPixels = originalImage.Width * originalImage.Height;
            label15.Text = totalPixels.ToString();
            pictureBox2.Image = resultImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // Öklid mesafesi hesaplama
        private double EuclideanDistance(Color c1, Color c2)
        {
            return Math.Sqrt(
                Math.Pow(c1.R - c2.R, 2) +
                Math.Pow(c1.G - c2.G, 2) +
                Math.Pow(c1.B - c2.B, 2)
            );
        }

        // Merkezlerin eşitliğini kontrol etme
        private bool AreCentroidsEqual(List<Color> c1, List<Color> c2)
        {
            for (int i = 0; i < c1.Count; i++)
            {
                if (c1[i] != c2[i]) return false;
            }
            return true;
        }

        private void KMMahalanobis()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            // K değeri seçilmiş mi kontrol et
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir K değeri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // K değerini ve maksimum iterasyon sayısını al
            int k = int.Parse(comboBox2.SelectedItem.ToString());
            int maxIterations = 100;

            // Resmi pictureBoxResimGoster'dan al
            Bitmap grayImage = new Bitmap(pictureBoxResimGoster.Image);

            // Gri tonlama işlemi
            for (int x = 0; x < grayImage.Width; x++)
            {
                for (int y = 0; y < grayImage.Height; y++)
                {
                    Color pixel = grayImage.GetPixel(x, y);
                    int grayValue = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                    grayImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }

            // Başlangıç merkezlerini rastgele seç
            Random rand = new Random();
            List<int> centers = new List<int>();
            for (int i = 0; i < k; i++)
            {
                centers.Add(rand.Next(0, 256)); // 0-255 arası yoğunluk değerlerinden merkez seç
            }

            // Başlangıç merkezlerini listBox1'e yazdır
            listBox1.Items.Add("Başlangıç Merkezleri:");
            for (int i = 0; i < centers.Count; i++)
            {
                listBox1.Items.Add($"İlk T{i + 1}: {centers[i]}");
            }

            // Piksel kümelerini tutacak dizi
            int[] labels = new int[grayImage.Width * grayImage.Height];
            bool centersUpdated;
            int iterations = 0;

            // K-Means Mahalanobis algoritması
            do
            {
                centersUpdated = false;

                // Piksel yoğunluklarını en yakın merkeze atama (Mahalanobis mesafesi kullanarak)
                for (int x = 0; x < grayImage.Width; x++)
                {
                    for (int y = 0; y < grayImage.Height; y++)
                    {
                        int gray = grayImage.GetPixel(x, y).R;
                        double minDistance = double.MaxValue;
                        int clusterIndex = 0;

                        for (int i = 0; i < k; i++)
                        {
                            // Mahalanobis mesafesi hesapla (basitleştirilmiş)
                            double distance = Math.Abs(gray - centers[i]) / (double)centers[i];
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                clusterIndex = i;
                            }
                        }
                        labels[y * grayImage.Width + x] = clusterIndex;
                    }
                }

                // Yeni merkezlerin hesaplanması
                int[] clusterSums = new int[k];
                int[] clusterCounts = new int[k];
                Array.Clear(clusterSums, 0, clusterSums.Length);
                Array.Clear(clusterCounts, 0, clusterCounts.Length);

                for (int i = 0; i < labels.Length; i++)
                {
                    int clusterIndex = labels[i];
                    int gray = grayImage.GetPixel(i % grayImage.Width, i / grayImage.Width).R;
                    clusterSums[clusterIndex] += gray;
                    clusterCounts[clusterIndex]++;
                }

                for (int i = 0; i < k; i++)
                {
                    if (clusterCounts[i] > 0)
                    {
                        int newCenter = clusterSums[i] / clusterCounts[i];
                        if (centers[i] != newCenter)
                        {
                            centers[i] = newCenter;
                            centersUpdated = true;
                        }
                    }
                }

                // Iterasyon bilgilerini listBox1'e yazdır
                listBox1.Items.Add($"İterasyon {iterations + 1}:");
                for (int i = 0; i < centers.Count; i++)
                {
                    //listBox1.Items.Add($"T{i + 1}: {centers[i]} - Piksel Sayısı: {clusterCounts[i]}");
                    listBox1.Items.Add($"T{i + 1}: {centers[i]}");
                }

                iterations++;
            } while (centersUpdated && iterations < maxIterations);

            // Segmentlenmiş resmi oluşturma
            Bitmap segmentedImage = new Bitmap(grayImage.Width, grayImage.Height);
            for (int x = 0; x < grayImage.Width; x++)
            {
                for (int y = 0; y < grayImage.Height; y++)
                {
                    int clusterIndex = labels[y * grayImage.Width + x];
                    int grayValue = centers[clusterIndex];
                    segmentedImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }

            // Segmentlenmiş resmi pictureBox2'de göster
            pictureBox2.Image = segmentedImage;

            // Sonuçları listBox2'ye yazdır
            listBox2.Items.Add("Son Merkezler ve Piksel Sayıları:");
            // Calculate final cluster counts based on labels
            int[] finalClusterCounts = new int[k];
            foreach (int clusterIndex in labels)
            {
                finalClusterCounts[clusterIndex]++;
            }
            for (int i = 0; i < k; i++)
            {
                listBox2.Items.Add($"Son T{i + 1}: {centers[i]} - Piksel Sayısı: {finalClusterCounts[i]}");
            }

            // Toplam piksel sayısını label15'e yazdır
            int totalPixels = grayImage.Width * grayImage.Height;
            label15.Text = totalPixels.ToString();

            // Toplam iterasyon sayısını label14'e yazdır
            label14.Text = iterations.ToString();

            // Son T değerlerini chart1'de histogram olarak göster
            chart1.Series.Clear();
            chart1.Series.Add("Son T Değerleri");
            for (int i = 0; i < k; i++)
            {
                chart1.Series["Son T Değerleri"].Points.AddXY($"T{i + 1}", centers[i]);
            }
        }

        private void KMMahalanobisND()
        {
            // Listeleri temizle
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            // K değeri kontrolü
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir K değeri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int k = int.Parse(comboBox2.SelectedItem.ToString());
            int maxIterations = 300;

            // Orijinal resmi al
            Bitmap originalImage = new Bitmap(pictureBoxResimGoster.Image);

            // Piksel verilerini topla
            List<double[]> dataPoints = new List<double[]>();
            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color pixel = originalImage.GetPixel(x, y);
                    double[] features = { pixel.R, pixel.G, pixel.B };
                    dataPoints.Add(features);
                }
            }

            // Rastgele merkezler seç
            Random rand = new Random();
            List<double[]> centers = new List<double[]>();
            for (int i = 0; i < k; i++)
            {
                centers.Add(dataPoints[rand.Next(dataPoints.Count)]);
            }

            // Başlangıç merkezlerini listele
            listBox1.Items.Add("Başlangıç Merkezleri:");
            for (int i = 0; i < centers.Count; i++)
            {
                listBox1.Items.Add($"İlk T{i + 1}: [{string.Join(", ", centers[i].Select(val => Math.Round(val)))}]");
            }

            int[] labels = new int[dataPoints.Count];
            bool centersUpdated;
            int iterations = 0;

            // K-Means algoritması
            do
            {
                centersUpdated = false;

                // Her piksel için en yakın merkezi bul
                for (int i = 0; i < dataPoints.Count; i++)
                {
                    double minDistance = double.MaxValue;
                    int clusterIndex = 0;

                    for (int j = 0; j < k; j++)
                    {
                        double distance = MahalanobisDistance(dataPoints[i], centers[j]);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            clusterIndex = j;
                        }
                    }
                    labels[i] = clusterIndex;
                }

                // Yeni merkezleri hesapla
                List<double[]> newCenters = new List<double[]>();
                int[] clusterCounts = new int[k];

                for (int i = 0; i < k; i++)
                {
                    double[] newCenter = new double[dataPoints[0].Length];
                    int clusterSize = 0;

                    for (int j = 0; j < dataPoints.Count; j++)
                    {
                        if (labels[j] == i)
                        {
                            for (int dim = 0; dim < dataPoints[j].Length; dim++)
                            {
                                newCenter[dim] += dataPoints[j][dim];
                            }
                            clusterSize++;
                        }
                    }

                    if (clusterSize > 0)
                    {
                        for (int dim = 0; dim < newCenter.Length; dim++)
                        {
                            newCenter[dim] /= clusterSize;
                        }
                    }

                    newCenters.Add(newCenter);
                    clusterCounts[i] = clusterSize;
                }

                // Merkezleri güncelle
                for (int i = 0; i < k; i++)
                {
                    if (!centers[i].SequenceEqual(newCenters[i]))
                    {
                        centers[i] = newCenters[i];
                        centersUpdated = true;
                    }
                }

                // İterasyon bilgilerini listele
                listBox1.Items.Add($"\nİterasyon {iterations + 1}:");
                for (int i = 0; i < centers.Count; i++)
                {
                    listBox1.Items.Add($"T{i + 1}: [{string.Join(", ", centers[i].Select(val => Math.Round(val)))}]");
                }

                iterations++;
            } while (centersUpdated && iterations < maxIterations);

            // Segment edilmiş resmi oluştur
            Bitmap segmentedImage = new Bitmap(originalImage.Width, originalImage.Height);
            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    // DÜZELTME: Doğru indeks hesaplama
                    int index = x * originalImage.Height + y; // Önceki hatalı kod: y * originalImage.Width + x

                    int clusterIndex = labels[index];
                    double[] center = centers[clusterIndex];

                    int r = (int)Math.Round(center[0]);
                    int g = (int)Math.Round(center[1]);
                    int b = (int)Math.Round(center[2]);

                    segmentedImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            // Segment edilmiş resmi göster
            pictureBox2.Image = segmentedImage;

            // Son merkezleri ve piksel sayılarını listele
            listBox2.Items.Add("Son Merkezler ve Piksel Sayıları:");

            // Piksel sayılarını hesapla
            int[] finalClusterCounts = new int[k];
            foreach (int label in labels)
            {
                finalClusterCounts[label]++;
            }

            for (int i = 0; i < k; i++)
            {
                listBox2.Items.Add($"Son T{i + 1}: [{string.Join(", ", centers[i].Select(val => Math.Round(val)))}] - Piksel Sayısı: {finalClusterCounts[i]}");
            }

            // İterasyon ve piksel sayısını göster
            label14.Text = iterations.ToString();
            int totalPixels = originalImage.Width * originalImage.Height;
            label15.Text = totalPixels.ToString();

            // Histogram hesapla
            int[] histogram = new int[256];
            for (int x = 0; x < segmentedImage.Width; x++)
            {
                for (int y = 0; y < segmentedImage.Height; y++)
                {
                    Color pixel = segmentedImage.GetPixel(x, y);
                    int grayValue = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                    histogram[grayValue]++;
                }
            }

            // Histogramı göster
            chart1.Series.Clear();
            chart1.Series.Add("Histogram");
            for (int i = 0; i < histogram.Length; i++)
            {
                chart1.Series["Histogram"].Points.AddXY(i, histogram[i]);
            }
        }

        private double MahalanobisDistance(double[] point1, double[] point2)
        {
            double distance = 0;
            for (int i = 0; i < point1.Length; i++)
            {
                distance += Math.Pow(point1[i] - point2[i], 2);
            }
            return Math.Sqrt(distance); // Mesafeyi döndür
        }


        private void KenarBulma()
        {
            Bitmap image = new Bitmap(pictureBoxResimGoster.Image);
            Bitmap edgeDetection = EdgeDetection(image);
            pictureBox2.Image = edgeDetection;
        }

        //Kenar bulmada resmi gri formata dönüştürme
        private Bitmap GriYapFonk(Image image) 
        {
            Bitmap bitmap = new Bitmap(image);
            Bitmap CikisResmi = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++) // yükseklik ve genişlik için iç içe for
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color OkunanRenk = bitmap.GetPixel(x, y); // görüntüyü bir matrise çevirip her bir değerin rgb değerini al
                    int R = OkunanRenk.R;
                    int G = OkunanRenk.G;
                    int B = OkunanRenk.B;
                    int GriRenk = (R + G + B) / 3; // gri format için bütün rgb değerleri toplanım 3 e bölünür
                    Color DonusenRenkGri = Color.FromArgb(GriRenk, GriRenk, GriRenk);
                    CikisResmi.SetPixel(x, y, DonusenRenkGri);
                }
            }

            pictureBox2.Image = CikisResmi;

            return CikisResmi;
        }
        //Kenar bulma işlemleri Euclidean formülü kullanarak yapma
        private Bitmap EdgeDetection(Bitmap image)
        {
            // Görüntüyü gri tonlara çevir
            Bitmap gri = GriYapFonk(image);
            Bitmap buffer = new Bitmap(gri.Width, gri.Height);
            Color renk;

            // Kenar tespiti için Euclidean mesafesi kullan
            for (int i = 1; i < gri.Height - 1; i++)
            {
                for (int j = 1; j < gri.Width - 1; j++)
                {
                    // Yatay (x) ve dikey (y) yöndeki komşu piksellerin farklarını al
                    int dx = gri.GetPixel(j + 1, i).R - gri.GetPixel(j - 1, i).R;
                    int dy = gri.GetPixel(j, i + 1).R - gri.GetPixel(j, i - 1).R;

                    // Euclidean mesafesini hesapla
                    int euclidean = (int)Math.Sqrt(dx * dx + dy * dy);

                    // Euclidean değerini 0-255 aralığına sınırla
                    if (euclidean < 0) { euclidean = 0; }
                    if (euclidean > 255) { euclidean = 255; }

                    // Yeni rengi oluştur ve buffer'a ata
                    renk = Color.FromArgb(euclidean, euclidean, euclidean);
                    buffer.SetPixel(j, i, renk);
                }
            }

            return buffer;
        }

        //Matris Tersini alma
        public static double[,] Invert3x3Matrix(double[,] matrix)
        {
            // 3x3 Matris doğrulama
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
                throw new ArgumentException("Sadece 3x3 matrisler destekleniyor.");

            // Determinant hesaplama
            double determinant =
                matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) -
                matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

            if (determinant == 0)
                throw new InvalidOperationException("Matrisin tersi yok çünkü determinant sıfır.");

            // Kofaktör matris
            double[,] cofactors = new double[3, 3];
            cofactors[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]);
            cofactors[0, 1] = -(matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]);
            cofactors[0, 2] = (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

            cofactors[1, 0] = -(matrix[0, 1] * matrix[2, 2] - matrix[0, 2] * matrix[2, 1]);
            cofactors[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]);
            cofactors[1, 2] = -(matrix[0, 0] * matrix[2, 1] - matrix[0, 1] * matrix[2, 0]);

            cofactors[2, 0] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]);
            cofactors[2, 1] = -(matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0]);
            cofactors[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);

            // Adjoint (kofaktör matrisin transpozu)
            double[,] adjoint = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    adjoint[i, j] = cofactors[j, i];
                }
            }

            // Ters matris hesaplama
            double[,] inverse = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    inverse[i, j] = adjoint[i, j] / determinant;
                }
            }

            return inverse;
        }

    }

}
