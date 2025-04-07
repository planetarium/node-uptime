# Node Uptime

블록체인 노드 상태 모니터링 도구로, 블록 타임스탬프 확인을 통해 네트워크의 정상 작동 여부를 검증합니다.

## 프로젝트 개요

이 프로젝트는 Odin과 Heimdall 블록체인 네트워크의 다양한 노드(RPC, Validator)를 모니터링하여 블록 생성 지연이나 문제가 발생할 경우 PagerDuty를 통해 알림을 발송합니다.

## 주요 기능

- GraphQL API를 통한 블록체인 노드 상태 확인
- 최신 블록 타임스탬프 검증 (5분 이내 생성 여부)
- PagerDuty 알림 시스템 연동
- JWT 인증을 통한 보안 API 호출
- 자동화된 테스트 실행 및 상태 보고

## 설정 방법

1. `appsettings.json` 파일을 복사하여 `appsettings.local.json` 파일을 생성합니다.
2. 다음 설정을 업데이트합니다:
   - `Headless.JwtSecretKey`: GraphQL API 접근을 위한 JWT 시크릿 키
   - `PagerDuty.Enabled`: PagerDuty 알림 활성화 여부
   - `PagerDuty.RoutingKeys`: 각 네트워크(Odin, Heimdall)에 대한 PagerDuty 라우팅 키

```json
{
  "Headless": {
    "JwtIssuer": "NodeUptime",
    "JwtSecretKey": "your_secure_key_here"
  },
  "PagerDuty": {
    "Enabled": true,
    "RoutingKeys": {
      "Odin": "your_odin_routing_key",
      "Heimdall": "your_heimdall_routing_key"
    }
  }
}
```

## 실행 방법

```bash
dotnet test
```

## 모니터링 대상 노드

다음 노드들의 상태를 모니터링합니다:

- Odin: 
  - RPC 노드: odin-rpc-1, odin-rpc-2, odin-eks-rpc-1
  - Validator 노드: odin-validator-5
- Heimdall: 
  - RPC 노드: heimdall-rpc-1, heimdall-rpc-2, heimdall-eks-rpc-1
  - Validator 노드: heimdall-validator-1
