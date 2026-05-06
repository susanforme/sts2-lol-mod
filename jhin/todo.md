# TODO

## bug

- [ ] 装填这张牌不能是消耗,删除抽牌

- [ ] 卡牌颜色或许要调整

- [ ] 普通牌过多需要调整.保证比例为20#36#26

- [ ] 终幕伏笔 现在会给自己加buff.每回合开始获得能量.但是不是4/4 这样加而是4/- [ ]且这个图标能服用以前的吗?

- [ ] 我记得游戏界面能看自己有那些牌的稀有度?是单人模式和多人模式的区别吗

- [x] 敌人身上有标记buff的时候重复显示了tooltip 又显示了一次标记的介绍 **没验证**

- [ ] 标记的tooltip伤害还是占位符没有正确显示出来

- [x] 在敌人身上有标记buff的时候,选中敌人手上的攻击牌伤害没有正确显示出来.且伤害应该合并计算**没验证**

- [ ] 完美作品的 文案存在问题. 本场战斗每触发一次华彩额外造成4点伤害

- [ ] 镜头锁定文案不太清晰优化下

- [ ] 幕间休息存在bug. 使用 打击牌攻击敌人时. 卡牌一直悬浮无法结算. 且没有额外伤害

- [ ] 增加一张牌 第二声枪响. 以达到 01234 都有 不过需要注意稀有度.0应该已经有了就是谢幕

- [x] 不管是遗物还是牌 不需要限制是不是第一次华彩.华彩有点不好打 **没验证**

- [x] 文案修改 1层 子弹 都改为 1 发子弹 **没验证**

- [ ] 低语齐射太弱了  改为消耗2发子弹  对所有敌人造成8点伤害

- [ ] 速度把能量的图标给做了

- [ ] 完美命中的 伤害计算逻辑有问题.敌人身上有2层标记和1层易伤. 结算伤害为- [ ]  按理来说乘法计算顺序有问题

- [ ] 出一张x牌 弹匣倾泻.  打出几发就消耗几发子弹和能量

- [x] 死亡绽放 没有莲花陷阱的tooltip **没验证**

- [x] 重大bug

```
[ERROR] Localization formatting error! message=Error parsing format string: No source extension could handle the selector named "markDamagePerStack" at 40
当烬下一张[gold]射击[/gold]牌命中带有标记的敌人时，每层额外造成 {markDamagePerStack} 点伤害并移除所有标记。非射击伤害不会消耗标记。
----------------------------------------^
table=card_keywords key=JHIN-MARK.description variables={energyPrefix:}
   at BaseLib.Patches.Localization.CustomTooltips.DynamicKeywordTips.CustomKeyword(CardKeyword keyword, IHoverTip& __result)
   at MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromKeyword_Patch1(CardKeyword keyword)
   at jhin.Powers.MarkPower.get_ExtraHoverTips()
   at MegaCrit.Sts2.Core.Models.PowerModel.get_HoverTips()
   at MegaCrit.Sts2.Core.Entities.Creatures.Creature.get_HoverTips()
   at MegaCrit.Sts2.Core.Nodes.Combat.NCreature.OnFocus()
   at Godot.Callable.<From>g__Trampoline|1_0(Object delegateObj, NativeVariantPtrArgs args, godot_variant& ret)
   at Godot.DelegateUtils.InvokeWithVariantArgs(IntPtr delegateGCHandle, Void* trampoline, godot_variant** args, Int32 argc, godot_variant* outRet)

[ERROR] Localization formatting error! message=Error parsing format string: No source extension could handle the selector named "markDamagePerStack" at 40
当烬下一张[gold]射击[/gold]牌命中带有标记的敌人时，每层额外造成 {markDamagePerStack} 点伤害并移除所有标记。非射击伤害不会消耗标记。
----------------------------------------^
table=card_keywords key=JHIN-MARK.description variables={energyPrefix:}
   at BaseLib.Patches.Localization.CustomTooltips.DynamicKeywordTips.CustomKeyword(CardKeyword keyword, IHoverTip& __result)
   at MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromKeyword_Patch1(CardKeyword keyword)
   at jhin.Powers.MarkPower.get_ExtraHoverTips()
   at MegaCrit.Sts2.Core.Models.PowerModel.get_HoverTips()
   at MegaCrit.Sts2.Core.Entities.Creatures.Creature.get_HoverTips()
   at MegaCrit.Sts2.Core.Nodes.Combat.NCreature.OnFocus()
   at Godot.Callable.<From>g__Trampoline|1_0(Object delegateObj, NativeVariantPtrArgs args, godot_variant& ret)
   at Godot.DelegateUtils.InvokeWithVariantArgs(IntPtr delegateGCHandle, Void* trampoline, godot_variant** args, Int32 argc, godot_variant* outRet)

[INFO] Monster BOWLBUG_SILK performing move TOXIC_SPIT_MOVE
[INFO] Monster SLUMBERING_BEETLE performing move SNORE_MOVE
[INFO] [sts2.piyixiajiuhenfen.damagemeter] [UITrace] Window 0 UpdateDisplay #150 (category=0, view=-1, players=1)
[INFO] [sts2.piyixiajiuhenfen.damagemeter] SaveCombatState: 1 players, 6 segs, turn 3
[ERROR] Localization formatting error! message=Error parsing format string: No source extension could handle the selector named "markDamagePerStack" at 40
当烬下一张[gold]射击[/gold]牌命中带有标记的敌人时，每层额外造成 {markDamagePerStack} 点伤害并移除所有标记。非射击伤害不会消耗标记。
----------------------------------------^
table=card_keywords key=JHIN-MARK.description variables={energyPrefix:}
   at BaseLib.Patches.Localization.CustomTooltips.DynamicKeywordTips.CustomKeyword(CardKeyword keyword, IHoverTip& __result)
   at MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromKeyword_Patch1(CardKeyword keyword)
   at jhin.Powers.MarkPower.get_ExtraHoverTips()
   at MegaCrit.Sts2.Core.Models.PowerModel.get_HoverTips()
   at MegaCrit.Sts2.Core.Entities.Creatures.Creature.get_HoverTips()
   at MegaCrit.Sts2.Core.Nodes.Combat.NCreatureStateDisplay.OnHovered()
   at Godot.Callable.<From>g__Trampoline|1_0(Object delegateObj, NativeVariantPtrArgs args, godot_variant& ret)
   at Godot.DelegateUtils.InvokeWithVariantArgs(IntPtr delegateGCHandle, Void* trampoline, godot_variant** args, Int32 argc, godot_variant* outRet)

[ERROR] Localization formatting error! message=Error parsing format string: No source extension could handle the selector named "markDamagePerStack" at 40
当烬下一张[gold]射击[/gold]牌命中带有标记的敌人时，每层额外造成 {markDamagePerStack} 点伤害并移除所有标记。非射击伤害不会消耗标记。
----------------------------------------^
table=card_keywords key=JHIN-MARK.description variables={energyPrefix:}
   at BaseLib.Patches.Localization.CustomTooltips.DynamicKeywordTips.CustomKeyword(CardKeyword keyword, IHoverTip& __result)
   at MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromKeyword_Patch1(CardKeyword keyword)
   at jhin.Powers.MarkPower.get_ExtraHoverTips()
   at MegaCrit.Sts2.Core.Models.PowerModel.get_HoverTips()
   at MegaCrit.Sts2.Core.Entities.Creatures.Creature.get_HoverTips()
   at MegaCrit.Sts2.Core.Nodes.Combat.NCreature.OnFocus()
   at Godot.Callable.<From>g__Trampoline|1_0(Object delegateObj, NativeVariantPtrArgs args, godot_variant& ret)
   at Godot.DelegateUtils.InvokeWithVariantArgs(IntPtr delegateGCHandle, Void* trampoline, godot_variant** args, Int32 argc, godot_variant* outRet)

[ERROR] System.InvalidOperationException: Trying to add multiple instances of a non-instanced power to a creature.
   at MegaCrit.Sts2.Core.Entities.Creatures.Creature.ApplyPowerInternal(PowerModel power)
   at MegaCrit.Sts2.Core.Models.PowerModel.ApplyInternal(Creature owner, Decimal amount, Boolean silent)
   at jhin.Actions.JhinCombatActionUtil.ApplyOrStackStrength(Creature target, Int32 amount)
   at jhin.Powers.ActorsInstinctPower.OnTurnStart()
   at jhin.Patches.PlayerCombatStateResetEnergyPatch.Postfix(PlayerCombatState __instance)
   at MegaCrit.Sts2.Core.Combat.CombatManager.SetupPlayerTurn(Player player, HookPlayerChoiceContext playerChoiceContext)
   at MegaCrit.Sts2.Core.Helpers.TaskHelper.WhenAny(Task[] tasks)
   at MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext.WaitForPauseOrCompletionWithoutAssigningTask(Task task)
   at MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(Func`1 actionDuringEnemyTurn)
   at MegaCrit.Sts2.Core.Combat.CombatManager.EndEnemyTurn()
   at MegaCrit.Sts2.Core.Combat.CombatManager.ExecuteEnemyTurn(Func`1 actionDuringEnemyTurn)
   at MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(Func`1 actionDuringEnemyTurn)
   at MegaCrit.Sts2.Core.Combat.CombatManager.SwitchFromPlayerToEnemySide(Func`1 actionDuringEnemyTurn)
   at MegaCrit.Sts2.Core.Combat.CombatManager.AfterAllPlayersReadyToBeginEnemyTurn(Func`1 actionDuringEnemyTurn)
   at MegaCrit.Sts2.Core.Helpers.TaskHelper.LogTaskExceptions(Task task)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MegaCrit.Sts2.Core.Combat.CombatManager.AfterAllPlayersReadyToBeginEnemyTurn(Func`1 actionDuringEnemyTurn)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.ExecutionContextCallback(Object s)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext()
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MegaCrit.Sts2.Core.Combat.CombatManager.SwitchFromPlayerToEnemySide(Func`1 actionDuringEnemyTurn)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.ExecutionContextCallback(Object s)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext()
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(Func`1 actionDuringEnemyTurn)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.ExecutionContextCallback(Object s)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext()
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MegaCrit.Sts2.Core.Combat.CombatManager.ExecuteEnemyTurn(Func`1 actionDuringEnemyTurn)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.ExecutionContextCallback(Object s)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext()
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MegaCrit.Sts2.Core.Combat.CombatManager.EndEnemyTurn()
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.ExecutionContextCallback(Object s)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext()
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(Func`1 actionDuringEnemyTurn)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.ExecutionContextCallback(Object s)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext()
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at MegaCrit.Sts2.Core.Commands.Cmd.CustomScaledWait(Single fastSeconds, Single standardSeconds, Boolean ignoreCombatEnd, CancellationToken cancellationToken)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at MegaCrit.Sts2.Core.Commands.Cmd.Wait(Single seconds, CancellationToken cancelToken, Boolean ignoreCombatEnd)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Threading.Tasks.AwaitTaskContinuation.RunCallback(ContextCallback callback, Object state, Task& currentTask)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.TrySetResult()
   at MegaCrit.Sts2.Core.Commands.Cmd.<>c__DisplayClass2_0.<WaitInternal>g__Receive|0()
   at Godot.Callable.<From>g__Trampoline|1_0(Object delegateObj, NativeVariantPtrArgs args, godot_variant& ret)
   at Godot.DelegateUtils.InvokeWithVariantArgs(IntPtr delegateGCHandle, Void* trampoline, godot_variant** args, Int32 argc, godot_variant* outRet)

[ERROR] Localization formatting error! message=Error parsing format string: No source extension could handle the selector named "markDamagePerStack" at 40
当烬下一张[gold]射击[/gold]牌命中带有标记的敌人时，每层额外造成 {markDamagePerStack} 点伤害并移除所有标记。非射击伤害不会消耗标记。
----------------------------------------^
table=card_keywords key=JHIN-MARK.description variables={energyPrefix:}
   at BaseLib.Patches.Localization.CustomTooltips.DynamicKeywordTips.CustomKeyword(CardKeyword keyword, IHoverTip& __result)
   at MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromKeyword_Patch1(CardKeyword keyword)
   at jhin.Powers.MarkPower.get_ExtraHoverTips()
   at MegaCrit.Sts2.Core.Models.PowerModel.get_HoverTips()
   at MegaCrit.Sts2.Core.Entities.Creatures.Creature.get_HoverTips()
   at MegaCrit.Sts2.Core.Nodes.Combat.NCreature.OnFocus()
   at Godot.Callable.<From>g__Trampoline|1_0(Object delegateObj, NativeVariantPtrArgs args, godot_variant& ret)
   at Godot.DelegateUtils.InvokeWithVariantArgs(IntPtr delegateGCHandle, Void* trampoline, godot_variant** args, Int32 argc, godot_variant* outRet)
```

### 消耗错误

~~枪口校准,换位演出(改为抽1张牌 ) ,最后的观众~~

## 重大注意事项

- [ ] 需要统一.先乘法还是先加法.


- [ ] 文案中的额外伤害.例如完美命中这样的. 若目标带有标记额外xxx  有没有更好的解决方式? 也可以直接选中目标的时候把伤害加上.涉及如下牌
    完美命中
   艺术切割 (文案优化,额外造成一次)

## 重做删除牌

- [ ] 试演一枪  这张牌反而在不是华彩的时候有收益?重做或者删除

## 机制

- [ ] 有的卡牌太冗余了.升级不应该增加额外效果.而是在现有机制增强数值

- [x] 优化遗物 机制(每当弹匣填满抽一张牌.需要在弹匣没满的时候) 需要注意原有卡牌中的就需要删除

- [x] 将莲花陷阱和标记合二为一 不能合并!!!!