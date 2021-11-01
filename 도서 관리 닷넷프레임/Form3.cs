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
    public partial class Form3 : Form {
        Form1 form1;

        public Form3() {
            InitializeComponent();
        }

        public Form3( Form1 _form ) {
            InitializeComponent();

            form1 = _form;

            dataGridView1.DataSource = DataManager.Users.Where(x => x.Id != 999).ToList();  // 관리자 계정 표시 X
            
            Text = "도서 관리";
        }

        private void dataGridView1_CurrentCellChanged( object sender, EventArgs e ) {
            try {
                User user = dataGridView1.CurrentRow.DataBoundItem as User;
                textBox1.Text = user.Id.ToString();
                textBox2.Text = user.Name;
            }
            catch (Exception err) { /* 최초 실행시 선택된 셀이 없기 때문에 오류 처리를 위한 구문 */    }
        }

        private void button1_Click( object sender, EventArgs e ) {  /* 추가 */
            Form5 form5 = new Form5();
            form5.ShowDialog();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = DataManager.Users.Where(x => x.Id != 999).ToList();  // 관리자 계정 표시 X
            form1.ViewRefresh(2);
        }

        private void button2_Click( object sender, EventArgs e ) {  /* 수정 */
            if( textBox1.Text.Trim() == "" ) {
                MessageBox.Show("수정하려는 회원의 Id를 입력해주세요.");
            }
            else if ( textBox2.Text.Trim() == "" ) {
                MessageBox.Show("삭제하려는 회원의 이름을 입력해주세요.");
            }
            else {
                try {
                    Form8 form8 = new Form8(int.Parse(textBox1.Text));
                    form8.ShowDialog();
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Users.Where(x => x.Id != 999).ToList();  // 관리자 계정 표시 X
                    form1.ViewRefresh(2);
                }
                catch (Exception err) {
                    MessageBox.Show("일치하는 사용자가 없습니다.");
                }
            }
        }

        private void button3_Click( object sender, EventArgs e ) {  // 삭제
            if ( textBox1.Text.Trim() == "" ) {
                MessageBox.Show("삭제하려는 회원의 Id를 입력해주세요.");
            }
            else if ( textBox2.Text.Trim() == "" ) {
                MessageBox.Show("삭제하려는 회원의 이름을 입력해주세요.");
            }
            else {
                try {
                    User user = DataManager.Users.Single(x => x.Id.ToString() == textBox1.Text);
                    DataManager.Users.Remove(user);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Users;
                    DataManager.Save();

                    form1.ViewRefresh(2);
                    MessageBox.Show("사용자 \"" + user.Id + "\"의 정보를 삭제했습니다.");
                }
                catch ( Exception err ) {
                    MessageBox.Show("일치하는 사용자가 없습니다.");
                }
            }
        }

        private void button4_Click( object sender, EventArgs e ) {  /* 검색 */
            List<User> users = null;
            if ( textBox1.Text.Trim() == "" ) {
                MessageBox.Show("검색하려는 회원의 Id를 입력해주세요.");
            }
            else {
                try {
                    users = DataManager.Users.Where(x => x.Id.ToString().Equals(textBox1.Text)).ToList();
                    if(!users.Any()) {
                        MessageBox.Show("일치하는 사용자가 없습니다.");
                    }
                    else {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = users;
                    }
                }
                catch ( Exception err ) {
                }
            }
        }

        private void Form3_FormClosed( object sender, FormClosedEventArgs e ) {
            this.Close();
        }
    }
}