/* ---------- TSMFILEHIST 신설: 파일 다운로드 이력(보안 감사용) ----------
   사용자가 첨부파일을 다운로드할 때마다 남기는 이력 테이블(사장님 지시, 2026-09-06 - 보안상
   누가 어떤 파일을 언제 받았는지 남겨야 함).

   TSMFILE(파일 원본 메타)과는 별개로 doc_type/doc_id/doc_no/file_nm을 그대로 한 번 더
   담아둔다(스냅샷) - 감사 이력은 원본 파일이 나중에 삭제/이름변경 되더라도 "그때 그 파일을
   받았다"는 사실 자체가 남아있어야 하기 때문에, TSMFILE.file_id 하나만 들고 있고 조인해서
   보여주는 방식은 쓰지 않는다(TSMFILE도 doc_type+doc_id로 마스터를 FK 없이 느슨하게
   가리키는 것과 같은 이유 - 실제로 겪은 적은 없지만 감사 로그는 원본이 사라져도 살아있어야
   하는 성격이 강하다). down_ip/down_pc/result_cd는 기존 TSMLOGINHIST(로그인 이력)의
   CLIENT_IP/RESULT_CD 컬럼과 같은 목적 - 다운로드 성공/실패 시도 모두 남긴다. */

CREATE TABLE TSMFILEHIST (
    hist_id bigint IDENTITY(1,1) NOT NULL,
    file_id bigint NOT NULL,
    doc_type varchar(10) NOT NULL,
    doc_id bigint NOT NULL,
    doc_no varchar(100) NOT NULL,
    file_nm nvarchar(200) NULL,
    down_user_id varchar(30) NOT NULL,
    down_dt datetime NOT NULL,
    down_pc nvarchar(200) NULL,
    down_ip varchar(50) NULL,
    result_cd varchar(10) NULL,
    CONSTRAINT PK_TSMFILEHIST PRIMARY KEY (hist_id)
);
GO
