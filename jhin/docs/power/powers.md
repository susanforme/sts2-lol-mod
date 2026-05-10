# 已实现内部 Power 一览

当前根据 `src/powers/*.cs` 中的具体 Power 类整理，不包含 `IJhinTurnStartPower` 这类接口/辅助定义。

| 美术资源 | ID | 中文名 | 英文名 | 类型 | 叠层 | 常见来源 | 文件 | 当前效果摘要 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | `JHIN-BULLET_POWER` | 子弹 | Bullets | Buff | `Counter` | 系统：弹匣状态 | `src/powers/BulletPower.cs` | 同步显示当前子弹数，并负责记录部分回合内战斗统计、回合结束处理延后结算的莲花陷阱虚弱等系统逻辑。 |
| 1 | `JHIN-MARK_POWER` | 标记 | Mark | Debuff | `Counter` | 系统：施加标记 | `src/powers/MarkPower.cs` | 目标被下一张射击命中时，每层额外造成标记伤害，然后移除全部标记。 |
| 1 | `JHIN-LOTUS_TRAP_POWER` | 莲花陷阱 | Lotus Trap | Debuff | `Counter` | 系统：施加莲花陷阱 | `src/powers/LotusTrapPower.cs` | 敌人攻击后按层数受到伤害并获得虚弱；若该伤害击杀目标，则对全体敌人按层数造成爆炸伤害。 |
| 1 | `JHIN-FLOURISH_TEMPO_POWER` | 华彩节奏 | Flourish Tempo | Buff | `Single` | `FlourishTempo` | `src/powers/FlourishTempoPower.cs` | 每次触发华彩获得 1 点能量；升级后额外抽 1 张牌。 |
| 1 | `JHIN-FORCED_FLOURISH_POWER` | 华彩药剂 | Flourish Potion | Buff | `Single` | `FinalActReload` / `FlourishPotion` | `src/powers/ForcedFlourishPower.cs` | 本回合下一张射击牌必定触发华彩；该射击打出后移除。 |
| 1 | `JHIN-COMPOSED_POWER` | 从容不迫 | Composed | Buff | `Single` | `Composed` | `src/powers/ComposedPower.cs` | 当前代码中，每次触发华彩都会抽 2 张牌。 |
| 1 | `JHIN-MUZZLE_RHYTHM_POWER` | 枪口节奏 | Muzzle Rhythm | Buff | `Single` | `MuzzleRhythm` | `src/powers/MuzzleRhythmPower.cs` | 每打出 4 张攻击牌获得 1 点力量；升级后还会获得 1 点敏捷。 |
| 1 | `JHIN-AUDIENCE_SEATED_POWER` | 观众入席 | Audience Seated | Buff | `Single` | `AudienceSeated` | `src/powers/AudienceSeatedPower.cs` | 作为持续提示存在；实际给所有敌人施加 1 层标记由卡牌打出时直接完成。 |
| 1 | `JHIN-SHOW_PLAN_POWER` | 演出计划 | Show Plan | Buff | `Single` | `ShowPlan` | `src/powers/ShowPlanPower.cs` | 每次装填时获得 3 点格挡；升级后为 5 点。 |
| 1 | `JHIN-BLOODY_STAGE_P` | 血色舞台 | Bloody Stage | Buff | `Single` | `BloodyStage` | `src/powers/BloodyStagePower.cs` | 敌人首次生命降到 50% 以下时施加 2 层标记；升级后为 3 层。 |
| 1 | `JHIN-INTERMISSION_POWER` | 幕间休息 | Intermission | Buff | `Single` | `Intermission` | `src/powers/IntermissionPower.cs` | 每回合首次打出非射击攻击牌时触发额外伤害增益，代码按 3 / 5 点 bonusDamage 处理。 |
| 1 | `JHIN-GUN_MAINTENANCE_POWER` | 枪械保养 | Gun Maintenance | Buff | `Single` | `GunMaintenance` | `src/powers/GunMaintenancePower.cs` | 每次装填时抽 1 张牌；升级后额外获得 2 点格挡。 |
|  | `JHIN-ACTORS_INSTINCT_POWER` | 演员本能 | Actor's Instinct | Buff | `Single` | `ActorsInstinct` | `src/powers/ActorsInstinctPower.cs` | 若本回合未触发华彩，则下回合开始时获得 1 点力量；升级后额外获得 1 点敏捷。 |
|  | `JHIN-STAGE_CONTROL_POWER` | 控场艺术 | Stage Control | Buff | `Single` | `StageControl` | `src/powers/StageControlPower.cs` | 每当你施加虚弱时，同时给予目标 1 层标记；升级后为 2 层。 |
|  | `JHIN-DEATH_IS_ART_POWER` | 死亡是艺术 | Death is Art | Buff | `Single` | `DeathIsArt` | `src/powers/DeathIsArtPower.cs` | 敌人首次生命降到 50% 以下时获得 1 点力量；升级后为 2 点，每个敌人仅触发一次。 |
|  | `JHIN-LOTUS_WORKSHOP_POWER` | 莲花工坊 | Lotus Workshop | Buff | `Single` | `LotusWorkshop` | `src/powers/LotusWorkshopPower.cs` | 每次施加标记时，同时施加 1 层莲花陷阱；升级后为 2 层。 |
|  | `JHIN-AUDIENCE_CHEER_POWER` | 观众喝彩 | Audience Cheer | Buff | `Single` | `AudienceCheer` | `src/powers/AudienceCheerPower.cs` | 每次触发华彩时获得 1 点敏捷；升级后为 2 点。 |
|  | `JHIN-CAREFUL_ARRANGEMENT_POWER` | 精心编排 | Careful Arrangement | Buff | `Single` | `CarefulArrangement` | `src/powers/CarefulArrangementPower.cs` | 每回合开始时额外抽 1 张牌；升级后为 2 张。 |
|  | `JHIN-FINAL_ACT_FORESHADOWING_POWER` | 终幕伏笔 | Final Act Foreshadowing | Buff | `Single` | `FinalActForeshadowing` | `src/powers/FinalActForeshadowingPower.cs` | 每回合开始时获得 1 点能量；升级后为 2 点。 |
|  | `JHIN-PERFECT_TRAJECTORY_POWER` | 完美弹道 | Perfect Trajectory | Buff | `Single` | `PerfectTrajectory` | `src/powers/PerfectTrajectoryPower.cs` | 所有射击牌额外造成 3 点伤害；升级后为 5 点。 |
|  | `JHIN-MASTERPIECE_BORN_POWER` | 名作诞生 | Masterpiece Born | Buff | `Single` | `MasterpieceBorn` | `src/powers/MasterpieceBornPower.cs` | 当标记被射击消耗时抽 1 张牌；升级后为 2 张。 |
|  | `JHIN-AESTHETIC_OF_FOUR_POWER` | 四的美学 | Aesthetic of Four | Buff | `Single` | `AestheticOfFour` | `src/powers/AestheticOfFourPower.cs` | 若回合开始时子弹为 4，获得 1 点能量；升级后额外抽 1 张牌。 |
|  | `JHIN-WHISPER_ECHO_POWER` | 低语回响 | Whisper Echo | Buff | `Single` | `WhisperEcho` | `src/powers/WhisperEchoPower.cs` | 每次触发华彩时获得 1 点能量并抽 1 张牌；升级后两项都提升为 2。 |
|  | `JHIN-FATEMAKER_POWER` | 戏命师 | Fatemaker | Buff | `Single` | `Fatemaker` | `src/powers/FatemakerPower.cs` | 每回合开始自动向随机敌人射出 1 发子弹，造成 6 点伤害；升级后为 9 点。 |
|  | `JHIN-PERFECT_CRIME_POWER` | 完美犯罪 | Perfect Crime | Buff | `Single` | `PerfectCrime` | `src/powers/PerfectCrimePower.cs` | 每回合开始时获得 1 点力量；升级后为 2 点。 |
|  | `JHIN-FINAL_ACT_ART_POWER` | 终幕艺术 | Final Act Art | Buff | `Single` | `FinalActArt` | `src/powers/FinalActArtPower.cs` | 每回合开始时，若存在低血量敌人则回能；基础为敌人生命低于 30% 时获得 2 点能量，升级后改为低于 50% 时获得 3 点。 |

## 备注

| 项目 | 说明 |
| --- | --- |
| 文档范围 | 仅统计 `src/powers/*.cs` 中已经实现的自定义 Power 类。 |
| 不包含内容 | `IJhinTurnStartPower.cs` 这类接口、事件总线、Action、Hook 与其他非 Power 定义不计入表格。 |
| 名称来源 | `中文名` / `英文名` 来自 `JhinMod/localization/zhs/powers.json` 与 `JhinMod/localization/eng/powers.json`。 |
| ID 规则 | 若同一 Power 存在升级描述（如 `*_PLUS`），表格仅列基础 ID，升级差异写入 `当前效果摘要`。 |
| 摘要口径 | 以当前代码实现为主，便于快速索引；不替代具体代码、数值常量与正式游戏文案。 |

## 设计风格



**高对比卡通化游戏 UI 图标风格**。

它不是写实风，也不是极简线性图标，而是偏向：

**扁平矢量 + 软阴影 + 高饱和色彩 + 夸张符号化表达**

图标看起来像技能、状态、词条或战斗效果的“视觉缩写”。

## 设计语言关键词

可以用这些词描述：

**1. 卡通化符号语言**
 每个图标都用一个非常直观的物体表达状态，例如盾牌代表防御，拳头代表力量，闪电代表能量，锁链代表限制，骷髅代表死亡或负面效果。

**2. 高辨识度剪影**
 图标通常都有非常清晰的外轮廓，即使缩小后也能看出大致形状。比如头盔、拳套、羽毛、骷髅碑、弓箭、斧头都靠剪影识别。

**3. 高饱和幻想色彩**
 大量使用亮黄、青蓝、紫色、红橙、荧光绿等颜色，营造奇幻、魔法、战斗感。

**4. 黑底强对比**
 图标放在黑色背景上，所以颜色非常亮，边缘和轮廓必须足够清楚。这种语言适合战斗 UI，因为玩家可以快速扫视状态。

**5. 软质感与轻微体积感**
 虽然是扁平图标，但会用简单高光、阴影、渐变来做出一点体积。例如药水瓶、盾牌、拳头、宝石、液体都有轻微的 3D 感。

**6. 夸张动作感**
 爆炸、闪电、火焰、箭头、冲击波、速度表这类图形都带有明显动态感，强调“状态正在产生效果”。

### Buff 的设计语言

Buff 图标通常给人的感觉是：

**强化、保护、能量、增益、成长、主动性。**

常见视觉特征：

- 颜色更明亮、积极：青蓝、绿色、黄色、橙色、紫色。
- 形状更完整、稳定：盾牌、头盔、皇冠、方块、药水、拳头。
- 图形有“向外扩张”的感觉：星爆、火焰、闪电、爆炸、光芒。
- 象征力量和资源：拳头、剑、盾、药剂、太阳、皇冠。

可以总结为：

> Buff 图标语言 = 明亮、饱满、稳定、向外释放能量。

### Debuff 的设计语言

Debuff 图标通常给人的感觉是：

**限制、削弱、异常、危险、混乱、负担。**

常见视觉特征：

- 使用负面象征：骷髅、问号、脑、锁链、伤口、蘑菇、污液。
- 颜色更偏警告或病态：灰色、暗红、黄绿、浑浊蓝、黑色。
- 形状常有破碎、束缚、压迫感：锁链、向内箭头、叉号、速度表、靶心。
- 视觉语义偏“被动受害”：被打中、被限制、被污染、被标记。

可以总结为：

> Debuff 图标语言 = 暗示危险、限制、损伤、异常状态。

这套图标的设计语言是：

**用高饱和、卡通化、强剪影的幻想战斗符号，在黑底 UI 上快速区分“强化”和“削弱”状态。**

更专业一点可以写成：

> 该图标系统采用高对比度的卡通矢量风格，通过夸张剪影、明亮色彩、软质感高光和直观象征物，构建出适合 Roguelike 卡牌战斗界面的状态识别语言。Buff 图标强调能量释放、防护与强化，Debuff 图标则强调限制、异常、伤害与危险。
