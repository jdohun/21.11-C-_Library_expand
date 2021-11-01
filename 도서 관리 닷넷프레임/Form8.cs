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
    public partial class Form8 : Form {
        User user;
        bool confirmPW = true;

        public Form8( int id ) {
            InitializeComponent();

            Text = "일반 : 내 정보 수정";

            user = DataManager.Users.Single(x => x.Id == id);
            label9.Text = user.Name;
            label10.Text = user.Birth;
            label11.Text = user.Id.ToString();
            textBox6.Text = user.PhoneNumber;
            textBox7.Text = user.Email;
            label12.Text = "일치";

        }

        private void textBox4_TextChanged( object sender, EventArgs e ) {
            if ( textBox4.Text.Equals(textBox5.Text) ) {
                label12.ForeColor = Color.Blue;
                label12.Text = "일치";
                confirmPW = true;
            }
            else {
                label12.ForeColor = Color.Red;
                label12.Text = "불일치";
                confirmPW = false;
            }
        }

        private void textBox5_TextChanged( object sender, EventArgs e ) {
            if ( textBox4.Text.Equals(textBox5.Text) ) {
                label12.ForeColor = Color.Blue;
                label12.Text = "일치";
                confirmPW = true;
            }
            else {
                label12.ForeColor = Color.Red;
                label12.Text = "불일치";
                confirmPW = false;
            }
        }

        private void button2_Click( object sender, EventArgs e ) {  // 수정하기
            if ( textBox4.Text.Trim().Equals("") && textBox5.Text.Trim().Equals("") ) { // 비밀번호 변경 안할 때
                if ( textBox6.Text.Length != 11 ) {
                    MessageBox.Show("전화번호 11자리를 입력해주세요.");
                }
                else if ( !textBox7.Text.Contains("@") ) {
                    MessageBox.Show("이메일 양식에 맞게 입력해주세요.");
                }
                else {
                    user.PhoneNumber = textBox6.Text;
                    user.Email = textBox7.Text;

                    DataManager.Save();
                    MessageBox.Show(user.Name + "님의 정보가 수정되었습니다.");
                    this.Close();
                    return;
                }
            }
            else {   // 비밀번호 변경 할 때
                if ( confirmPW && !textBox4.Text.Equals(user.Pwd) ) {   // 새 비밀번호끼리 일치하고 현재 비밀번호와 다를 때
                    user.Pwd = textBox4.Text;
                    user.PhoneNumber = textBox6.Text;
                    user.Email = textBox7.Text;

                    DataManager.Save();
                    MessageBox.Show(user.Name + "님의 정보가 수정되었습니다.");
                    this.Close();
                }
                else if ( confirmPW && textBox4.Text.Equals(user.Pwd) ) {  // 일치하나 현재 비밀번호와 동일
                    MessageBox.Show(user.Name + "기존 비밀번호와 동일합니다.");
                }
                else {
                    MessageBox.Show("새 비밀번호가 일치하지 않습니다.");
                    this.Close();
                }
            }

        }
    }
}