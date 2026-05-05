# 📜 AGENTS.md

## 严重规范

不允许使用任何subagent，所有功能必须由主agent完成。

## 🎯 资源使用规范（强制）

### 1. 卡牌资源（Card Images）

* **默认规则：**

  * 所有卡牌图片 **必须使用占位图**

* **占位图路径：**

  ```
  JhinMod\Images\card_placeholder.png
  ```

* **例外情况：**

  * 仅当需求中**明确指定使用自定义图片**时，才允许替换占位图

---

### 2. 角色资源（Character / Role Images）

* **适用范围：**

  * 角色立绘
  * 头像
  * 相关展示图

* **默认规则：**

  * 所有角色相关图片 **必须使用占位图**

* **占位图路径：**

  ```
  JhinMod\Images\role_placeholder.png
  ```

* **例外情况：**

  * 仅当需求中**明确指定角色图片资源**时，才允许使用非占位图

---

## ⚠️ 优先级规则

1. **显式需求 > 本规范**

   * 用户明确要求真实资源 → 可以覆盖占位图规则

2. **未说明 = 必须占位图**

   * 任何未提及图片资源的情况，一律使用占位图

## 文案

所有文案都不能硬编码在代码中，必须在JhinMod\localization中进行编写和管理，然后参考项目中的示例进行使用。

## 函数方法调用

不允许使用游戏的私有函数方法，必须使用公开的API接口进行调用。

## 日志

运行时日志在

```
C:\Users\<YourUsername>\AppData\Roaming\SlayTheSpire2\logs
```
YourUsername 需要替换为实际的Windows用户名
。


## 文档

baselib

```
https://alchyr.github.io/BaseLib-Wiki/docs/Features.html
```

## 碰到复杂问题怎么办

先去 `knowledge/` 目录下看看有没有相关的笔记.

## 修改后

必须执行编译和部署流程.
