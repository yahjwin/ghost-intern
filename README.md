# Ghost Intern

> 신입 귀신이 되어 인간들을 놀래키고 공포 에너지를 모아 퇴근하는 Unity 3D 캐주얼 게임

<p align="center">
  <img src="docs/gifs/gameplay.gif" width="750">
</p>

---

## Project Overview

**Ghost Intern(귀신인턴)**은 Unity와 C#으로 개발한 3D 미션형 캐주얼 게임입니다.

플레이어는 귀신 회사의 신입 인턴이 되어 도시를 탐색하며 NPC와 상호작용하고, NPC별 미니게임을 수행합니다. 각 미션을 완료하면 Fear Energy를 획득하며, 최종적으로 **300 Energy**를 모아 회사로 복귀하면 게임이 종료됩니다.

개별 미니게임뿐만 아니라 **플레이어 이동 → NPC 상호작용 → 미니게임 → 진행 상태 저장 → 이벤트 발생 → 엔딩**으로 이어지는 전체 게임 플레이 흐름을 구현했습니다.

---

## Project Information

| Category        | Description           |
| --------------- | --------------------- |
| Development     | Individual Project    |
| Genre           | 3D Casual / Mini Game |
| Engine          | Unity                 |
| Language        | C#                    |
| Platform        | PC                    |
| Version Control | Git / GitHub          |

---

## Gameplay

### Jump Scare

학생 NPC와 상호작용하면 시작되는 타이밍 기반 미니게임입니다.

움직이는 포인터를 성공 영역에 맞춰 학생을 놀라게 하면 미션에 성공하며 Fear Energy를 획득합니다.

<p align="center">
  <img src="docs/gifs/jump-scare.gif" width="650">
</p>

### Power Off

직장인 NPC와 상호작용하면 시작되는 기억력 기반 미니게임입니다.

화면에 제시되는 전선 순서를 기억한 뒤 제한 시간 안에 동일한 순서로 입력합니다. 총 3개의 Round를 통과하면 미션에 성공합니다.

<p align="center">
  <img src="docs/gifs/wire-game.gif" width="650">
</p>

### Guard Mission

학생과 직장인 미션을 완료하면 경비원이 등장합니다.

경비원의 행동을 확인하며 제한 시간 안에 귀신을 숨겨야 하며, Round가 진행될수록 반응 시간이 감소합니다.

<p align="center">
  <img src="docs/gifs/guard-game.gif" width="650">
</p>

---

## Key Features

### Mission & Progress System

NPC별 미션 완료 결과를 Fear Energy에 반영하고, 전체 게임 진행 상태에 따라 다음 이벤트가 발생하도록 구현했습니다.

### Scene Progress Management

메인 월드와 각 미니게임을 별도의 Scene으로 구성하고, Scene 전환 후에도 Fear Energy와 미션 완료 상태가 유지되도록 구현했습니다.

### NPC Interaction

플레이어가 NPC의 상호작용 범위에 진입하면 안내 UI를 표시하고, 입력을 통해 해당 NPC의 미니게임 Scene으로 이동하도록 구현했습니다.

### NPC Spawn

NPC의 최대 개수, 생성 주기, 생성 범위 및 플레이어와의 거리를 고려하여 NPC가 동적으로 생성되도록 구현했습니다.

### Quest System

현재 수행해야 하는 미션과 완료된 미션을 UI를 통해 확인할 수 있도록 구현했습니다.

### Progress-based Event

학생과 직장인 미션 완료 여부에 따라 경비원이 등장하도록 구성하여 플레이어의 진행 상태가 다음 콘텐츠에 영향을 미치도록 구현했습니다.

---

## Source Code

주요 게임 시스템과 미니게임 로직을 C#으로 직접 구현했습니다.

| Source                    | Description                         |
| ------------------------- | ----------------------------------- |
| `GameManager.cs`          | Fear Energy, 미션 완료 상태 및 전체 게임 진행 관리 |
| `QuestManager.cs`         | 미션 진행 상태 및 Quest UI 관리              |
| `PlayerController.cs`     | 플레이어 이동, 달리기 및 캐릭터 표현               |
| `NPCInteractable.cs`      | NPC 상호작용 및 미니게임 Scene 전환            |
| `NPCSpawner.cs`           | NPC 생성 주기, 위치 및 최대 생성 수 관리          |
| `JumpScareGameManager.cs` | Jump Scare 미니게임 진행 및 성공/실패 판정       |
| `WireGameManager.cs`      | 전선 순서 기억 미니게임 및 Round 진행 관리         |
| `GuardGameManager.cs`     | 경비원 미니게임 및 단계별 난이도 관리               |

### Source Directory

`Assets/Scripts/`

```text id="8y8x7f"
Assets/Scripts/
├── Manager/
│   ├── GameManager.cs
│   └── QuestManager.cs
│
├── MiniGame/
│   ├── JumpScareGameManager.cs
│   ├── WireGameManager.cs
│   └── GuardGameManager.cs
│
├── NPC/
│   └── NPCInteractable.cs
│
├── Player/
│   └── PlayerController.cs
│
├── Spawn/
│   └── NPCSpawner.cs
│
└── UI/
```

---

## Tech Stack

| Category        | Technology            |
| --------------- | --------------------- |
| Game Engine     | Unity                 |
| Language        | C#                    |
| UI              | Unity UI, TextMeshPro |
| Data            | PlayerPrefs           |
| Version Control | Git, GitHub           |

---

## Development Highlights

**Game Progress Management**
각 미니게임의 결과를 전체 Fear Energy와 미션 완료 상태에 연결하고, Scene이 전환되어도 진행 상태가 유지되도록 구성했습니다.

**Multiple Mini-game Systems**
타이밍, 기억력, 반응 속도 등 서로 다른 플레이 방식을 가진 3개의 미니게임을 각각 구현하고 하나의 게임 진행 구조로 연결했습니다.

**Progress-based Content**
미션 완료 상태를 기반으로 경비원 NPC와 새로운 미션이 활성화되도록 구현하여 플레이어의 진행에 따라 콘텐츠가 변화하도록 구성했습니다.

**Functional Script Structure**
게임 진행, 미니게임, NPC, 플레이어, Spawn, UI 기능을 역할별 스크립트로 분리하여 관리했습니다.

---


## Gameplay Demo
https://youtu.be/FYu-GhskwD0



## Developer

**yahjwin**
