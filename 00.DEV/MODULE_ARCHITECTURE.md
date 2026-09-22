# 모듈 경계 - "어느 모듈에 둘지" / "어떻게 서로 열지" 결정 기준

## 0. 전제 - 모듈은 서로를 참조하지 않는다

`99.SOURCE\{AP,BA,MA,PR,SA,SM,SYS}\WYNLAB.*.csproj`는 전부 `WYNLAB.BaseForm`/
`WYNLAB.Controls`/`WYNLAB.Shared`만 참조한다. 모듈끼리는 절대 서로 참조하지 않는다
(BA가 AP를 참조하지도, 그 반대도 없음 - 형제 모듈은 컴파일타임에 완전히 독립).

이 전제 때문에 "다른 모듈의 화면/기능을 써야 한다"는 상황이 생기면 항상 아래 둘 중
하나를 선택해야 한다. 어느 쪽인지는 "얼마나 자주 쓰는가"가 아니라 **아래 두 질문**으로
판단한다.

## 1. 판단 기준 (질문 두 개)

**질문 1 - 소유권**: 이 화면/기능이 특정 모듈의 업무 데이터를 다루는가, 아니면 어느
업무에도 속하지 않는 범용 기능인가?
- 작업지시정보는 생산(MA)의 업무 데이터(작업지시/공정/BOM)를 다룬다 - MA 소유.
- 전자결재/파일첨부는 `doc_type`(문자열 파라미터) 하나로 어떤 모듈의 어떤 문서든
  다룬다 - 코드 자체가 특정 업무를 모른다. "어느 모듈 소속"이라는 말이 성립 안 한다.

**질문 2 - 결합도**: 호출한 쪽이 결과값을 즉시 받아서 자기 로직을 이어가야 하는가?
- SA가 MA 작업지시 화면을 열 때는 "열어서 보여주면 끝" - 돌려받을 게 없다.
- 전자결재는 호출한 화면이 `bool`(결재상태 바뀜 여부)을 즉시 받아서 재조회할지
  결정해야 한다. 리플렉션으로 이걸 받으려면 Task를 박싱/언박싱하는 코드를 매번
  반복해야 해서 번거롭다.

## 2. 결론

| | 질문1: 특정 모듈 소유 | 질문1: 누구 소유도 아님(범용) |
|---|---|---|
| **질문2: 결과값 불필요** | **리플렉션**(`ModuleLoader`) | 리플렉션도 가능하지만 보통 범용이면 그냥 BaseForm |
| **질문2: 결과값 필요** | (드묾 - 생기면 그때 다시 판단) | **BaseForm 공용 컴포넌트** |

- **리플렉션(ModuleLoader)**: `00.DEV\01.Client\WYNLAB.BaseForm\ModuleLoader.cs`의
  `ModuleLoader.EnsureLoaded(assemblyName)`으로 대상 모듈 dll을 로드하고,
  그 반환된 `Assembly` 객체에서 직접 `GetType(typeName)`을 불러 폼을 만든다
  (`Type.GetType(전체문자열)`을 쓰면 안 됨 - ModuleLoader.cs 클래스 설명의 CLR 캐싱
  함정 참고). `ShellForm.OpenMenuForm`이 메뉴를 열 때 이미 이 방식을 쓰고 있다 -
  SA→MA 작업지시 조회 같은 케이스는 이 패턴을 그대로 재사용하면 된다(새 헬퍼를
  또 만들 필요 없음).
- **공용 컴포넌트 - 별도 프로젝트(`WYNLAB.Popup`)**: `popFileUpload`(파일첨부),
  `popApp`(전자결재), `popPopUp`(팝업조회)이 여기 산다
  (`99.SOURCE\POPUP\WYNLAB.Popup\` - 처음엔 `00.DEV\01.Client\WYNLAB.Popup\`에 뒀다가
  2026-09-12에 다른 화면 모듈(AP/BA/...)과 같은 자리인 `99.SOURCE` 밑으로 옮겼다. 물리적
  위치만 바뀌었을 뿐, 빌드/배포 역할은 그대로 CoreAssembly 소속이다 - `.sln`을 일부러 안
  만들어서 `Deploy-Package.ps1`의 "99.SOURCE 밑 전부 훑는 모듈 스캔"에 `Modules\POPUP\`으로
  잘못 잡히지 않는다). **`WYNLAB.BaseForm`엔 이런 화면을 두지 않는다** - BaseForm은 "어떤
  모듈에도 한정되지 않는 기반 프레임워크"(`BaseForm` 클래스/`Session`/`ApiClient`/
  `ModuleLoader` 등)라는 개념이고, 실제로 열리는 화면(폼)은 별도 계층이라는 원칙(2026-09-12
  확정). 모든 모듈이 `WYNLAB.BaseForm`/`Controls`/`Shared`처럼 이 dll도 직접 참조해서
  `await popFileUpload.ShowAsync(...)`처럼 바로 부른다.
  - 참조 방향: `WYNLAB.Popup` → `WYNLAB.BaseForm`/`Controls`/`Shared` (반대 아님 - BaseForm이
    Popup을 참조하면 순환참조가 된다).
  - `WYNLAB.Controls`의 `PopupLookupProvider.OpenPopup` 같은 IoC 훅에 `popPopUp.ShowAsync`를
    연결하는 코드는 `ControlDataSources.Initialize()`(BaseForm)가 아니라
    `WYNLAB.Popup.PopupWiring.Initialize()`(Popup)가 한다 - `Program.cs`가 앱 시작 시 둘 다
    호출한다.
  - `ProcData.ToDataTable`처럼 원래 같은 어셈블리라 `internal`로 열어뒀던 멤버는
    `WYNLAB.BaseForm.csproj`의 `<InternalsVisibleTo Include="WYNLAB.Popup" />`로 계속
    internal 접근을 허용한다(public으로 올려서 전체 모듈에 노출하지 않기 위함).

## 3. 트레이드오프

리플렉션 방식은 컴파일타임 타입체크가 없다 - 클래스명 오타는 실행해봐야(그 메뉴를
열어봐야) 드러난다. 지금 메뉴 시스템 전체가 이미 감수하고 있는 부분이라 새로울 건
없지만, 화면 코드에서 직접 이 방식을 쓸 땐 이 제약을 인지하고 쓸 것.
