using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PersonEntry
{
    public partial class AddContactForm : Form
    {
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        
       
        public AddContactForm()
        {
            //InitializeComponent();

            // Set form properties
            this.Text = "Add New Contact";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // Create controls
            Label lblNameTitle = new Label
            {
                Text = "Name:",
                Location = new Point(30, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtName = new TextBox
            {
                Location = new Point(150, 30),
                Size = new Size(200, 20)
            };

            Label lblEmailTitle = new Label
            {
                Text = "Email:",
                Location = new Point(30, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtEmail = new TextBox
            {
                Location = new Point(150, 70),
                Size = new Size(200, 20)
            };

            Label lblPhoneTitle = new Label
            {
                Text = "Phone:",
                Location = new Point(30, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtPhone = new TextBox
            {
                Location = new Point(150, 110),
                Size = new Size(200, 20)
            };

            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(100, 180),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0x8a, 0x2b, 0xe2),
                ForeColor = Color.White,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(220, 180),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            // Add event handlers
            btnSave.Click += BtnSave_Click;

            // Add controls to form
            this.Controls.Add(lblNameTitle);
            this.Controls.Add(txtName);
            this.Controls.Add(lblEmailTitle);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPhoneTitle);
            this.Controls.Add(txtPhone);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Set accept and cancel buttons
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
           

        }


        public PersonEntry GetNewContact()
        {
            return new PersonEntry(
                txtName.Text.Trim(),
                txtEmail.Text.Trim(),
                txtPhone.Text.Trim()
            );
        }
    }
}