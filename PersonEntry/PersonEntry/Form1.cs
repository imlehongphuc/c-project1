using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonEntry
{
    public partial class Form1 : Form
    {
        // Declare some UI features
        private int hoveredIndex = -1;
        private List<PersonEntry> contacts = new List<PersonEntry>();
        private RichTextBox addTextBox;
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPhone;

        private List<PersonEntry> GenerateRandomContacts(int count)
        {
            string[] firstNames = { "John", "Jane", "Bob", "Alice", "Charlie", "Emily", "Daniel", "Olivia", "Michael", "Sophia" };
            string[] lastNames = { "Doe", "Smith", "Johnson", "Williams", "Brown", "Davis", "Miller", "Wilson", "Taylor", "Anderson" };
            string[] emailDomains = { "example.com", "mail.com", "test.org", "demo.net" };
            string[] countryCodes = { "+1", "+44", "+61", "+84", "+49", "+33", "+81" }; // US, UK, AU, VN, DE, FR, JP

            Random random = new Random();
            List<PersonEntry> randomContacts = new List<PersonEntry>();
            HashSet<string> usedNames = new HashSet<string>();

            int attempts = 0;

            while (randomContacts.Count < count && attempts < count * 5)
            {
                attempts++;

                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                string name = $"{firstName} {lastName}";

                // Avoid duplicate full names in the same batch
                if (usedNames.Contains(name)) continue;
                usedNames.Add(name);

                // Create random email with number suffix for uniqueness
                string email = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(100, 999)}@{emailDomains[random.Next(emailDomains.Length)]}";

                // Generate international phone number
                string countryCode = countryCodes[random.Next(countryCodes.Length)];
                string phone = $"{countryCode} {random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(1000, 9999)}";

                randomContacts.Add(new PersonEntry(name, email, phone));
            }

            return randomContacts;
        }

        public Form1()
        {
            InitializeComponent();

            panelHeader.Paint += panelHeader_Paint;
            this.Resize += Form1_Resize;

            // Custom all LISTBOX features
            listItems.DrawMode = DrawMode.OwnerDrawFixed;
            listItems.ItemHeight = 50;
            listItems.BorderStyle = BorderStyle.None;

            listItems.DrawItem += listItems_DrawItem;
            listItems.MouseMove += listItems_MouseMove;
            listItems.MouseLeave += (s, e) => { hoveredIndex = -1; listItems.Invalidate(); };
            listItems.SelectedIndexChanged += listItems_SelectedIndexChanged;

            // Set up dialogs functions before using for files
            saveFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";
            openFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";

            // Load functions file txt
            LoadContactsFromFile("contacts.txt");

            // Add contacts to listbox
            foreach (var contact in contacts)
            {
                listItems.Items.Add(contact);
            }
        }

        // Function load file from directory or file name
        private void LoadContactsFromFile(string filePath)
        {
            try
            {
                contacts.Clear();
                listItems.Items.Clear();

                bool fileExists = File.Exists(filePath);
                bool fileIsEmpty = fileExists && new FileInfo(filePath).Length == 0;

                if (!fileExists || fileIsEmpty)
                {
                    var generated = GenerateRandomContacts(5);
                    contacts.AddRange(generated);
                    SaveContactsToFile(filePath);
                }
                else
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 3)
                        {
                            string name = parts[0].Trim();
                            string email = parts[1].Trim();
                            string phone = parts[2].Trim();

                            contacts.Add(new PersonEntry(name, email, phone));
                        }
                    }
                }

                // Add to list box
                foreach (var contact in contacts)
                {
                    listItems.Items.Add(contact);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading contacts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Function that save contacts to file
        private void SaveContactsToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var contact in contacts)
                    {
                        writer.WriteLine($"{contact.Name},{contact.Email},{contact.Phone}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving contacts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        // Function resize elements items in case form is resized 
        private void Form1_Resize(object sender, EventArgs e)
        {
            panelHeader.Invalidate(); // Repaint gradient
        }


        // Function custom UI of list items
        private void listItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;
            bool hovered = (e.Index == hoveredIndex);

            // Background
            using (SolidBrush bgBrush = new SolidBrush(hovered ? Color.FromArgb(250, 250, 250) : Color.White))
            {
                g.FillRectangle(bgBrush, bounds);
            }

            // Bottom border (like CSS border-bottom)
            using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                g.DrawLine(pen, bounds.Left + 10, bounds.Bottom - 1, bounds.Right - 10, bounds.Bottom - 1);
            }

            // Text
            string itemText = listItems.Items[e.Index].ToString();
            using (Font font = new Font("Segoe UI", 10, FontStyle.Regular))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(51, 51, 51)))
            {
                g.DrawString(itemText, font, textBrush,
                    new RectangleF(bounds.Left + 20, bounds.Top + 15, bounds.Width - 40, bounds.Height - 20));
            }
        }


        // Function custom hover when mouse move in items
        private void listItems_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listItems.IndexFromPoint(e.Location);
            if (index != hoveredIndex)
            {
                hoveredIndex = index;
                listItems.Invalidate();
            }
        }


        // Function custom panel header UI
        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = ((Control)sender).ClientRectangle;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                Color.FromArgb(0x8a, 0x2b, 0xe2),
                Color.FromArgb(0xff, 0x69, 0xb4),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            string title = "Email Address Book (BETA)";
            using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                // Measure function to center the text title
                SizeF textSize = e.Graphics.MeasureString(title, font);
                float x = (rect.Width - textSize.Width) / 2;
                float y = (rect.Height - textSize.Height) / 2;
                e.Graphics.DrawString(title, font, textBrush, x, y);
            }
        }

      
        // Function click to items in the list then display the specific info of that person 
        private void listItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listItems.SelectedIndex >= 0)
            {
                // Get the selected contact
                PersonEntry selectedContact = (PersonEntry)listItems.SelectedItem;

                // Show the details form
                ContactDetailsForm detailsForm = new ContactDetailsForm(selectedContact);
                detailsForm.ShowDialog();
            }
        }


        // Function when click display diaglog and validate all inputs
        private void button1_Click(object sender, EventArgs e)
        {
            AddContactForm addForm = new AddContactForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                PersonEntry newContact = addForm.GetNewContact();

                if (!IsValidEmail(newContact.Email))
                {
                    MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!IsValidInternationalPhone(newContact.Phone))
                {
                    MessageBox.Show("Please enter a valid international phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                contacts.Add(newContact);
                listItems.Items.Add(newContact);
                SaveContactsToFile("contacts.txt");
            }
        }


        // Function clicking delete button to remove items from the list

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedIndex >= 0)
            {
                // Confirm deletion
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this contact?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int index = listItems.SelectedIndex;
                    contacts.RemoveAt(index);
                    listItems.Items.RemoveAt(index);

                    // Save contacts to file
                    SaveContactsToFile("contacts.txt");
                }
            }
        }

        // Function clicking button to remove all items in the list
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete all contacts?",
                "Confirm Delete All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                contacts.Clear();
                listItems.Items.Clear();
                SaveContactsToFile("contacts.txt");
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Save contacts to the selected file
            SaveContactsToFile(saveFileDialog1.FileName);
            MessageBox.Show("Contacts saved successfully!", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            MessageBox.Show("Select a folder to save your contacts file.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Clear existing contacts
            contacts.Clear();
            listItems.Items.Clear();

            // Load contacts from the selected file
            LoadContactsFromFile(openFileDialog1.FileName);

            // Refresh the listbox
            foreach (var contact in contacts)
            {
                listItems.Items.Add(contact);
            }
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidInternationalPhone(string phone)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.IsMatch(
                    phone,
                    @"^\+[\d\s\-\(\)]+$") &&
                    new string(phone.Where(char.IsDigit).ToArray()).Length >= 8;
            }
            catch
            {
                return false;
            }
        }

    }
}