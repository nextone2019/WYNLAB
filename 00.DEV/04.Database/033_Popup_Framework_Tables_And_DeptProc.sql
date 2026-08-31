-- 팝업 프레임워크(사장님 기획) 1단계: 정의/컬럼 설정 테이블 + 첫 팝업 프로시져(SSP_POP_DEPT_Q).
--
-- 테이블 접두어를 TSM/TBA(업무 데이터)와 다르게 sys*로 둔 이유: 이 두 테이블은 어떤 업무
-- 담당자가 관리하는 데이터가 아니라, 다른 화면들이 팝업을 어떻게 그릴지 정의하는 프레임워크
-- 내부 메타데이터라서다. 쿼리분석기의 테이블 목록에서 대문자 T*(TBA/TSM) 업무 테이블 무리와
-- 시각적으로도 확실히 구분되도록 대소문자를 섞어 썼다.
--
-- SSP_POP_DEPT_Q는 USP_BA_DEPT_Q(CRUD용)와 완전히 별개의 조회 전용 프로시져다 - SSP_CBO_CODE_Q와
-- 같은 스타일(work_type 분기 없음, 검색조건 하나)로, 어느 화면에서든(메뉴 권한과 무관하게) 호출
-- 가능해야 하는 게 핵심이라 메뉴별 PROC_PREFIX 화이트리스트에 안 걸리는 별도 API(추후 작업)로 노출한다.

CREATE TABLE sysPopUpM (
    popup_key       VARCHAR(30)   NOT NULL,
    proc_nm         VARCHAR(100)  NOT NULL,
    popup_nm        NVARCHAR(100) NOT NULL,
    hierarchical_yn CHAR(1)       NOT NULL DEFAULT 'N',  -- Y=트리로 표시(부서 등 계층형), N=그리드
    key_field       VARCHAR(50)   NOT NULL,              -- 결과셋 중 고유키 컬럼명
    parent_field    VARCHAR(50)   NULL,                  -- hierarchical_yn='Y'일 때만 사용
    display_field   VARCHAR(50)   NOT NULL,               -- "명칭" 컬럼명(기본 검색 대상)
    popup_width     INT           NOT NULL DEFAULT 700,   -- 팝업창 기본 가로(px) - 화면별로 재정의 가능
    popup_height    INT           NOT NULL DEFAULT 500,   -- 팝업창 기본 세로(px)
    use_yn          CHAR(1)       NOT NULL DEFAULT 'Y',
    remark          NVARCHAR(200) NULL,
    reg_user_id     VARCHAR(50)   NULL,
    reg_dt          DATETIME      NULL DEFAULT GETDATE(),
    upt_user_id     VARCHAR(50)   NULL,
    upt_dt          DATETIME      NULL,
    CONSTRAINT PK_sysPopUpM PRIMARY KEY (popup_key)
);
GO

CREATE TABLE sysPopUpD (
    popup_key      VARCHAR(30)   NOT NULL,
    column_nm      VARCHAR(50)   NOT NULL,               -- SSP_POP_*_Q 결과셋의 실제 컬럼명
    caption        NVARCHAR(50)  NULL,                    -- 그리드 헤더 표시 텍스트(NULL이면 column_nm 그대로)
    control_type   VARCHAR(10)   NOT NULL DEFAULT 'TEXT', -- TEXT/DATE/LOOKUP/CHECK
    lookup_proc_nm VARCHAR(100)  NULL,                    -- control_type='LOOKUP'일 때만 사용
    sort           INT           NOT NULL DEFAULT 0,
    width          INT           NOT NULL DEFAULT 100,    -- 컬럼 폭(px)
    visible_yn     CHAR(1)       NOT NULL DEFAULT 'Y',
    CONSTRAINT PK_sysPopUpD PRIMARY KEY (popup_key, column_nm),
    CONSTRAINT FK_sysPopUpD_sysPopUpM FOREIGN KEY (popup_key) REFERENCES sysPopUpM(popup_key)
);
GO

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_DEPT_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT dept_cd, dept_nm, par_dept_cd, dept_type, remark
    FROM TBADEPT
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR dept_cd LIKE '%' + @p_keyword + '%'
           OR dept_nm LIKE '%' + @p_keyword + '%')
    ORDER BY dept_cd;
END
GO
