# Puzzle_Survial(PUZ) Project

## 🎮 게임 소개
**『PUZ』**는 서바이벌과 퍼즐을 결합한 게임입니다.

Unity 엔진 기반으로 개발되었으며, FSM, ScriptableObject, DoTween, 상태 전환 시스템 등 다양한 기술을 활용하여 구현하였습니다.

- 장르: 3D 원거리 액션 게임

- 플랫폼: PC (Windows)

- 개발 툴: Unity 2022.3.17f1, Visual Studio, GitHub

- 개발 기간: 2025.08.13 ~ 2025.08.22


## 🖼️ 게임 화면
게임 플레이 화면입니다.
![Adobe-Express---DayAndNight-1](https://github.com/user-attachments/assets/bec74a7a-c007-4626-8802-ff9565a9dacb)
![Adobe-Express---Biled](https://github.com/user-attachments/assets/6159cd69-5926-4441-bd26-e6ef581f6a33)


## 🕹️ 플레이 방법

이동: WASD 키

공격: 마우스로 에임을 조준, 마우스 좌클릭으로 공격

아이템: 게임에서 사용되는 아이템입니다.
- 소비 아이템 : 포션, 스프, 돌
- 장비 아이템 : 도끼, 머신건, 칼, 나무
- 자원 아이템 : 풀, 돌, 나무

버프: 소비 아이템을 먹으면 버프를 얻습니다.
- 포션 : 체력 80회복, 무적 10초, 속도 5증가
- 돌 : 체력 80회복, 무적 10초, 속도 5증가, 점프력 80증가
- 스프 : 체력 30감소

게임 흐름
- 타이틀 씬에서 게임시작 버튼을 눌러서 게임을 시작합니다.
- 몬스터(곰, 좀비)를 조준해서 공격합니다.
- NPC(Amy)와 대화해서 퀘스트, 미로게임을 합니다.

게임 오버
- 체력이 0이 되면 "You Die" UI를 출력합니다.

## 🛠 사용 기술
- Unity 2022.3 LTS
- C# (게임 로직 및 시스템 구현)
- Git & GitHub & Figma (버전 관리 및 협업)
- Gamma (ppt 제작)
- TextMeshPro, DoTween (UI 시스템 구성 및 애니메이션)
- ScriptableObject (아이템 데이터 관리)
- FSM (AI 몬스터 상태 전이)
- AI Navigation(AI 몬스터 이동)


## 🌟 주요 구현 기능 
 - RayCast를 통해 총기를 구현하여 전투의 재미 향상
 - CSV파일 직렬화를 통한 NPC와의 상호작용 제작
 - ScirptableObject를 활용한 아이템과 제작 시스템 구현
 - AI Navigation, NavMesh를 조합하여 적 움직임 구현
 - 중앙 제어형 사운드 매니저 시스템 구축하여 사운드와 볼륨 조절 시스템 구현


## ✨ 구현 기능
자원 수집 및 가공 
- 플레이어가 자원을 찾아서 수집하고, 이를 가공하여 생존에 필요한 아이템을 제작할 수 있도록 합니다.
- 자원 수집과 가공 메커니즘을 구현합니다.

식사와 수분 관리
- 플레이어의 캐릭터가 식사와 수분을 관리해야 하며, 굶주림과 갈증을 방지해야 합니다.
- 식사와 수분 관리 시스템을 구현하여 생존 요소를 추가합니다.

건축 및 생존 기지 구축
- 플레이어가 기지를 건축하고 안전한 곳을 만들 수 있도록 합니다.
- 건축 및 기지 관리 메커니즘을 구현합니다.

적과의 전투
- 다양한 적과의 전투를 구현하고, 적의 AI를 제공하여 플레이어에게 도전을 제공합니다.
- 전투 시스템과 AI를 개발합니다.

생존 관리 시스템
- 플레이어의 체력, 스태미너, 온도 등 생존과 관련된 상태를 관리합니다.
- 생존 관리 시스템을 설계하고 구현합니다.

자원 리스폰
- 자원이 다시 생성되는 시스템을 구현하여 게임의 지속 가능성을 유지합니다.
- 자원 리스폰 주기 및 메커니즘을 설계합니다.

NPC와의 대화 기능
- 간단한 NPC 추가하기
- 대화 연출, 타이핑 연출

UI 애니메이션 추가
- 대화 UI 애니메이션 (Unity 기본 제공)
- 외부 라이브러리(두트윈)

날씨와 환경 요소
- 게임 세계의 날씨와 환경 요소가 플레이어의 생존에 영향을 미치도록 합니다.
- 날씨 변화, 온도, 기상 조건 등을 추가합니다.

고급 건축 시스템
- 다양한 건축 옵션과 고급 건축 요소를 추가하여 기지 건설을 더 다양하게 만듭니다.
- 건축 및 구조물 개발 시스템을 확장합니다.

다양한 적 종류
- 다양한 종류의 적을 추가하여 게임의 다양성을 높입니다.
- 다양한 동물과 적 캐릭터를 구현합니다.

크래프팅 시스템
- 다양한 아이템과 장비를 제작하기 위한 복잡한 크래프팅 시스템을 도입합니다.
- 아이템 제작 및 조합 메커니즘을 확장합니다.

퀘스트와 스토리
- 게임에 퀘스트와 스토리 요소를 추가하여 게임 세계를 더 풍부하게 만듭니다.
- 플레이어의 진행과 스토리 개발을 통합합니다.

고급 AI 시스템
- 적과 동물의 AI를 더 복잡하게 만들어 게임의 난이도를 조절합니다.
- AI 행동 패턴, 지능, 전략 등을 개발합니다.

사운드 및 음악
- 게임에 사운드 효과와 음악을 추가하여 게임의 분위기를 개선합니다.
- 자연 소리, 적의 소리 효과 등을 통해 게임 환경을 풍부하게 만듭니다.

고급 퍼즐 요소
- 퍼즐의 난이도를 높이기 위해 고급 퍼즐 요소를 도입합니다.
- 복합적인 논리나 물리학 요소를 활용한 퍼즐을 추가합니다.

스토리와 미스테리
- 게임에 깊은 스토리나 미스테리 요소를 포함하여 게임의 내러티브를 강화합니다.
- 퍼즐 해결과 스토리 진행을 연계합니다.

퍼즐 생성기
- 플레이어들이 자신만의 퍼즐을 생성하고 공유할 수 있는 퍼즐 생성기를 제공합니다.
- 사용자 맞춤형 콘텐츠 생성을 지원합니다.


## 📂 프로젝트 폴더 구조  

📦 2. Scripts/
├── 📂 BuffData/ # 버프 스크립트
│ └─ 📂 Data/ # 버프 Scriptable Object
│ └─ Buff_DoubleJump.asset
│ └─ Buff_Heal.asset
│ └─ Buff_JumpUp.asset
│ └─ Buff_SpeedUp.asset
│ └─ Buff_Venom.asset
│ └─ BuffData.cs
├── 📂 Build/ # 건축 스크립트
│ └─ Build.cs
│ └─ BuildSnapPoints.cs
│ └─ PreviewObject.cs
│ └─ Turret.cs
├── 📂 Enemy/ # Enemy 스크립트
│ └─ Enemy_Bear.cs
│ └─ Enemy_Zombie.cs
│ └─ EnemyManager.cs
├── 📂 Environment/ # 환경 스크립트
│ └─ DayNightCycle.cs
│ └─ SceneFlowManager.cs
│ └─ TitleManager.cs
├── 📂 Global/ # 공통 스크립트
│ └─ Constant.cs
├── 📂 Item/ # 아이템 관련 스크립트
│ └─ Equip.cs
│ └─ EquipTool.cs
│ └─ Interactable.cs
│ └─ ItemObject.cs
│ └─ ResorceSpawnManager.cs
│ └─ Resource.cs
│ └─ WeaponGun.cs
│ └─ WeaponManager.cs
├── 📂 ItemData/ # 아이템 Scriptable Object
│ └─ 📂 Data/
│ └─ 📂 Consumable/ # 소비 아이템
│ └─ Item_Potion.asset
│ └─ Item_Rock.asset
│ └─ Item_Soup.asset
│ └─ 📂 Equipments/ # 장착 아이템
│ └─ Item_Axe.asset
│ └─ Item_SubMachinGunLong.asset
│ └─ Item_SubMachinGunShort.asset
│ └─ Item_Sword.asset
│ └─ Item_Wood.asset
│ └─ 📂 Harvestables/ # 드랍 아이템
│ └─ Item_Grass_HarvestableObject.asset
│ └─ Item_Rock_HarvestableObject.asset
│ └─ Item_Tree_HarvestableObject.asset
│ └─ 📂 Resources/ # 자원 아이템
│ └─ Item_Grass.asset
│ └─ Item_RockRsourse.asset
│ └─ Item_Woods.asset
│ └─ ItemData.cs
├── 📂 NPC/ # NPC 스크립트
│ └─ DialogueLine.cs
│ └─ DialogueManager.cs
│ └─ MazeGenerator.cs
│ └─ MazeTeleport.cs
│ └─ NPC.cs
│ └─ NPCDialogueDataManager.cs
│ └─ NPCLoadScene.cs
│ └─ SaveNPC.cs
├── 📂 Platform/ # 발판 스크립트
│ └─ JumpPlatform.cs
├── 📂 Player/ # 플레이어 스크립트
│ └─ Condition.cs
│ └─ Equipment.cs
│ └─ Interaction.cs
│ └─ Player.cs
│ └─ PlayerCondition.cs
│ └─ PlayerController.cs
│ └─ PlayerManager.cs
├── 📂 Quest/ # 퀘스트 스크립트
│ └─ QuestData.cs
│ └─ QuestManager.cs
├── 📂 Sound/ # 사운드 스크립트
│ └─ SoundControler.cs
│ └─ SoundManager.cs
├── 📂 UI/ # UI 스크립트
│ └─ CraftUIManager.cs
│ └─ ItemSlot.cs
│ └─ UIBuff.cs
│ └─ UIBuffManager.cs
│ └─ UIBuild.cs
│ └─ UICondition.cs
│ └─ UICraft.cs
│ └─ UIInventory.cs
│ └─ UIMaze.cs
│ └─ UIProduction.cs
│ └─ UIQuest.cs


## 👤 개발자
<p>팀장 : 정세윤 - 적과의 전투, 자원 채취, 날씨와 환경 요소, 프로젝트 합치기</p>
<p>팀원 : 조영종 - NPC와의 대화 기능, 퀘스트와 스토리, 고급 퍼즐 요소, 스토리와 미스테리, 퍼즐 생성기</p>
<p>팀원 : 박성한 - 건축 및 생존 기지 구축, 고급 건축 시스템, 다양한 적 종류, 고급 AI 시스템, 크래프팅 시스템</p>
<p>팀원 : 장보석 - 식사와 수분 관리, 생존 관리 시스템, UI 애니메이션 추가, 적과의 전투</p>
<p>팀원 : 김상균 - 자원 수집 및 가공, 사운드 및 음악, 크래프팅 시스템</p>


## 🧠트러블 슈팅  
<p>이슈</p>
- 다양한 적 종류를 만들고 EnemyManager를 구현하던 중 적들이 관리가 잘 안되어 Null 오류가 발생이 되었다. 
- 적들 사망시 한번만 이루어저야 할 동작(아이템 드랍 등)이 중복으로 발생되는 문제가 있었다.

<p>원인</p>
- 안일한 코드의 짜임새로 발생된 것인데 재생성 되고 파괴되는 적들을 List에 삽입하여 제대로 관리해주지 못한것이 원인
- 사망한 적들의 콜라이더가 존재하여 동작이 계속 되는 것이 원인 이었다.

<p>방법</p>
- 델리게이트를 활용하여 적들이 생성되고 사망했을 시에 List로 관리할 코드를 추가하여 확실하게 관리 할 수 있도록 했고 같은 메서드에 콜라이더와 사망시 정지해야할 동작들의 컴포넌트를 비활성화 해주는 방식을 취했다.

<p>결과</p>
- 모두 의도했던 대로 사망시 한번만 동작 → 생성되었던 List에서 삭제함으로서 원활하게 작동하였고 만족스러운 결과를 얻었다.


## 😊 프로젝트 회고
<p>정세윤 - 이번에 맡은 일이 숙련주차 강의를 보면 구현할 수 있는 거라 금방 끝낼 수 있을 것 같았습니다. 그런데 생각보다 적용하는 시간이 오래 걸렸던 것 같습니다. 그래도 기능을 구현하는 과정에서 내가 배운 기능들을 완벽히 이해했다는 점에서 한층 더 성장한 것 같습니다.</p>
<p>조영종 - 대다수의 개발은 협업으로 이루어진다는 것을 인지하고 협업을 잘 할 수 있는 방안을 계속 생각했던 것 같습니다. 이번 작품 활동을 하면서 정말 재밌었는데, 특히 조 분위기가 너무나 좋아서 행복했습니다.</p>
<p>박성한 - 이번 조의 협업 기간 동안 정말 즐겁게 임했습니다. 건축 부분에서 제가 맡았던 기능이 머릿속에 있었는데 조금은 허술하지만 생각했던 대로 구현이 되었던 것이 매우 만족스럽습니다. 프로젝트 진행을 매우 재밌게해서 만족스러운 결과가 나온 것 같습니다. 그와는 별개로 더 많은 공부를 해야겠다는 생각이 들었습니다.</p>
<p>장보석 - 여러 객체를 생성하고 소멸시키는 과정에서 오브젝트 풀링이라는 기술을 사용해보고 싶었지만 코드에 대한 이해가 아직 부족하여 구현하지 못한 것이 많이 아쉬웠습니다. 언어에 대해서, 엔진에 대해 더 공부를 한 뒤에 다음 프로젝트에서 꼭 구현해보고 싶습니다.</p>
<p>김상균 - 기능을 구현하기에 앞서 간단하게 만들 수 있겠다라고 생각한 부분에서 생각보다 고민을 많이 하여야 했고 시간이 많이 걸렸습니다. 그 과정에서 다른 조원분들의 도움이나 튜터님들의 도움을 많이 받게 되어서 고마운 생각이 들었습니다.</p>
