/* ---------- TSMFILE: doc_id(int) -> bigint 전환 + file_size 컬럼 추가 ----------
   최근 CUST_ID/DEPT_ID 등 새로 만든 PK들이 전부 bigint인데 TSMFILE.doc_id만 int로 남아있던
   불일치를 맞춘다(frmSMFILE 파일업로드 공통팝업 설계 중 발견, 2026-09-06). 0건 테이블이고
   doc_id는 PK가 아니라 느슨한 참조 컬럼(테이블명 없이 doc_type+doc_id로 어느 마스터를
   가리키는지 구분)이라 CUST_ID 전환(084)처럼 테이블을 통째로 새로 만들 필요 없이 컬럼
   타입만 바꾸면 된다. FK도 없음(사전 확인 완료).

   file_size는 목업(첨부파일 팝업)의 "크기" 컬럼 표시와 향후 업로드 용량 제한 검증에
   필요한데 원래 테이블 설계에 빠져 있어서 같이 추가한다. mime_type/storage_type/del_yn/
   retry_cnt 등은 아직 정책이 정해지지 않아 이번엔 보류 - 필요해지면 번호를 이어서 추가. */

ALTER TABLE TSMFILE ALTER COLUMN doc_id bigint NOT NULL;
GO

ALTER TABLE TSMFILE ADD file_size bigint NULL;
GO
