using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace baitap5
{
    public partial class Form1 : Form
    {
        private string currentFilePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nạp danh sách Font hệ thống
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            cmbFont.Items.Clear();
            foreach (FontFamily font in installedFonts.Families)
            {
                cmbFont.Items.Add(font.Name);
            }

            // Nạp Kích thước chữ
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbSize.Items.Clear();
            foreach (int size in sizes)
            {
                cmbSize.Items.Add(size);
            }

            ResetToDefault();
        }

        private void ResetToDefault()
        {
            richText.Clear();
            currentFilePath = "";
            cmbFont.SelectedItem = "Tahoma";
            cmbSize.SelectedItem = 14;
            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);
            UpdateWordCount();
        }

        private void menuTaoMoi_Click(object sender, EventArgs e)
        {
            ResetToDefault();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetToDefault();
        }

        private void menuMoFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openDlg.FileName;
                if (Path.GetExtension(currentFilePath).ToLower() == ".rtf")
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
                UpdateWordCount();
            }
        }

        private void menuLuuFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt";
                saveDlg.DefaultExt = "rtf";

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveDlg.FileName;
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (Path.GetExtension(currentFilePath).ToLower() == ".rtf")
                {
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
                MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuDinhDang_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;

            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richText.SelectionColor = fontDlg.Color;
                richText.SelectionFont = fontDlg.Font;
            }
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Bold);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Italic);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Underline);
        }

        private void ToggleStyle(FontStyle style)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ style;
                richText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            }
        }

        private void cmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null && cmbFont.SelectedItem != null)
            {
                string fontName = cmbFont.SelectedItem.ToString();
                float fontSize = richText.SelectionFont.Size;
                FontStyle fontStyle = richText.SelectionFont.Style;

                richText.SelectionFont = new Font(fontName, fontSize, fontStyle);
            }
        }

        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null && cmbSize.SelectedItem != null)
            {
                string fontName = richText.SelectionFont.Name;
                float fontSize = Convert.ToSingle(cmbSize.SelectedItem);
                FontStyle fontStyle = richText.SelectionFont.Style;

                richText.SelectionFont = new Font(fontName, fontSize, fontStyle);
            }
        }

        private void richText_TextChanged(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            string text = richText.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                lblStatus.Text = "Tong so tu : 0";
            }
            else
            {
                string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                lblStatus.Text = $"Tong so tu : {words.Length}";
            }
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
