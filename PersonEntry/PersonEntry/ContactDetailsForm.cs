using System;
using System.Drawing;
using System.Windows.Forms;

namespace PersonEntry
{
    public partial class ContactDetailsForm : Form
    {
        private PersonEntry contact;

        public ContactDetailsForm(PersonEntry contact)
        {
            //InitializeComponent();
            this.contact = contact;

            // Set form properties
            this.Text = "Contact Details";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            // Create controls
            Label lblNameTitle = new Label
            {
                Text = "Name:",
                Location = new Point(30, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblName = new Label
            {
                Text = contact.Name,
                Location = new Point(150, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            Label lblEmailTitle = new Label
            {
                Text = "Email:",
                Location = new Point(30, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblEmail = new Label
            {
                Text = contact.Email,
                Location = new Point(150, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            Label lblPhoneTitle = new Label
            {
                Text = "Phone:",
                Location = new Point(30, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblPhone = new Label
            {
                Text = contact.Phone,
                Location = new Point(150, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(150, 160),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0x8a, 0x2b, 0xe2),
                ForeColor = Color.White
            };
            btnClose.Click += (s, e) => this.Close();

            // Add controls to form
            this.Controls.Add(lblNameTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(lblEmailTitle);
            this.Controls.Add(lblEmail);
            this.Controls.Add(lblPhoneTitle);
            this.Controls.Add(lblPhone);
            this.Controls.Add(btnClose);
        }
    }
}