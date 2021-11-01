using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 도서_관리_닷넷프레임 {
    class Record {
        public int Id { get; set; }
        public int CountOfRec { get; set; }
        public string Isbn { get; set; }
        public string BookName { get; set; }
        public bool BookReturned { get; set; }  // 반납 처리 상태
        public DateTime BorrowedAt { get; set; } // 빌린 시기
        public int BorrowedDays { get; set; }   // 빌린 기간
        public int OverdueDays { get; set; }    // 연체 기간
    }
}