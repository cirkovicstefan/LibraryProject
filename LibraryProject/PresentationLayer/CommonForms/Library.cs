﻿using PresentationLayer.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class Library : Form
    {
        public Library()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
        }


        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonRestoreDown_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonUSER_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.Dock = DockStyle.Fill;
            panel2Right.Controls.Clear();
            panel2Right.Controls.Add(addUser);
        }

        private void buttonBooks_Click(object sender, EventArgs e)
        {
            Books books = new Books();
            books.Dock = DockStyle.Fill;
            panel2Right.Controls.Clear();
            panel2Right.Controls.Add(books);
        }

        private void buttonRented_Click(object sender, EventArgs e)
        {
            Retnt rented = new Retnt();
            rented.Dock = DockStyle.Fill;
            panel2Right.Controls.Clear();
            panel2Right.Controls.Add(rented);
        }

        private void buttonReturnBook_Click(object sender, EventArgs e)
        {
            ReturnBook returnBook = new ReturnBook();
            returnBook.Dock = DockStyle.Fill;
            panel2Right.Controls.Clear();
            panel2Right.Controls.Add(returnBook);
        }
    }
}
