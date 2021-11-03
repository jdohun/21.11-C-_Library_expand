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
    public partial class Form7 : Form {
        Form4 form4;
        User user;
        public Form7() {
            InitializeComponent();
        }

        public Form7(Form4 _form4, int id) {
            InitializeComponent();
            form4 = _form4;
            Text = "일반 도서 관리";
            
            user = DataManager.Users.Single(x => x.Id == id);
            label2.Text = user.Id.ToString();
            label4.Text = user.Name;
            
            label7.Text = DataManager.Books.Count.ToString();
            label9.Text = DataManager.Books.Where(x => x.UserId == user.Id).Count().ToString();
            label11.Text = DataManager.Books.Where(x => x.UserId == user.Id && x.BorrowedAt.AddDays(7) < DateTime.Now).Count().ToString();

            dataGridView1.DataSource = DataManager.Books;
            List<Book> myBooks = DataManager.Books.Where(x => x.UserId == id).ToList();
            dataGridView2.DataSource = myBooks;
        }

        private void button1_Click( object sender, EventArgs e ) { // 로그아웃
            this.Visible = false;
            form4.Show();
        }

        private void button2_Click( object sender, EventArgs e ) { // 내 정보
            Form8 form8 = new Form8(user.Id);
            form8.ShowDialog();
        }

        private void button3_Click( object sender, EventArgs e ) {  // 검색
            List<Book> Books = new List<Book>();
            string word = textBox1.Text;
            try {
                Books = DataManager.Books.Where(x => x.Name.Contains(word)).ToList<Book>();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = Books;
            }
            catch ( Exception err ) {
                MessageBox.Show("검색된 도서가 없습니다.");
                dataGridView1.DataSource = null;
            }
        }

        private void button4_Click( object sender, EventArgs e ) {  // 도서 다시보기
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = DataManager.Books;
        }

        private void dataGridView1_CurrentCellChanged( object sender, EventArgs e ) {
            try {
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book;
                textBox2.Text = book.Isbn;
                textBox3.Text = book.Name;
            }
            catch ( Exception error ) { }
        }

        private void Form7_FormClosed( object sender, FormClosedEventArgs e ) {
            form4.Close();
        }

        private void button5_Click( object sender, EventArgs e ) {    /* 대여 */
            if ( textBox2.Text.Trim() == "" ) {
                MessageBox.Show("찾으려는 책의 Isbn을 입력해주세요.");
            }
            else {
                try {
                    // DataManager의 Book List에서 1개의 객체만을 반환, 그 객체 x의 Isbn이 textBox1.Text와 같으면
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox2.Text);
                    int count = DataManager.Records.Where(x => x.Id == user.Id).Count();

                    if ( book.IsBorrowed ) { // 그 책이 이미 빌린 책일 경우
                        MessageBox.Show("이미 대여 중인 도서입니다.");
                    }
                    else {  // 빌리는게 가능한 경우
                        // 책 정보에 빌린 user의 Id와 이름, 빌려간 표시, 빌려간 시간 기록
                        book.UserId = user.Id;
                        book.UserName = user.Name;
                        book.IsBorrowed = true;
                        book.BorrowedAt = DateTime.Now;
                        book.LessDays = (book.BorrowedAt.AddDays(7) - DateTime.Now).Days.ToString();

                        /* 빌린 기록 */
                        Record record = new Record() {
                            Id = user.Id,
                            CountOfRec = count + 1,
                            Isbn = book.Isbn,
                            BookName = book.Name,
                            BorrowedAt = book.BorrowedAt,
                            BookReturned = false,
                            BorrowedDays = 1,
                            OverdueDays = (DateTime.Now - book.BorrowedAt).Days < 0 ? ~(DateTime.Now - book.BorrowedAt).Days+1 : 0
                        };

                        DataManager.Records.Add(record);
                        // 바뀐 정보를 파일에 저장
                        DataManager.Save();

                        viewRefresh();
                        MessageBox.Show("\"" + user.Name + "\"님이\n"
                            + "\"" + book.Name + "\"을 대여하셨습니다.");
                    }
                }
                catch ( Exception error ) {
                    MessageBox.Show("존재하지 않는 도서/사용자입니다.");
                }
            }
        }

        private void button6_Click( object sender, EventArgs e ) {  /* 반납 */
            if ( textBox2.Text.Trim() == "" ) {
                MessageBox.Show("반납하려는 책의 Isbn을 입력해주세요.");
            }
            else {
                try {
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox2.Text);
                    Record record = DataManager.Records.Single(x => x.Isbn == textBox2.Text && x.Id == user.Id && x.BookReturned == false);

                    if ( book.IsBorrowed ) {
                        if ( user.Id == book.UserId ) {   // 대여자와 사용자가 일치하는지 확인
                            DateTime dataState = book.BorrowedAt;
                            string lender = book.UserName;

                            book.UserId = 0;
                            book.UserName = "";
                            book.IsBorrowed = false;
                            book.BorrowedAt = new DateTime();
                            book.LessDays = ".";

                            record.BookReturned = true;
                            DataManager.Save();
                            
                            viewRefresh();

                            if ( dataState.AddDays(7) < DateTime.Now ) {
                                MessageBox.Show("\"" + lender + "\"님의\n"
                                    + "\"" + book.Name + "\"이/가 연체상태로 반납되었습니다.");
                            }
                            else {
                                MessageBox.Show("\"" + lender + "\"님의\n"
                                    + "\"" + book.Name + "\"이/가 반납되었습니다.");
                            }
                        }
                        else {
                            MessageBox.Show("대여자와 사용자가 일치하지 않습니다");
                        }
                    }
                    else {
                        MessageBox.Show("대여 중인 도서가 아닙니다.");
                    }
                }
                catch ( Exception error ) {
                    MessageBox.Show("존재하지 않는 도서입니다.");
                }
            }
        }

        public void viewRefresh() {
            label2.Text = user.Id.ToString();
            label4.Text = user.Name;

            label7.Text = DataManager.Books.Count.ToString();
            label9.Text = DataManager.Books.Where(x => x.UserId == user.Id).Count().ToString();
            label11.Text = DataManager.Books.Where(x => x.UserId == user.Id && x.BorrowedAt.AddDays(7) < DateTime.Now).Count().ToString();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = DataManager.Books;
            List<Book> myBooks = DataManager.Books.Where(x => x.UserId == user.Id).ToList();
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = myBooks;
        }
    }
}