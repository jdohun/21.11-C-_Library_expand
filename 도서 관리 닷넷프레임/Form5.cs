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
    public partial class Form5 : Form {
        Form4 form4;
        bool confirmID = false;
        bool confirmPW = false;

        public Form5() {
            InitializeComponent();
            Text = "도서 관리 : 회원가입";
        }

        public Form5(Form4 _form4) {
            InitializeComponent();

            form4 = _form4;

            Text = "도서 관리 : 회원가입";
            label9.ForeColor = Color.Red;
            label9.Text = "중복 검사 필요";
            label10.ForeColor = Color.Blue;
            label10.Text = "일치";
        }

        private void button1_Click( object sender, EventArgs e ) {  // 중복확인
            if(textBox3.Text != "" ) {
                int id = int.Parse(textBox3.Text);
                try {
                    if(DataManager.Users.Exists(x => x.Id == id)) {
                        MessageBox.Show("중복된 ID 입니다.");
                    }
                    else {
                        confirmID = true;
                        label9.ForeColor = Color.Blue;
                        label9.Text = "중복 검사 완료";
                    }
                }
                catch (Exception err) { /* Exist에 의해 생길 수 있는 ArgumentNullException 오류 처리 */ }
            }
            else {
                MessageBox.Show("ID를 입력하세요");
            }
        }

        private void textBox3_TextChanged( object sender, EventArgs e ) {   // Id 재입력시 중복검사 필요
            confirmID = false;
            label9.ForeColor = Color.Red;
            label9.Text = "중복 검사 필요";
        }

        private void textBox2_KeyPress( object sender, KeyPressEventArgs e ) {
            if ( !( char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) ) ) {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress( object sender, KeyPressEventArgs e ) {  // 숫자와 백스페이스만 입력가능
            if(!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back))) {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress( object sender, KeyPressEventArgs e ) {
            if ( !( char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) ) ) {
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged( object sender, EventArgs e ) {   // 비밀번호 입력
            if ( textBox4.Text.Equals(textBox5.Text) ) {
                label10.ForeColor = Color.Blue;
                label10.Text = "일치";
                confirmPW = true;
            }
            else {
                label10.ForeColor = Color.Red;
                label10.Text = "불일치";
                confirmPW = false;
            }
        }

        private void textBox5_TextChanged( object sender, EventArgs e ) {   // 비밀번호 확인 입력
            if ( textBox4.Text.Equals(textBox5.Text) ) {
                label10.ForeColor = Color.Blue;
                label10.Text = "일치";
                confirmPW = true;
            }
            else {
                label10.ForeColor = Color.Red;
                label10.Text = "불일치";
                confirmPW = false;
            }
        }

        private void button2_Click( object sender, EventArgs e ) {  // 회원가입
            if ( textBox1.Text.Trim() == "" ||
                textBox2.Text.Trim() == "" ||
                textBox3.Text.Trim() == "" ||
                textBox4.Text.Trim() == "" ||
                textBox5.Text.Trim() == "" ||
                textBox6.Text.Trim() == "" ||
                textBox7.Text.Trim() == "" ) {
                MessageBox.Show("입력되지 않은 정보가 있습니다..");
            }
            else if ( !confirmID ) {
                MessageBox.Show("ID 중복검사가 필요합니다.");
            }
            else if( !confirmPW ) {
                MessageBox.Show("비밀번호가 일치하지 않습니다.");
            }
            else if( textBox2.Text.Length != 8 ) {
                MessageBox.Show("생년월일 8자리를 입력해주세요.");
            }
            else if ( textBox6.Text.Length != 11 ) {
                MessageBox.Show("전화번호 11자리를 입력해주세요.");
            }
            else if ( !textBox7.Text.Contains("@") ) {
                MessageBox.Show("이메일 양식에 맞게 입력해주세요.");
            }
            else {
                User user = new User {
                    Id = int.Parse(textBox3.Text),
                    Pwd = textBox4.Text,
                    Name = textBox1.Text,
                    Birth = textBox2.Text,
                    PhoneNumber = textBox6.Text,
                    Email = textBox7.Text
                };
                DataManager.Users.Add(user);
                form4.ViewRefresh();
                DataManager.Save();
                MessageBox.Show(textBox3.Text + " 회원님의 가입을 축하드립니다.");
                this.Close();
            }
        }
    }
}
