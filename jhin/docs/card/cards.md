# 已实现卡牌一览

当前根据 `src/cards/*.cs` 中的具体卡牌类整理，不包含 `AbstractJhinCard`、`AbstractShootCard`、`JhinKeywords` 等非卡牌定义文件。

| 美术 | ID | 中文名 | 英文名 | 类型 | 费用 | 稀有度 | 目标 | 文件 | 当前效果摘要 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | `JHIN-STRIKE` | 打击 | Strike | 攻击 | `1` | 基础 | 单体敌人 | `src/cards/Strike.cs` | 基础攻击牌，造成单体伤害。 |
| 1 | `JHIN-DEFEND` | 防御 | Defend | 技能 | `1` | 基础 | 自身 | `src/cards/Defend.cs` | 基础防御牌，获得格挡。 |
| 1 | `JHIN-COMMON_SHOT` | 普通射击 | Common Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/CommonShot.cs` | 消耗 1 层子弹，造成基础射击伤害。 |
|  | `JHIN-PERFECT_SHOT` | 精准一枪 | Perfect Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/PerfectShot.cs` | 射击造成更高单体伤害；华彩时回能，升级后还会抽牌。 |
|  | `JHIN-WHISPER_BURST` | 低语点射 | Whisper Burst | 攻击 / 射击 | `0` | 普通 | 单体敌人 | `src/cards/WhisperBurst.cs` | 0 费射击牌，消耗 1 层子弹造成伤害。 |
|  | `JHIN-GRACEFUL_FOOTWORK` | 优雅走位 | Graceful Footwork | 技能 | `1` | 普通 | 自身 | `src/cards/GracefulFootwork.cs` | 获得格挡；若本回合已使用过射击牌，再获得额外格挡。 |
|  | `JHIN-AIM_SHOT` | 瞄准射击 | Aim Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/AimShot.cs` | 射击造成伤害，并给予目标标记。 |
|  | `JHIN-PIERCING_ROUND` | 穿透弹 | Piercing Round | 攻击 / 射击 | `1` | 普通 | 全体敌人 | `src/cards/PiercingRound.cs` | 消耗 1 层子弹，对所有敌人造成射击伤害。 |
|  | `JHIN-DANCING_GRENADE` | 曼舞手雷 | Dancing Grenade | 攻击 | `2` | 稀有 | 随机敌人 | `src/cards/DancingGrenade.cs` | 随机命中 1 名敌人后最多弹跳 4 次；每次弹跳都会提高伤害，击杀时还会让下一跳额外增伤。 |
|  | `JHIN-FINISH_OFF` | 收尾 | Finish Off | 攻击 | `1` | 普通 | 单体敌人 | `src/cards/FinishOff.cs` | 对半血以下目标造成更高伤害。 |
|  | `JHIN-ART_SLICE` | 艺术切割 | Art Slice | 攻击 | `1` | 普通 | 单体敌人 | `src/cards/ArtSlice.cs` | 造成伤害；若目标带有虚弱，追加额外伤害。 |
|  | `JHIN-SIDESTEP_FIRE` | 侧身开火 | Sidestep Fire | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/SidestepFire.cs` | 射击造成伤害并获得格挡。 |
|  | `JHIN-QUICK_DRAW` | 快速拔枪 | Quick Draw | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/QuickDraw.cs` | 射击造成伤害并抽 1 张牌。 |
|  | `JHIN-REHEARSAL_SHOT` | 试演一枪 | Rehearsal Shot | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/RehearsalShot.cs` | 射击造成伤害；若不是华彩则抽 1 张牌。 |
|  | `JHIN-GUNFLAME_GRAZE` | 枪焰擦伤 | Gunflame Graze | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/GunflameGraze.cs` | 射击造成伤害并施加虚弱；华彩时施加更多层。 |
|  | `JHIN-INTERMISSION_KILL` | 幕间点杀 | Intermission Kill | 攻击 | `1` | 普通 | 单体敌人 | `src/cards/IntermissionKill.cs` | 造成伤害；若目标带有标记，获得格挡。 |
|  | `JHIN-ENCORE_BULLET` | 余兴弹 | Encore Bullet | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/EncoreBullet.cs` | 射击造成伤害；若本回合已使用过技能牌，额外造成伤害。 |
|  | `JHIN-WHISPERED_THREAT` | 低声威胁 | Whispered Threat | 攻击 | `0` | 普通 | 单体敌人 | `src/cards/WhisperedThreat.cs` | 0 费攻击，造成伤害并给予标记；使用后消耗。 |
|  | `JHIN-SET_THE_STAGE` | 布置舞台 | Set the Stage | 技能 | `1` | 普通 | 自身 | `src/cards/SetTheStage.cs` | 获得格挡，并给随机敌人施加标记。 |
|  | `JHIN-CALCULATED_PLAN` | 精密计算 | Calculated Plan | 技能 | `1` | 普通 | 自身 | `src/cards/CalculatedPlan.cs` | 抽 2 张牌；在子弹为 1，升级后为 1 或 0 时回能。 |
|  | `JHIN-BACKSTEP` | 后撤步 | Backstep | 技能 | `0` | 普通 | 自身 | `src/cards/Backstep.cs` | 获得少量格挡；若本回合已射击则抽 1 张牌。 |
|  | `JHIN-SET_TRAP` | 设下陷阱 | Set a Trap | 技能 | `1` | 普通 | 单体敌人 | `src/cards/SetTrap.cs` | 获得格挡，并给予目标莲花陷阱。 |
|  | `JHIN-CALM_RELOAD` | 冷静装填 | Calm Reload | 技能 | `1` | 普通 | 自身 | `src/cards/CalmReload.cs` | 将子弹装填至 4，并获得格挡。 |
|  | `JHIN-OBSERVE_WEAKNESS` | 观察弱点 | Observe Weakness | 技能 | `1` | 普通 | 单体敌人 | `src/cards/ObserveWeakness.cs` | 给予目标标记；若其生命低于 50%，抽 1 张牌。 |
|  | `JHIN-PERFECT_STANCE` | 完美姿态 | Perfect Stance | 技能 | `1` | 普通 | 自身 | `src/cards/PerfectStance.cs` | 获得格挡；若子弹为 4，则再获得大量格挡。 |
|  | `JHIN-AWAIT_APPLAUSE` | 等待掌声 | Await Applause | 技能 | `1` | 普通 | 自身 | `src/cards/AwaitApplause.cs` | 获得格挡；下回合开始时获得能量。 |
|  | `JHIN-COUNT_BEATS` | 数拍 | Count Beats | 技能 | `0` | 普通 | 自身 | `src/cards/CountBeats.cs` | 抽牌；若子弹为 1 再抽 1 张；使用后消耗。 |
|  | `JHIN-RELOAD` | 装填 | Reload | 技能 | `0` | 普通 | 自身 | `src/cards/Reload.cs` | 仅将当前子弹直接装填至 4。 |
|  | `JHIN-COMPOSED` | 从容不迫 | Composed | 能力 | `1` | 普通 | 自身 | `src/cards/Composed.cs` | 本场战斗中首次触发华彩时，抽 2 张牌。 |
|  | `JHIN-MUZZLE_RHYTHM` | 枪口节奏 | Muzzle Rhythm | 能力 | `1` | 普通 | 自身 | `src/cards/MuzzleRhythm.cs` | 每打出 4 张攻击牌，获得力量；升级后还获得敏捷。 |
|  | `JHIN-AUDIENCE_SEATED` | 观众入席 | Audience Seated | 能力 | `1` | 普通 | 自身 | `src/cards/AudienceSeated.cs` | 所有敌人获得标记。 |
|  | `JHIN-SHOW_PLAN` | 演出计划 | Show Plan | 能力 | `1` | 普通 | 自身 | `src/cards/ShowPlan.cs` | 每当装填时，获得格挡。 |
|  | `JHIN-BLOODY_STAGE` | 血色舞台 | Bloody Stage | 能力 | `1` | 普通 | 自身 | `src/cards/BloodyStage.cs` | 当敌人首次生命降到 50% 以下时，给予标记。 |
|  | `JHIN-INTERMISSION` | 幕间休息 | Intermission | 能力 | `1` | 普通 | 自身 | `src/cards/Intermission.cs` | 每回合首次使用非射击攻击牌时，额外造成伤害。 |
|  | `JHIN-GUN_MAINTENANCE` | 枪械保养 | Gun Maintenance | 能力 | `1` | 普通 | 自身 | `src/cards/GunMaintenance.cs` | 每当装填时抽牌；升级后还获得格挡。 |
|  | `JHIN-ACTORS_INSTINCT` | 演员本能 | Actor's Instinct | 能力 | `1` | 普通 | 自身 | `src/cards/ActorsInstinct.cs` | 若本回合未触发华彩，下回合获得力量；升级后还获得敏捷。 |
|  | `JHIN-STAGE_CONTROL` | 控场艺术 | Stage Control | 能力 | `1` | 普通 | 自身 | `src/cards/StageControl.cs` | 每当你给敌人施加虚弱时，同时给予标记。 |
|  | `JHIN-DEADLY_SHOT` | 弹无虚发 | Deadly Shot | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/DeadlyShot.cs` | 射击造成伤害；若目标没有虚弱，再追加一段伤害。 |
|  | `JHIN-LOTUS_DETONATION` | 引爆莲花 | Lotus Detonation | 攻击 | `1` | 罕见 | 单体敌人 | `src/cards/LotusDetonation.cs` | 按目标莲花陷阱层数造成伤害，并移除所有莲花陷阱。 |
|  | `JHIN-DEATH_BLOOM` | 死亡绽放 | Death Bloom | 攻击 | `1` | 罕见 | 单体敌人 | `src/cards/DeathBloom.cs` | 造成伤害并给予目标莲花陷阱。 |
|  | `JHIN-WHISPER_VOLLEY` | 低语齐射 | Whisper Volley | 攻击 / 射击 | `2` | 罕见 | 全体敌人 | `src/cards/WhisperVolley.cs` | 需要至少 2 发子弹；连续消耗 2 发子弹，对所有敌人各造成 2 次射击伤害。 |
|  | `JHIN-ENCORE_REHEARSAL` | 反复排练 | Encore Rehearsal | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/EncoreRehearsal.cs` | 射击造成伤害；若不是华彩，抽 1 张牌并获得 1 点能量；使用后消耗。 |
|  | `JHIN-CURTAIN_PIERCE` | 幕布穿刺 | Curtain Pierce | 攻击 | `2` | 罕见 | 单体敌人 | `src/cards/CurtainPierce.cs` | 造成伤害，并按目标现有标记层数追加伤害，但不会消耗标记。 |
|  | `JHIN-BLOODLINE_ART` | 血线艺术 | Bloodline Art | 攻击 | `2` | 罕见 | 单体敌人 | `src/cards/BloodlineArt.cs` | 造成伤害；若目标生命低于 50%，再追加一段伤害。 |
|  | `JHIN-GUNNERS_GIFT` | 枪下谢礼 | Gunner's Gift | 攻击 / 射击 | `2` | 罕见 | 单体敌人 | `src/cards/GunnersGift.cs` | 射击造成伤害；若击杀目标则获得大量格挡；使用后消耗。 |
|  | `JHIN-LENS_LOCK` | 镜头锁定 | Lens Lock | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/LensLock.cs` | 射击造成伤害，保留并补充目标标记；华彩时施加更多标记。 |
|  | `JHIN-THIRD_GUNSHOT` | 第三乐章 | Third Act | 攻击 / 射击 | `1` | 普通 | 单体敌人 | `src/cards/ThirdGunshot.cs` | 只能在子弹为 2 时使用；消耗 1 发子弹，造成单体射击伤害。 |
|  | `JHIN-FIRST_GUNSHOT` | 第一乐章 | First Act | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/FirstGunshot.cs` | 只能在子弹为 4 时使用；消耗 1 发子弹，造成伤害并施加 1 层易伤与 1 层标记；未升级时消耗，升级后移除消耗。 |
|  | `JHIN-SECOND_GUNSHOT` | 第二乐章 | Second Act | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/SecondGunshot.cs` | 只能在子弹为 3 时使用；消耗 1 发子弹，造成伤害；若目标带有标记则追加一段伤害。 |
|  | `JHIN-STACCATO_FIRE` | 断奏射击 | Staccato Fire | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/StaccatoFire.cs` | 射击连续命中 2 次；华彩时改为 3 次。 |
|  | `JHIN-GORGEOUS_EXECUTION` | 华丽处刑 | Gorgeous Execution | 攻击 | `3` | 罕见 | 单体敌人 | `src/cards/GorgeousExecution.cs` | 高额单体伤害；若击杀目标则获得 2 点能量；使用后消耗。 |
|  | `JHIN-PERFECT_RELOAD` | 完美换弹 | Perfect Reload | 技能 | `1` | 罕见 | 自身 | `src/cards/PerfectReload.cs` | 将子弹装填至 4，并获得力量；使用后消耗。 |
|  | `JHIN-BARREL_CALIBRATION` | 枪口校准 | Barrel Calibration | 技能 | `1` | 罕见 | 单体敌人 | `src/cards/BarrelCalibration.cs` | 给予目标标记；若目标原本已有标记，则获得能量；使用后消耗。 |
|  | `JHIN-EXIT_ROUTE` | 退场路线 | Exit Route | 技能 | `1` | 罕见 | 自身 | `src/cards/ExitRoute.cs` | 获得格挡；若子弹为 0，额外抽 2 张牌。 |
|  | `JHIN-LENS_FOCUS` | 镜头聚焦 | Lens Focus | 技能 | `1` | 罕见 | 单体敌人 | `src/cards/LensFocus.cs` | 给予目标大量标记。 |
|  | `JHIN-SILENT_AUDIENCE` | 沉默观众 | Silent Audience | 技能 | `2` | 罕见 | 自身 | `src/cards/SilentAudience.cs` | 获得格挡，并对所有敌人施加虚弱。 |
|  | `JHIN-PERFECT_FINALE` | 完美谢场 | Perfect Finale | 技能 | `2` | 罕见 | 自身 | `src/cards/PerfectFinale.cs` | 获得格挡；若本回合已触发华彩，则获得 2 点能量；使用后消耗。 |
|  | `JHIN-HIDDEN_MECHANISM` | 暗藏机关 | Hidden Mechanism | 技能 | `1` | 罕见 | 单体敌人 | `src/cards/HiddenMechanism.cs` | 给予目标大量莲花陷阱。 |
|  | `JHIN-DEATH_NOTICE` | 死亡预告 | Death Notice | 技能 | `1` | 罕见 | 单体敌人 | `src/cards/DeathNotice.cs` | 给予目标标记和易伤；使用后消耗。 |
|  | `JHIN-SHOW_PAUSE` | 演出暂停 | Show Pause | 技能 | `0` | 罕见 | 自身 | `src/cards/ShowPause.cs` | 对所有敌人施加虚弱；并可触发弱化相关的标记联动；使用后消耗。 |
|  | `JHIN-POSITION_SWAP` | 换位演出 | Position Swap | 技能 | `0` | 罕见 | 自身 | `src/cards/PositionSwap.cs` | 抽 1 张牌，升级后抽 2 张；并获得 1 点能量；使用后消耗。 |
|  | `JHIN-DEATH_IS_ART` | 死亡是艺术 | Death is Art | 能力 | `2` | 罕见 | 自身 | `src/cards/DeathIsArt.cs` | 当敌人首次生命降到半血以下时获得力量；升级后层数更高。 |
|  | `JHIN-LOTUS_WORKSHOP` | 莲花工坊 | Lotus Workshop | 能力 | `2` | 罕见 | 自身 | `src/cards/LotusWorkshop.cs` | 每当你施加标记时，同时施加莲花陷阱。 |
|  | `JHIN-AUDIENCE_CHEER` | 观众喝彩 | Audience Cheer | 能力 | `2` | 罕见 | 自身 | `src/cards/AudienceCheer.cs` | 每次触发华彩时获得敏捷；升级后层数更高。 |
|  | `JHIN-CAREFUL_ARRANGEMENT` | 精心编排 | Careful Arrangement | 能力 | `1` | 罕见 | 自身 | `src/cards/CarefulArrangement.cs` | 每回合开始时额外抽牌。 |
|  | `JHIN-FINAL_ACT_FORESHADOWING` | 终幕伏笔 | Final Act Foreshadowing | 能力 | `2` | 罕见 | 自身 | `src/cards/FinalActForeshadowing.cs` | 每回合开始时获得能量。 |
|  | `JHIN-PERFECT_TRAJECTORY` | 完美弹道 | Perfect Trajectory | 能力 | `2` | 罕见 | 自身 | `src/cards/PerfectTrajectory.cs` | 你的所有射击牌获得额外伤害。 |
|  | `JHIN-MASTERPIECE_BORN` | 名作诞生 | Masterpiece Born | 能力 | `2` | 罕见 | 自身 | `src/cards/MasterpieceBorn.cs` | 当标记被射击消耗时抽牌。 |
|  | `JHIN-AESTHETIC_OF_FOUR` | 四的美学 | Aesthetic of Four | 能力 | `2` | 罕见 | 自身 | `src/cards/AestheticOfFour.cs` | 若回合开始时子弹为 4，获得能量；升级后还会抽牌。 |
|  | `JHIN-GRAND_STAGE` | 盛大布景 | Grand Stage | 技能 | `2` | 罕见 | 自身 | `src/cards/GrandStage.cs` | 获得较高格挡，并给予所有敌人 1 层莲花陷阱。 |
|  | `JHIN-LONG_RANGE_SNIPE` | 远距狙击 | Long-Range Snipe | 攻击 / 射击 | `2` | 罕见 | 单体敌人 | `src/cards/LongRangeSnipe.cs` | 高额单体射击，并按目标标记层数追加伤害。 |
|  | `JHIN-FOURTH_ACT` | 第四乐章 | Fourth Act | 攻击 / 射击 | `2` | 稀有 | 单体敌人 | `src/cards/FourthAct.cs` | 只能在子弹为 1 时使用；消耗 1 发子弹，造成终幕一击伤害，并按目标已损失生命的 10% 档位逐级提高伤害；使用后消耗。 |
|  | `JHIN-DEADLY_FLOURISH` | 致命华彩 | Deadly Flourish | 攻击 / 射击 | `1` | 罕见 | 单体敌人 | `src/cards/DeadlyFlourish.cs` | 射击造成伤害；若目标有标记则施加易伤，华彩时更强。 |
|  | `JHIN-FLOURISH_TEMPO` | 华彩节奏 | Flourish Tempo | 能力 | `2` | 罕见 | 自身 | `src/cards/FlourishTempo.cs` | 每次华彩获得能量；升级后还会抽牌。 |
|  | `JHIN-CURTAIN_CALL` | 谢幕 | Curtain Call | 攻击 / 谢幕 | `2` | 稀有 | 随机敌人 | `src/cards/CurtainCall.cs` | 只能在子弹为 0 时使用；随机攻击 4 次，目标血量越低伤害越高；使用后消耗。 |
|  | `JHIN-LAST_ACT` | 最后一幕 | Last Act | 攻击 / 射击 | `2` | 稀有 | 单体敌人 | `src/cards/LastAct.cs` | 只能在子弹为 1 时使用；高额射击伤害，华彩时获得 3 点能量。 |
|  | `JHIN-PERFECT_WORK` | 完美作品 | Perfect Work | 攻击 / 射击 | `2` | 稀有 | 单体敌人 | `src/cards/PerfectWork.cs` | 射击造成伤害，并按本场战斗华彩次数额外提高伤害。 |
|  | `JHIN-DEATH_MONOLOGUE` | 死亡独白 | Death Monologue | 攻击 | `3` | 稀有 | 单体敌人 | `src/cards/DeathMonologue.cs` | 高额单体伤害；目标低血量时再补一段爆发，若击杀则获得力量；使用后消耗。 |
|  | `JHIN-FULL_HOUSE_CHEER` | 满堂喝彩 | Full House Cheer | 攻击 | `3` | 稀有 | 全体敌人 | `src/cards/FullHouseCheer.cs` | 对所有敌人造成伤害，并按击杀数量回能；使用后消耗。 |
|  | `JHIN-QUARTET` | 四重奏 | Quartet | 攻击 / 射击 | `2` | 稀有 | 单体敌人 | `src/cards/Quartet.cs` | 消耗 1 层子弹，随机攻击 4 次；华彩时每段伤害更高。 |
|  | `JHIN-PERFECT_HIT` | 完美命中 | Perfect Hit | 攻击 / 射击 | `2` | 稀有 | 单体敌人 | `src/cards/PerfectHit.cs` | 射击造成高伤；若目标仍带有标记，再追加一段伤害。 |
|  | `JHIN-CURTAIN_CALL_AFTERMATH` | 谢幕余音 | Curtain Call Aftermath | 攻击 | `1` | 稀有 | 单体敌人 | `src/cards/CurtainCallAftermath.cs` | 只能在子弹为 0 时使用，造成高额伤害；使用后消耗。 |
|  | `JHIN-LAST_AUDIENCE` | 最后的观众 | Last Audience | 攻击 / 射击 | `3` | 稀有 | 单体敌人 | `src/cards/LastAudience.cs` | 高额射击伤害，并获得能量；使用后消耗。 |
|  | `JHIN-PRECISE_CURTAIN_CALL` | 精准谢幕 | Precise Curtain Call | 技能 | `0` | 稀有 | 自身 | `src/cards/PreciseCurtainCall.cs` | 将子弹直接设为 0 并回能；升级后额外抽 1 张牌；使用后消耗。 |
|  | `JHIN-CURTAIN_CALL_PREP` | 谢幕准备 | Curtain Call Prep | 技能 | `1` | 稀有 | 自身 | `src/cards/CurtainCallPrep.cs` | 大量抽牌并获得能量；使用后消耗。 |
|  | `JHIN-DEADLY_STAGE` | 致命舞台 | Deadly Stage | 技能 | `1` | 稀有 | 自身 | `src/cards/DeadlyStage.cs` | 对所有敌人同时施加标记和莲花陷阱；使用后消耗。 |
|  | `JHIN-PERFECT_PERFORMANCE` | 完美演出 | Perfect Performance | 技能 | `2` | 稀有 | 自身 | `src/cards/PerfectPerformance.cs` | 装填至 4，获得大量格挡并回能；使用后消耗。 |
|  | `JHIN-WHISPER_ECHO` | 低语回响 | Whisper Echo | 能力 | `2` | 稀有 | 自身 | `src/cards/WhisperEcho.cs` | 每次触发华彩时同时回能并抽牌。 |
|  | `JHIN-FATEMAKER` | 戏命师 | Fatemaker | 能力 | `2` | 稀有 | 自身 | `src/cards/Fatemaker.cs` | 提高能量上限 1，并在每回合开始时自动射出 1 发子弹随机攻击敌人。 |
|  | `JHIN-PERFECT_CRIME` | 完美犯罪 | Perfect Crime | 能力 | `2` | 稀有 | 自身 | `src/cards/PerfectCrime.cs` | 立即获得力量，并在之后每回合开始时继续获得力量。 |
|  | `JHIN-FINAL_ACT_ART` | 终幕艺术 | Final Act Art | 能力 | `2` | 稀有 | 自身 | `src/cards/FinalActArt.cs` | 每回合开始时，若有敌人生命低于阈值则获得能量。 |
|  | `JHIN-SOUL_SHOT` | 灵魂射击 | Soul Shot | 攻击 / 射击 | `1` | 远古 | 单体敌人 | `src/cards/SoulShot.cs` | 远古射击牌；按标记提高伤害并重新施加标记，击杀时返还子弹，华彩形态更强。 |
|  | `JHIN-FINAL_ACT_RELOAD` | 终幕装填 | Final Act Reload | 技能 | `0` | 远古 | 自身 | `src/cards/FinalActReload.cs` | 装填至 4，抽 3 张牌，并让本回合下一张射击必定触发华彩；使用后消耗。 |



## 备注

| 项目 | 说明 |
| --- | --- |
| 射击牌统一入口 | 当前所有射击牌统一继承 `AbstractShootCard`，负责子弹消耗、可打出条件校验以及华彩上下文处理。 |
| 文档定位 | 本文根据 `src/cards/*.cs` 与 `JhinMod/localization/*/cards.json` 同步整理，`当前效果摘要` 仅做快速索引，不替代代码与正式文案。 |
| 标记 / 莲花 / 华彩 / 装填联动 | 这些系统已被多张攻击、技能与能力牌接入；具体数值、升级差异与消耗行为以实现代码为准。 |
| 谢幕体系 | `谢幕`、`谢幕余音`、`精准谢幕`、`谢幕准备` 组成独立的空弹匣爆发分支，与普通射击体系并行。 |
| 占位图规则 | 当前卡牌默认使用 `JhinMod/Images/card_placeholder.png`。 |
