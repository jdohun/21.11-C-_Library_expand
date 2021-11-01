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
    public partial class Form2 : Form {
        Form1 form1;

        public Form2() {
            InitializeComponent();
        }

        public Form2(Form1 _form1) {
            InitializeComponent();

            form1 = _form1;

            Text = "도서 관리";

            dataGridView1.DataSource = DataManager.Books;
            
            /* 속성을 통해 코드 삽입하는 방법 외에 이미 디자이너에 생성되어있는 함수에 접근하는 방식
            dataGridView1.CurrentCellChanged += DataGridView1_CurrentCellChanged; */

            button1.Click += ( sender, e ) => { /* 추가 */
                if ( textBox1.Text.Trim() == "" ) {
                    MessageBox.Show("추가하려는 책의 Isbn을 입력해주세요.");
                }
                else {
                    try {
                        // textBox1.Text가 Books에 있으면 이미 존재
                        if ( DataManager.Books.Exists(x => x.Isbn == textBox1.Text) ) {
                            MessageBox.Show("이미 존재하는 도서입니다");
                        }
                        else {
                            // 새 책의 정보를 객체에 저장
                            Book book = new Book() {
                                Isbn = textBox1.Text,
                                Name = textBox2.Text,
                                Publisher = textBox3.Text,
                                Page = int.Parse(textBox4.Text)
                            };
                            // 새 책의 정보가 담긴 객체를 리스트에 추가
                            DataManager.Books.Add(book);

                            // 새로 고침
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = DataManager.Books;

                            // 파일에 저장
                            DataManager.Save();
                            form1.ViewRefresh(1);
                            MessageBox.Show("새 책\n" +
                                "ISBN : \"" + book.Isbn + "\"\n" 
                                + "도서 이름 : \"" + book.Name + "\"을/를 추가했습니다.");
                        }
                    }
                    catch (Exception err) { /* Exists의 ArgumentNullException 처리를 위한 구문 */ }
                }
            };

            button2.Click += ( sender, e ) => { /* 수정 */
                if ( textBox1.Text.Trim() == "" ) {
                    MessageBox.Show("수정하려는 책의 Isbn을 입력해주세요.");
                }
                else {
                    try {
                        Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                        book.Name = textBox2.Text;
                        book.Publisher = textBox3.Text;
                        book.Page = int.Parse(textBox4.Text);

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Books;
                        DataManager.Save();
                        form1.ViewRefresh(1);

                        MessageBox.Show("\"" + book.Isbn + "\"의 정보를 수정했습니다.");
                    }
                    catch ( Exception err ) {
                        MessageBox.Show("존재하지 않는 도서입니다.");
                    }
                }
            };
        }

        /*private void DataGridView1_CurrentCellChanged( object sender, EventArgs e ) {
            try {
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book;
                textBox1.Text = book.Isbn;
            }
            catch (Exception err) {}
        }*/

        private void dataGridView1_CurrentCellChanged( object sender, EventArgs e ) {
            try {
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book;
                textBox1.Text = book.Isbn;
                textBox2.Text = book.Name;
                textBox3.Text = book.Publisher;
                textBox4.Text = book.Page.ToString();

            }
            catch ( Exception error ) { }
        }

        private void button3_Click( object sender, EventArgs e ) {  /* 삭제 */
            if(textBox1.Text.Trim() == "" ) {
                MessageBox.Show("삭제하려는 책의 Isbn을 입력해주세요.");
            }
            else {
                try {
                    // Books에서 textBox1에 해당되는 Book 검색 후 삭제
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    DataManager.Books.Remove(book);

                    // 새로고침
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Books;
                
                    // 파일에 저장
                    DataManager.Save();
                    form1.ViewRefresh(1);

                    MessageBox.Show("ISBN : \"" + book.Isbn + "\"\n"
                        + "도서 이름 : \"" + book.Name + "\"을/를 삭제했습니다.");
                }
                catch (Exception err) {
                   MessageBox.Show("존재하지 않는 책입니다.");
                }
            }
        }
    }
}
