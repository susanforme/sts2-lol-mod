# 已实现卡牌一览

当前根据 `src/cards/*.cs` 中的具体卡牌类整理，不包含 `AbstractJhinCard`、`AbstractShootCard`、`JhinKeywords` 等非卡牌定义文件。

| ID | 中文名 | 英文名 | 类型 | 费用 | 稀有度 | 目标 | 文件 | 当前效果摘要 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `JHIN-COMMON_SHOT` | 普通射击 | Common Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/CommonShot.cs` | 消耗 1 层子弹造成基础伤害；华彩时追加一段固定伤害。 |
| `JHIN-PERFECT_SHOT` | 精准一枪 | Perfect Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/PerfectShot.cs` | 射击造成更高单体伤害；华彩时回能，升级后还能抽牌。 |
| `JHIN-WHISPER_BURST` | 低语点射 | Whisper Burst | 攻击 / 射击 | `0` | 普通 | 单体敌人 | `src/cards/WhisperBurst.cs` | 0 费射击；华彩时追加高额补伤。 |
| `JHIN-GRACEFUL_FOOTWORK` | 优雅走位 | Graceful Footwork | 技能 | `1` | 普通 | 自身 | `src/cards/GracefulFootwork.cs` | 获得格挡；若本回合已射击则再获得额外格挡。 |
| `JHIN-AIM_SHOT` | 瞄准射击 | Aim Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/AimShot.cs` | 先进行射击伤害，再给予目标标记；升级后改为 2 层。 |
| `JHIN-PIERCING_ROUND` | 穿透弹 | Piercing Round | 攻击 / 射击 | `1` | 普通 | 全体敌人 | `src/cards/PiercingRound.cs` | 消耗 1 发子弹对所有敌人射击；华彩时全体追加固定伤害。 |
| `JHIN-DANCING_GRENADE` | 曼舞手雷 | Dancing Grenade | 攻击 | `1` | 普通 | 单体敌人 | `src/cards/DancingGrenade.cs` | 非射击伤害；若击杀目标，会随机弹跳到另一名敌人并提高伤害。 |
| `JHIN-FINISH_OFF` | 收尾 | Finish Off | 攻击 | `1` | 普通 | 单体敌人 | `src/cards/FinishOff.cs` | 非射击终结牌；对半血以下目标造成额外伤害。 |
| `JHIN-SET_THE_STAGE` | 布置舞台 | Set the Stage | 技能 | `1` | 普通 | 自身 | `src/cards/SetTheStage.cs` | 获得格挡，并给随机敌人施加标记；升级后改为 2 层。 |
| `JHIN-CALCULATED_PLAN` | 精密计算 | Calculated Plan | 技能 | `1` | 普通 | 自身 | `src/cards/CalculatedPlan.cs` | 抽 2 张牌；在弹匣接近打空时返还 1 点能量。 |
| `JHIN-BACKSTEP` | 后撤步 | Backstep | 技能 | `0` | 普通 | 自身 | `src/cards/Backstep.cs` | 获得少量格挡；若本回合已射击则抽 1 张牌。 |
| `JHIN-SET_TRAP` | 设下陷阱 | Set a Trap | 技能 | `1` | 普通 | 单体敌人 | `src/cards/SetTrap.cs` | 获得格挡，并给予目标 1 层莲花陷阱。 |
| `JHIN-CALM_RELOAD` | 冷静装填 | Calm Reload | 技能 | `1` | 普通 | 自身 | `src/cards/CalmReload.cs` | 装填到满弹匣并获得格挡。 |
| `JHIN-LONG_RANGE_SNIPE` | 远距狙击 | Long-Range Snipe | 攻击 / 射击 | `2` | 罕见 | 单体敌人 | `src/cards/LongRangeSnipe.cs` | 高额单体射击；按标记层数追加伤害，华彩时再追加一段固定补伤。 |
| `JHIN-FOURTH_ACT` | 第四乐章 | Fourth Act | 攻击 / 射击 | `2` | 罕见 | 单体敌人 | `src/cards/FourthAct.cs` | 只能在最后一发子弹时打出；高额第四枪并施加易伤。 |
| `JHIN-GRAND_STAGE` | 盛大布景 | Grand Stage | 技能 | `2` | 罕见 | 自身 | `src/cards/GrandStage.cs` | 获得较高格挡，并给予所有敌人 1 层莲花陷阱。 |
| `JHIN-DEADLY_FLOURISH` | 致命华彩 | Deadly Flourish | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/DeadlyFlourish.cs` | 造成射击伤害；若目标命中前带有标记，则施加易伤，华彩时层数更高。 |
| `JHIN-RELOAD` | 装填 | Reload | 技能 | `0` | 普通 | 自身 | `src/cards/Reload.cs` | 将当前子弹装填至 4，并抽牌；本回合禁用华彩且会消耗。 |
| `JHIN-FLOURISH_TEMPO` | 华彩节奏 | Flourish Tempo | 能力 | `2` | 罕见 | 自身 | `src/cards/FlourishTempo.cs` | 持续强化华彩收益；每次华彩回能，升级后额外抽牌。 |
| `JHIN-CURTAIN_CALL` | 谢幕 | Curtain Call | 攻击 / 谢幕 | `2` | 稀有 | 随机敌人 | `src/cards/CurtainCall.cs` | 只能在子弹为 0 时使用；随机攻击 4 次，每段根据目标当前生命损失独立提高伤害，使用后消耗。 |

## 备注

| 项目 | 说明 |
| --- | --- |
| 射击牌统一入口 | 当前 MVP 射击牌统一继承 `AbstractShootCard`，包含 `普通射击`、`精准一枪`、`低语点射`、`瞄准射击`、`穿透弹`、`远距狙击`、`第四乐章`、`致命华彩`。 |
| 标记联动 | 当前已接入阶段 4 标记系统的牌为 `瞄准射击`、`布置舞台`、`远距狙击`、`致命华彩`。 |
| 莲花陷阱联动 | 当前已接入阶段 5 莲花陷阱系统的牌为 `设下陷阱`、`盛大布景`。 |
| 华彩联动 | `普通射击`、`精准一枪`、`低语点射`、`穿透弹`、`远距狙击`、`第四乐章`、`致命华彩` 和能力牌 `华彩节奏` 都会响应统一华彩判定。 |
| 谢幕联动 | 当前 `谢幕` 使用独立的谢幕条件与四段随机攻击 Action，不消耗子弹，也不触发普通射击华彩判定。 |
| 占位图规则 | 当前卡牌默认使用 `JhinMod/Images/card_placeholder.png`。 |
