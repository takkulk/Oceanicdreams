using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YachtKarbantartasiKoltsegnyilvantartoRendszer
{
    public partial class Form1 : Form
    {
        private string csvFilePath = "yacht_koltsegek_2024.csv";
        private List<Koltseg> koltsegek = new List<Koltseg>();
        private int nextId = 1;

        private DataGridView dataGridViewKoltsegek;
        private TextBox txtYachtName, txtOsszeg, txtMegjegyzes;
        private DateTimePicker dtpDatum;
        private ComboBox cmbKategoria;
        private Button btnHozzaadas;

        public Form1()
        {
            InitializeComponent();

            InitializeCustomComponents();

            InitializeKategoriaComboBox();
            LoadDataFromCsv();

            btnHozzaadas.Click += BtnHozzaadas_Click;
        }

        private void InitializeCustomComponents()
        {

            dataGridViewKoltsegek = new DataGridView() { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(700, 200) };
            txtYachtName = new TextBox() { Location = new System.Drawing.Point(10, 220), Width = 150, PlaceholderText = "Yacht neve" };
            dtpDatum = new DateTimePicker() { Location = new System.Drawing.Point(170, 220), Width = 150 };
            cmbKategoria = new ComboBox() { Location = new System.Drawing.Point(330, 220), Width = 150 };
            txtOsszeg = new TextBox() { Location = new System.Drawing.Point(490, 220), Width = 80, PlaceholderText = "Összeg" };
            txtMegjegyzes = new TextBox() { Location = new System.Drawing.Point(10, 250), Width = 300, PlaceholderText = "Megjegyzés" };
            btnHozzaadas = new Button() { Text = "Hozzáadás", Location = new System.Drawing.Point(330, 250) };


            Controls.Add(dataGridViewKoltsegek);
            Controls.Add(txtYachtName);
            Controls.Add(dtpDatum);
            Controls.Add(cmbKategoria);
            Controls.Add(txtOsszeg);
            Controls.Add(txtMegjegyzes);
            Controls.Add(btnHozzaadas);
        }

        private void InitializeKategoriaComboBox()
        {
            cmbKategoria.Items.AddRange(new string[] {
                "Maintenance",
                "Repairs",
                "Insurance",
                "Docking Fees",
                "Other"
            });
            cmbKategoria.SelectedIndex = 0;
        }

        private void LoadDataFromCsv()
        {
            if (!File.Exists(csvFilePath))
                return;

            koltsegek.Clear();

            try
            {
                var lines = File.ReadAllLines(csvFilePath, Encoding.UTF8);
                bool firstLine = true;
                foreach (var line in lines)
                {
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }
                    var fields = line.Split(';');
                    if (fields.Length >= 6)
                    {
                        var koltseg = new Koltseg
                        {
                            Id = int.Parse(fields[0]),
                            YachtName = fields[1],
                            Datum = DateTime.Parse(fields[2]),
                            Kategoria = fields[3],
                            Osszeg = decimal.Parse(fields[4]),
                            Megjegyzes = fields[5]
                        };
                        koltsegek.Add(koltseg);
                        if (koltseg.Id >= nextId)
                            nextId = koltseg.Id + 1;
                    }
                }
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a fájl betöltésekor: " + ex.Message);
            }
        }

        private void RefreshDataGrid()
        {
            dataGridViewKoltsegek.DataSource = null; 
            dataGridViewKoltsegek.DataSource = koltsegek;
        }

        private void BtnHozzaadas_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtYachtName.Text) || string.IsNullOrWhiteSpace(txtOsszeg.Text))
            {
                MessageBox.Show("Kérjük, töltse ki a kötelezõ mezõket!");
                return;
            }

            if (!decimal.TryParse(txtOsszeg.Text, out decimal osszeg))
            {
                MessageBox.Show("Érvénytelen összeg!");
                return;
            }

            var ujKoltseg = new Koltseg
            {
                Id = nextId++,
                YachtName = txtYachtName.Text,
                Datum = dtpDatum.Value.Date,
                Kategoria = cmbKategoria.SelectedItem.ToString(),
                Osszeg = osszeg,
                Megjegyzes = txtMegjegyzes.Text
            };

            koltsegek.Add(ujKoltseg);
            RefreshDataGrid();
            SaveDataToCsv();
        }

        private void SaveDataToCsv()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("id;yachtname;datum;kategoria;osszeg;megjegyzes");
                foreach (var k in koltsegek)
                {
                    sb.AppendLine($"{k.Id};{k.YachtName};{k.Datum:yyyy-MM-dd};{k.Kategoria};{k.Osszeg};{k.Megjegyzes}");
                }
                File.WriteAllText(csvFilePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a fájl mentésekor: " + ex.Message);
            }
        }
    }

    public class Koltseg
    {
        public int Id { get; set; }
        public string YachtName { get; set; }
        public DateTime Datum { get; set; }
        public string Kategoria { get; set; }
        public decimal Osszeg { get; set; }
        public string Megjegyzes { get; set; }
    }
}