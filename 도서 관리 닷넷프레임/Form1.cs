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
    public partial class Form1 : Form {
        Form4 form4;
        public Form1(Form4 _form4, int id) {
            InitializeComponent();
            Text = "도서 관리 : 관리자";

            form4 = _form4;

            label5.Text = DataManager.Books.Count.ToString();   // DataManger 생성자 최초 호출 지점
            label6.Text = DataManager.Users.Where(x => x.Id != 999).Count().ToString();       // 관리자 계정 취급 x
            label7.Text = DataManager.Books.Where( x => x.IsBorrowed).Count().ToString();    // where 절은 조건에 충족되는(True) 객체를 반환, 반환 객체 형식이 Obj이므로 count 프로퍼티가 아닌 함수 사용
            label8.Text = DataManager.Books.Where( x => {    // 대출 상태가 true 이며 대출 시작일 + 7일이 현재보다 과거(작으면)인 객체를 반환
                return x.IsBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now;
            }).Count().ToString();

            dataGridView1.DataSource = DataManager.Books;
            dataGridView2.DataSource = DataManager.Users.Where(x => x.Id != 999).ToList();  // 관리자 계정 표시 X
        }

        private void dataGridView1_CurrentCellChanged( object sender, EventArgs e ) {   // dataGridView1에서 선택한 셀이 속한 행(Row)의 정보에 접근
            try {   // 최초 실행 시에는 선택된 정보가 없기 때문에 try ~ catch 문을 이용해 오류처리
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book; // dataGridView1에서.현재 행.행의 정보를 Book으로 받음
                textBox1.Text = book.Isbn;  // 텍스트 박스에 정보를 입력
                textBox2.Text = book.Name;  // 텍스트 박스에 정보를 입력
            }
            catch (Exception error) {}
        }

        private void dataGridView2_CurrentCellChanged( object sender, EventArgs e ) {
            try {   // 최초 실행 시에는 선택된 정보가 없기 때문에 try ~ catch 문을 이용해 오류처리
                User user  = dataGridView2.CurrentRow.DataBoundItem as User;
                textBox3.Text = user.Id.ToString();
                textBox4.Text = user.Id.ToString();
            }
            catch (Exception error) {}
        }

        private void button1_Click( object sender, EventArgs e ) {  /* 대여 */
            // Trim()은 원래 string의 좌우 공백을 제거한 후 문자열을 반환하는 함수이지만 여기서는 문자열 존재여부를 확인하기 위한 용도로 사용하였다.
            if (textBox1.Text.Trim() == "" ) {
                MessageBox.Show("찾으려는 책의 Isbn을 입력해주세요.");
            }
            else if( textBox3.Text.Trim() == "" ) {
                MessageBox.Show("사용자 Id를 입력해주세요.");
            }
            else {
                // Single에서 찾아진 객체가 없을 경우 오류 처리
                try {
                    // DataManager의 Book List에서 1개의 객체만을 반환, 그 객체 x의 Isbn이 textBox1.Text와 같으면
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);

                    if ( book.IsBorrowed ) { // 그 책이 이미 빌린 책일 경우
                        MessageBox.Show("이미 대여 중인 도서입니다.");
                    }
                    else {  // 빌리는게 가능한 경우

                        // DataManager의 User List에서 1개의 객체만을 반환, 그 객체 x의 Id가 textBox3.Text와 같으면, Id는 int형이기 때문에 문자열 변환과정을 거침
                        User user = DataManager.Users.Single(x => x.Id.ToString() == textBox3.Text);
                        int count = DataManager.Records.Where(x => x.Id == user.Id).Count();

                        // 책 정보에 빌린 user의 Id와 이름, 빌려간 표시, 빌려간 시간 기록
                        book.UserId = user.Id;
                        book.UserName = user.Name;
                        book.IsBorrowed = true;
                        book.BorrowedAt = DateTime.Now;


                        // dataGridView1의 정보를 새로 고침
                        dataGridView1.DataSource = null;    // 무조건 필요한 작업은 아니나 오류가 나는 경우가 있음
                        dataGridView1.DataSource = DataManager.Books;

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

                        // 바뀐 정보를 파일에 저장
                        DataManager.Save();

                        MessageBox.Show("\"" + user.Name + "\"님이\n"
                            + "\"" + book.Name + "\"을 대여하셨습니다.");
                    }
                }
                catch (Exception error) {
                    MessageBox.Show("존재하지 않는 도서/사용자입니다.");
                }
            }
        }
        private void button2_Click( object sender, EventArgs e ) {  /* 반납 */
            if ( textBox1.Text.Trim() == "" ) {
                MessageBox.Show("반납하려는 책의 Isbn을 입력해주세요.");
            }
            else {
                try {
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    if ( book.IsBorrowed ) {
                        User user = DataManager.Users.Single(x => x.Id.ToString() == textBox3.Text);

                        if(user.Id == book.UserId ) {   // 대여자와 사용자가 일치하는지 확인
                            DateTime dataState = book.BorrowedAt;
                            string lender = book.UserName;

                            book.UserId = 0;
                            book.UserName = "";
                            book.IsBorrowed = false;
                            book.BorrowedAt = new DateTime();

                            Record record = DataManager.Records.Single(x => x.Isbn == textBox2.Text && x.Id == user.Id);
                            record.BookReturned = true;

                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = DataManager.Books;
                            DataManager.Save();

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
                catch (Exception error) {
                    MessageBox.Show("존재하지 않는 도서입니다.");
                }
            }
        }

        // 메뉴 선택시 Form 생성
        private void 도서관리ToolStripMenuItem_Click( object sender, EventArgs e ) {
            Form2 form2 =  new Form2(this);
            form2.ShowDialog();
        }

        private void 사용자관리ToolStripMenuItem_Click( object sender, EventArgs e ) {
            Form3 form3 = new Form3(this);
            form3.ShowDialog();
        }

        public void ViewRefresh( int target ) {
            // target == 1 이면 Books를 새로고침
            // target == 2 이면 Users를 새로고침

            if ( target == 1 ) {
                label5.Text = DataManager.Books.Count.ToString();
                label7.Text = DataManager.Books.Where(x => x.IsBorrowed).Count().ToString();    // where 절은 조건에 충족되는(True) 객체를 반환, 반환 객체 형식이 Obj이므로 count 프로퍼티가 아닌 함수 사용
                label8.Text = DataManager.Books.Where(x => {    // 대출 상태가 true 이며 대출 시작일 + 7일이 현재보다 과거(작으면)인 객체를 반환
                    return x.IsBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now;
                }).Count().ToString();

                dataGridView1.DataSource = null;    // 무조건 필요한 작업은 아니나 오류가 나는 경우가 있음
                dataGridView1.DataSource = DataManager.Books;
            }
            else if(target == 2 ) {
                label6.Text = DataManager.Users.Count.ToString();
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = DataManager.Users;
            }
        }

        private void Form1_FormClosed( object sender, FormClosedEventArgs e ) { // 전체 프로세스 종료
            form4.Close();
        }

        private void button3_Click( object sender, EventArgs e ) {  // 로그아웃
            this.Visible = false;
            form4.Visible = true;
        }

        private void button4_Click( object sender, EventArgs e ) {
            if(textBox4.Text != "" ) {
                int id = int.Parse(textBox4.Text);
                Form9 form9 = new Form9(id);
                form9.ShowDialog();
            }
            else {
                MessageBox.Show("기록을 보려는 사용자의 ID를 입력하세요.");
            }
        }
    }
}