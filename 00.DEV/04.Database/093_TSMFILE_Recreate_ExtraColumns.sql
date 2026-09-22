/* ---------- TSMFILE 재생성: mime_type/storage_type/retry_cnt 추가 ----------
   방금 만든 신규 테이블이라(0건, 참조하는 프로시저/FK 없음) 컬럼을 하나씩 ALTER로 늘리는
   대신 DROP 후 원하는 최종 모양으로 다시 CREATE한다(사장님 지시, 2026-09-06).

   092에서 추가했던 file_size는 그대로 가져가고, 이번에 mime_type(다운로드 시 Content-Type
   헤더용)/storage_type(LOCAL/NAS 중 실제 어디에 저장됐는지)/retry_cnt(NAS 업로드 실패 후
   재시도 횟수)를 추가한다. del_yn은 이번엔 제외(사장님 지시). */

DROP TABLE TSMFILE;
GO

CREATE TABLE TSMFILE (
    file_id bigint IDENTITY(1,1) NOT NULL,
    doc_type varchar(10) NOT NULL,
    doc_id bigint NOT NULL,
    doc_no varchar(100) NOT NULL,
    doc_serl int NOT NULL,
    serl int NOT NULL,
    file_type varchar(10) NOT NULL,
    file_nm nvarchar(200) NULL,
    file_size bigint NULL,
    mime_type varchar(100) NULL,
    file_path nvarchar(200) NULL,
    storage_type varchar(10) NULL,
    form_id varchar(50) NULL,
    remark nvarchar(100) NULL,
    reg_user_id varchar(30) NULL,
    reg_dt datetime NULL,
    reg_pc nvarchar(200) NULL,
    upt_user_id varchar(30) NULL,
    upt_dt datetime NULL,
    upt_pc nvarchar(200) NULL,
    fail_yn varchar(1) NULL,
    retry_cnt int NULL,
    CONSTRAINT PK_TSMFILE PRIMARY KEY (file_id)
);
GO
