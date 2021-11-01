using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 도서_관리_닷넷프레임 {
    public partial class Form9 : Form {
        public Form9() {
            InitializeComponent();
        }

        public Form9(int id) {
            InitializeComponent();
            Text = "사용자 기록";
            User user = DataManager.Users.Single(x => x.Id == id);
            label2.Text = user.Id.ToString();
            label4.Text = DataManager.Records.Where(x => x.Id == id).Count().ToString();
            label6.Text = DataManager.Records.Where(x => x.Id == id && x.BookReturned).Count().ToString();
            label8.Text = DataManager.Records.Where(x => x.Id == id && (DateTime.Now - x.BorrowedAt).Days > 0).Count().ToString();

            List<Record> records = DataManager.Records.Where(x => x.Id == id).ToList();
            dataGridView1.DataSource = records;
        }
    }
}