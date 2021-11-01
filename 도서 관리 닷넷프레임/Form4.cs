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
    public partial class Form4 : Form {
        public Form4() {
            InitializeComponent();
            Text = "도서 관리";

            label7.Text = DataManager.Books.Count.ToString();
            label9.Text = DataManager.Users.Where(x => x.Id != 999).Count().ToString();
            label11.Text = DataManager.Books.Where(x => x.IsBorrowed).Count().ToString();
            label13.Text = DataManager.Books.Where(x => {
                return x.IsBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now;
            }).Count().ToString();

            dataGridView1.DataSource = DataManager.Books;
        }

        private void button2_Click( object sender, EventArgs e ) {  // 회원가입 폼 열기
            Form form5 = new Form5(this);
            form5.ShowDialog();
        }

        private void button1_Click( object sender, EventArgs e ) {  // 로그인 폼 열기
            Form form6 = new Form6(this);
            form6.ShowDialog();
        }

        private void button3_Click( object sender, EventArgs e ) {  // 도서 검색
            List<Book> Books = new List<Book>();
            string word = textBox1.Text;
            try {
                Books = DataManager.Books.Where(x => x.Name.Contains(word)).ToList<Book>();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = Books;
            }
            catch (Exception err) {
                MessageBox.Show("검색된 도서가 없습니다.");
                dataGridView1.DataSource = null;
            }
        }

        private void button4_Click( object sender, EventArgs e ) {  // 전체보기
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = DataManager.Books;
        }

        public void ViewRefresh() { // 화면 초기화
            label7.Text = DataManager.Books.Count.ToString();
            label9.Text = DataManager.Users.Where(x => x.Id != 999).Count().ToString();
            label11.Text = DataManager.Books.Where(x => x.IsBorrowed).Count().ToString();
            label13.Text = DataManager.Books.Where(x => {
                return x.IsBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now;
            }).Count().ToString();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = DataManager.Books;
        }
    }
}