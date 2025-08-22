# Puzzle_Survial(PUZ) Project

## 🎮 게임 소개
**『PUZ』**는 서바이벌과 퍼즐을 결합한 게임입니다.

Unity 엔진 기반으로 개발되었으며, FSM, ScriptableObject, DoTween, 상태 전환 시스템 등 다양한 기술을 활용하여 구현하였습니다.

- 장르: 3D 원거리 액션 게임

- 플랫폼: PC (Windows)

- 개발 툴: Unity 2022.3.17f1, Visual Studio, GitHub

- 개발 기간: 2025.08.13 ~ 2025.08.22


## 🖼️ 게임 화면
<p>게임 플레이 화면입니다.</p>
<img src="https://github.com/user-attachments/assets/bec74a7a-c007-4626-8802-ff9565a9dacb" width="600"/>
<img src="https://github.com/user-attachments/assets/6159cd69-5926-4441-bd26-e6ef581f6a33" width="600"/>


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

📦 2. Scripts/</br>
├── 📂 BuffData/ # 버프 스크립트</br>
│ └─ 📂 Data/ # 버프 Scriptable Object</br>
│ └─ Buff_DoubleJump.asset</br>
│ └─ Buff_Heal.asset</br>
│ └─ Buff_JumpUp.asset</br>
│ └─ Buff_SpeedUp.asset</br>
│ └─ Buff_Venom.asset</br>
│ └─ BuffData.cs</br>
├── 📂 Build/ # 건축 스크립트</br>
│ └─ Build.cs</br>
│ └─ BuildSnapPoints.cs</br>
│ └─ PreviewObject.cs</br>
│ └─ Turret.cs</br>
├── 📂 Enemy/ # Enemy 스크립트</br>
│ └─ Enemy_Bear.cs</br>
│ └─ Enemy_Zombie.cs</br>
│ └─ EnemyManager.cs</br>
├── 📂 Environment/ # 환경 스크립트</br>
│ └─ DayNightCycle.cs</br>
│ └─ SceneFlowManager.cs</br>
│ └─ TitleManager.cs</br>
├── 📂 Global/ # 공통 스크립트</br>
│ └─ Constant.cs</br>
├── 📂 Item/ # 아이템 관련 스크립트</br>
│ └─ Equip.cs</br>
│ └─ EquipTool.cs</br>
│ └─ Interactable.cs</br>
│ └─ ItemObject.cs</br>
│ └─ ResorceSpawnManager.cs</br>
│ └─ Resource.cs</br>
│ └─ WeaponGun.cs</br>
│ └─ WeaponManager.cs</br>
├── 📂 ItemData/ # 아이템 Scriptable Object</br>
│ └─ 📂 Data/</br>
│ └─ 📂 Consumable/ # 소비 아이템</br>
│ └─ Item_Potion.asset</br>
│ └─ Item_Rock.asset</br>
│ └─ Item_Soup.asset</br>
│ └─ 📂 Equipments/ # 장착 아이템</br>
│ └─ Item_Axe.asset</br>
│ └─ Item_SubMachinGunLong.asset</br>
│ └─ Item_SubMachinGunShort.asset</br>
│ └─ Item_Sword.asset</br>
│ └─ Item_Wood.asset</br>
│ └─ 📂 Harvestables/ # 드랍 아이템</br>
│ └─ Item_Grass_HarvestableObject.asset</br>
│ └─ Item_Rock_HarvestableObject.asset</br>
│ └─ Item_Tree_HarvestableObject.asset</br>
│ └─ 📂 Resources/ # 자원 아이템</br>
│ └─ Item_Grass.asset</br>
│ └─ Item_RockRsourse.asset</br>
│ └─ Item_Woods.asset</br>
│ └─ ItemData.cs</br>
├── 📂 NPC/ # NPC 스크립트</br>
│ └─ DialogueLine.cs</br>
│ └─ DialogueManager.cs</br>
│ └─ MazeGenerator.cs</br>
│ └─ MazeTeleport.cs</br>
│ └─ NPC.cs</br>
│ └─ NPCDialogueDataManager.cs</br>
│ └─ NPCLoadScene.cs</br>
│ └─ SaveNPC.cs</br>
├── 📂 Platform/ # 발판 스크립트</br>
│ └─ JumpPlatform.cs</br>
├── 📂 Player/ # 플레이어 스크립트</br>
│ └─ Condition.cs</br>
│ └─ Equipment.cs</br>
│ └─ Interaction.cs</br>
│ └─ Player.cs</br>
│ └─ PlayerCondition.cs</br>
│ └─ PlayerController.cs</br>
│ └─ PlayerManager.cs</br>
├── 📂 Quest/ # 퀘스트 스크립트</br>
│ └─ QuestData.cs</br>
│ └─ QuestManager.cs</br>
├── 📂 Sound/ # 사운드 스크립트</br>
│ └─ SoundControler.cs</br>
│ └─ SoundManager.cs</br>
├── 📂 UI/ # UI 스크립트</br>
│ └─ CraftUIManager.cs</br>
│ └─ ItemSlot.cs</br>
│ └─ UIBuff.cs</br>
│ └─ UIBuffManager.cs</br>
│ └─ UIBuild.cs</br>
│ └─ UICondition.cs</br>
│ └─ UICraft.cs</br>
│ └─ UIInventory.cs</br>
│ └─ UIMaze.cs</br>
│ └─ UIProduction.cs</br>
│ └─ UIQuest.cs</br>


## 👤 개발자
<p>팀장 : 정세윤 - 적과의 전투, 자원 채취, 날씨와 환경 요소, 프로젝트 합치기</p>
<p>팀원 : 조영종 - NPC와의 대화 기능, 퀘스트와 스토리, 고급 퍼즐 요소, 스토리와 미스테리, 퍼즐 생성기</p>
<p>팀원 : 박성한 - 건축 및 생존 기지 구축, 고급 건축 시스템, 다양한 적 종류, 고급 AI 시스템, 크래프팅 시스템</p>
<p>팀원 : 장보석 - 식사와 수분 관리, 생존 관리 시스템, UI 애니메이션 추가, 적과의 전투</p>
<p>팀원 : 김상균 - 자원 수집 및 가공, 사운드 및 음악, 크래프팅 시스템</p>


## 🧠트러블 슈팅  
<img width="958" height="667" alt="image" src="https://github.com/user-attachments/assets/5f56623b-38ac-4903-8faf-075168023d17" />

<img width="962" height="507" alt="image" src="https://github.com/user-attachments/assets/db566ac8-492a-444a-b01f-53099f51c95a" />

<img width="960" height="623" alt="image" src="https://github.com/user-attachments/assets/8a974554-8f12-49f6-b87a-1df4b8522164" />

<img width="962" height="597" alt="image" src="https://github.com/user-attachments/assets/a61b25bf-5d25-4c43-ba1d-837d735361db" />

<img width="960" height="531" alt="image" src="https://github.com/user-attachments/assets/7750cc93-9827-4945-b94b-761fe6b26130" />


## 😊 프로젝트 회고
<p>정세윤 - 이번에 맡은 일이 숙련주차 강의를 보면 구현할 수 있는 거라 금방 끝낼 수 있을 것 같았습니다. 그런데 생각보다 적용하는 시간이 오래 걸렸던 것 같습니다. 그래도 기능을 구현하는 과정에서 내가 배운 기능들을 완벽히 이해했다는 점에서 한층 더 성장한 것 같습니다.</p>
<p>조영종 - 대다수의 개발은 협업으로 이루어진다는 것을 인지하고 협업을 잘 할 수 있는 방안을 계속 생각했던 것 같습니다. 이번 작품 활동을 하면서 정말 재밌었는데, 특히 조 분위기가 너무나 좋아서 행복했습니다.</p>
<p>박성한 - 이번 조의 협업 기간 동안 정말 즐겁게 임했습니다. 건축 부분에서 제가 맡았던 기능이 머릿속에 있었는데 조금은 허술하지만 생각했던 대로 구현이 되었던 것이 매우 만족스럽습니다. 프로젝트 진행을 매우 재밌게해서 만족스러운 결과가 나온 것 같습니다. 그와는 별개로 더 많은 공부를 해야겠다는 생각이 들었습니다.</p>
<p>장보석 - 여러 객체를 생성하고 소멸시키는 과정에서 오브젝트 풀링이라는 기술을 사용해보고 싶었지만 코드에 대한 이해가 아직 부족하여 구현하지 못한 것이 많이 아쉬웠습니다. 언어에 대해서, 엔진에 대해 더 공부를 한 뒤에 다음 프로젝트에서 꼭 구현해보고 싶습니다.</p>
<p>김상균 - 기능을 구현하기에 앞서 간단하게 만들 수 있겠다라고 생각한 부분에서 생각보다 고민을 많이 하여야 했고 시간이 많이 걸렸습니다. 그 과정에서 다른 조원분들의 도움이나 튜터님들의 도움을 많이 받게 되어서 고마운 생각이 들었습니다.</p>
