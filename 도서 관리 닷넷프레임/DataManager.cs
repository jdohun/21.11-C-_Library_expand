using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace 도서_관리_닷넷프레임 {
    class DataManager {

        // xml로부터 받아올 Book List, User List
        public static List<Book> Books = new List<Book>();
        public static List<User> Users = new List<User>();
        public static List<Record> Records = new List<Record>();

        /*private*/
        static DataManager() {  // 생성자
            Load();
        }

        public static void Load() {
            try {
                                //파일 경로 앞에 @를 넣음으로써 문자열 안에 백슬래시를 문자 그대로 인식하게 됨 그렇지 않으면 Escape문자로 인식하여 두번씩 연속으로 작성해야함 (테스트 결과 일반 슬래시는 상관 없음)
                                // ex) ".\\Books.xml
                /* 책 목록 읽기 */
                string booksOutput = File.ReadAllText(@"./Books.xml"); 
                XElement booksXElement = XElement.Parse(booksOutput);

                Books = ( from item in booksXElement.Descendants("book")
                          select new Book() {
                              Isbn = item.Element("isbn").Value,
                              Name = item.Element("name").Value,
                              Publisher = item.Element("publisher").Value,
                              Page = int.Parse(item.Element("page").Value),
                              UserId = int.Parse(item.Element("userId").Value),
                              UserName = item.Element("userName").Value,
                              IsBorrowed = item.Element("isBorrowed").Value != "0" ? true : false,
                              BorrowedAt = DateTime.Parse(item.Element("borrowedAt").Value),
                              LessDays = item.Element("isBorrowed").Value == "1" ? (DateTime.Parse(item.Element("borrowedAt").Value).AddDays(7)-DateTime.Now).Days.ToString() : "."
                          } ).ToList<Book>();

                /* 사용자 목록 읽기 */
                string usersOutput = File.ReadAllText(@"./Users.xml");
                XElement usersXElement = XElement.Parse(usersOutput);

                Users = ( from item in usersXElement.Descendants("user")
                          select new User() {
                              Id = int.Parse(item.Element("id").Value),
                              Name = item.Element("name").Value,
                              Pwd = item.Element("pwd").Value,
                              Birth = item.Element("birth").Value,
                              PhoneNumber = item.Element("phoneNumber").Value,
                              Email = item.Element("email").Value
                          } ).ToList<User>();

                /* 기록 목록 읽기 */
                string recordsOutput = File.ReadAllText(@"./Records.xml");
                XElement recordsXElement = XElement.Parse(recordsOutput);
                Records = ( from item in recordsXElement.Descendants("record")
                            select new Record() {
                                Id = int.Parse(item.Element("id").Value),
                                CountOfRec = int.Parse(item.Element("countOfRec").Value),
                                Isbn = item.Element("isbn").Value,
                                BookName = item.Element("bookName").Value,
                                BookReturned = item.Element("bookReturned").Value.Equals("1") ? true : false,
                                BorrowedAt = DateTime.Parse(item.Element("borrowedAt").Value),
                                BorrowedDays = item.Element("bookReturned").Value.Equals("1") ? int.Parse(item.Element("borrowedDays").Value) : ( DateTime.Now - DateTime.Parse(item.Element("borrowedAt").Value) ).Days,
                                OverdueDays = ((DateTime.Now - DateTime.Parse(item.Element("borrowedAt").Value).AddDays(7)).Days > 0 ? ( DateTime.Now - DateTime.Parse(item.Element("borrowedAt").Value).AddDays(7)).Days : 0)
                            } ).ToList<Record>();

            }
            catch ( FileNotFoundException e ) {
                Save();
            }
        }

        public static void Save() {
            /* 책 목록 */
            string booksOutput = "";
            booksOutput += "<books>\n";
            foreach ( var item in Books ) {
                booksOutput += "<book>\n";

                booksOutput += "<isbn>" + item.Isbn + "</isbn>\n";
                booksOutput += "<name>" + item.Name + "</name>\n";
                booksOutput += "<publisher>" + item.Publisher + "</publisher>\n";
                booksOutput += "<page>" + item.Page + "</page>\n";
                booksOutput += "<userId>" + item.UserId + "</userId>\n";
                booksOutput += "<userName>" + item.UserName + "</userName>\n";
                booksOutput += "<isBorrowed>" + ( item.IsBorrowed ? 1 : 0 ) + "</isBorrowed>\n";
                booksOutput += "<borrowedAt>" + item.BorrowedAt.ToLongDateString() + "</borrowedAt>\n";
                booksOutput += "<lessDays>" + item.LessDays + "</lessDays>\n";

                booksOutput += "</book>\n";
            }

            booksOutput += "</books>";

            /* 사용자 목록 */
            string usersOutput = "";
            usersOutput += "<users>\n";
            foreach ( var item in Users ) {
                usersOutput += "<user>\n";

                usersOutput += "<id>" + item.Id + "</id>\n";
                usersOutput += "<name>" + item.Name + "</name>\n";
                usersOutput += "<pwd>" + item.Pwd + "</pwd>\n";
                usersOutput += "<birth>" + item.Birth + "</birth>\n";
                usersOutput += "<phoneNumber>" + item.PhoneNumber + "</phoneNumber>\n";
                usersOutput += "<email>" + item.Email + "</email>\n";

                usersOutput += "</user>\n";
            }
            usersOutput += "</users>";

            /* 기록 목록 */
            string recordOutput = "";
            recordOutput += "<records>\n";
            foreach( var item in Records ) {
                recordOutput += "<record>\n";

                recordOutput += "<id>" + item.Id + "</id>\n";
                recordOutput += "<countOfRec>" + item.CountOfRec + "</countOfRec>\n";
                recordOutput += "<isbn>" + item.Isbn + "</isbn>\n";
                recordOutput += "<bookName>" + item.BookName + "</bookName>\n";
                recordOutput += "<bookReturned>" + (item.BookReturned ? 1 : 0) + "</bookReturned>\n";
                recordOutput += "<borrowedAt>" + item.BorrowedAt + "</borrowedAt>\n";
                recordOutput += "<borrowedDays>" + ( item.BookReturned ? item.BorrowedDays : (DateTime.Now-item.BorrowedAt).Days) + "</borrowedDays>\n";
                recordOutput += "<overdueDays>" + (item.BorrowedDays - 7 > 0 ? item.BorrowedDays - 7 : 0 ) + "</overdueDays>\n";

                recordOutput += "</record>\n";
            }
            recordOutput += "</records>";

            File.WriteAllText(@"./Books.xml", booksOutput);
            File.WriteAllText(@"./Users.xml", usersOutput);
            File.WriteAllText(@"./Records.xml", recordOutput);
        }
    }
}