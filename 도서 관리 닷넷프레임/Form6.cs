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
    public partial class Form6 : Form {
        Form4 form4;
        Form7 form7;
        Form1 form1;

        public Form6() {
            InitializeComponent();
        }
        public Form6(Form4 _form4) {
            InitializeComponent();
            Text = "도서 관리 : 로그인";
            form4 = _form4;
            radioButton2.Checked = true;
        }
        private void button1_Click( object sender, EventArgs e ) {
            try {
                if ( radioButton1.Checked ) {   // 관리자 로그인
                    User user = DataManager.Users.Single(x => x.Id == 999);
                    if (!textBox1.Text.Equals("999")) {
                        MessageBox.Show("관리자 아이디가 아닙니다.");
                    }
                    else if ( user.Pwd.Equals(textBox2.Text) ) {
                        this.Visible = false;
                        form4.Visible = false;
                        form1 = new Form1(form4, user.Id);
                        form1.ShowDialog();
                    }
                    else {
                        MessageBox.Show("비밀번호가 일치하지 않습니다.");
                    }
                }
                else if ( radioButton2.Checked ) {    // 일반 로그인
                    if(textBox1.Text != "999" ) {
                        User user = null;
                        user = DataManager.Users.Single(x => x.Id.ToString().Equals(textBox1.Text));
                        if ( user != null ) {
                            if ( user.Pwd.Equals(textBox2.Text) ) {
                                this.Close();
                                form4.Visible = false;
                                form7 = new Form7(form4, user.Id);
                                form7.Show();
                            }
                            else {
                                MessageBox.Show("비밀번호가 일치하지 않습니다.");
                            }
                        }
                    }
                    else {
                        MessageBox.Show("존재하지 않는 사용자입니다.");
                    }
                }
            }
            catch ( Exception err ) {
                MessageBox.Show("존재하지 않는 사용자입니다.");
            }
        }
    }
}