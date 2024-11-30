<div align="center">

![LOGO](./Ink-Canvas-Reborn-Logo.png)

# Ink Canvas Reborn [EN]

[Download](https://github.com/dubi906w/Ink-Canvas-Reborn/releases/latest) | [Manual](https://github.com/dubi906w/Ink-Canvas-Reborn/blob/master/MANUAL.md) | [FAQ](https://github.com/dubi906w/Ink-Canvas-Reborn#FAQ)
  
[![QQ Group](https://img.shields.io/badge/-QQ_Group%20195404368-blue?style=flat&logo=TencentQQ)](https://qm.qq.com/q/PDfJCGLqwM)  ![GitHub issues](https://img.shields.io/github/issues/dubi906w/Ink-Canvas-Reborn?logo=github)

---

🤓☝️ **AUV，您吉祥！** 我是地地道道的中国人，返回 **[中文版](./README.md)**

---

A fantastic Ink Canvas in WPF/C#, with fantastic support for Seewo Boards.

When schools switched from traditional projectors to Seewo Iwb *(like MAXHUB Interactive Flat Panel, both were manufactured by Cvte, MAXHUB is a subsidiary of Cvte)*, the default `"EasiNote5"` software turned out to be quite inconvenient, and alternative whiteboard tools were scarce. To address this, [WXRIW](https://github.com/WXRIW) developed the original [Ink Canvas](https://github.com/WXRIW/Ink-Canvas) project.

However, since [WXRIW](https://github.com/WXRIW) graduated from high school, the project has been mostly inactive. To fix lingering bugs, `dubi906w` has restructured and enhanced the software.

</div>

## 👀 Introduction

Before using or distributing this software (Ink Canvas Reborn), you must acknowledge and comply with the relevant open-source license. This software is derived from [WXRIW/Ink-Canvas](https://github.com/WXRIW/Ink-Canvas).


**⚠️ Warning:** `dubi906w` takes no responsibility for any loss or damage caused by using this software in public occasions!


## 🔧 Features

- Optimized support for Microsoft PowerPoint and WPS Office.
- **Write with the fine tip of the pen; flip it around to use the eraser. (This is not supported by the Seewo Whiteboard software.)**
- Improved palm rejection for erasing ink on the canvas.
- Hand-drawn **geometric shapes** can be automatically converted to **standard shapes**.
- Completely rewritten from the original `Ink Canvas` project, adding many new features, UI animations, interface upgrades, bug fixes, and improved usability for a smoother user experience.

## ℹ️ Tips

- Please read the [FAQ](https://github.com/WXRIW/Ink-Canvas#FAQ) before asking questions.
- If you encounter issues, try resolving them independently first. If you're unable to do so, describe the difference between your expectations and actual outcomes. If necessary, provide reproduction steps or error logs¹ (screenshots are welcome) and wait for a response.
- The developer welcomes constructive feedback and reasonable suggestions for new features. Ink Canvas is a non-commercial project—please be patient and allow time for bug-free, stable development.

> Patience is a virtue.

[1]: For lengthy log files, consider using an online clipboard services and share the link. (e.g., [https://pastes.dev/](https://pastes.dev/))

## 📗 FAQ

### Why does the program crash immediately after switch to the next build in PowerPoint presentation mode?

This is likely caused by an unactivated version of `Microsoft Office`. Please activate your Office installation.

### Why doesn’t the program switch to Presentator mode after starting a PowerPoint presentation file?

If you've installed `WPS Office` (which is basically a Chinese-specified knockoff of Microsoft Office) in the past and uninstalled it, this issue may arise due to 32-bit Office COM interfaces being overwritten by `WPS Office`. Full Reinstalling the  `Office 365 x86` should fix this problem.

> WPS Office is totally garbage. It's true, believe me.

Additionally, Presentation files in "protected (read-only)" mode cannot be recognized.

Make sure `Microsoft PowerPoint` and `Ink Canvas` are running with the same permissions. Different permission levels may also prevent the program from switching to **Presentator mode**.

### Why doesn’t the program start after installation?

Ensure that your computer has `.Net Framework 4.7.2` or a newer version installed. If not, download and install it.

If the issue persists, check if `Microsoft Office` is installed. If not, install Office and try again.

### Where can I submit feature requests or bug reports?

1. **GitHub Issues**  
   Feature Requests: [https://github.com/dubi906w/Ink-Canvas-Reborn/labels/enhancement/new](https://github.com/dubi906w/Ink-Canvas-Reborn/labels/enhancement/new)  
   Bug Reports: [https://github.com/dubi906w/Ink-Canvas-Reborn/labels/bug/new](https://github.com/dubi906w/Ink-Canvas-Reborn/labels/bug/new)

2. **Tencent QQ** (For Chinese users) 
   [![QQ Group](https://img.shields.io/badge/-%E4%BA%A4%E6%B5%81%E7%BE%A4%20195404368-blue?style=flat&logo=TencentQQ)](https://qm.qq.com/q/PDfJCGLqwM)

### What if switching between large and small screens or using fingers or thick pen tips triggers the eraser?

Click the `Settings` button in the toolbar, then enable the `Chinese TouchScreen/Iwb support.` option, or disable the `Use pen area to determine gesture eraser.` option.

### Does the software support languages other than Simplified Chinese (i18n)?

Yes, it does! We welcome contributions to the localization of this project. Simply place the corresponding language file in the root directory where the program is running. Then, click the "Settings" button in the toolbar, go to the settings page, and change the language to switch. For details on how to create language files, please refer to the program source code.

( Translation isn’t difficult—it’s easy to get started! You can even ask ChatGPT to help with translations. However, **<u>WE DO NOT RECOMMEND</u>** using ChatGPT or services like Google Translate to **<u>BULK-TRANSLATE</u>** and then submit a pull request. These services may not understand the context of each field, leading to inaccurate or inappropriate translations! )


## 🙏 Acknowledgments

Special thanks to [yuwenhui2020](https://github.com/yuwenhui2020) for contributing to the Ink Canvas user manual!

Thanks also to [WXRIW](https://github.com/WXRIW), [Raspberry Kan](https://github.com/Raspberry-Monster), [Kengwang](https://github.com/kengwang), [Charles Jia](https://github.com/jiajiaxd), [clover_yan](https://github.com/clover-yan), [Netherite_Bowl](https://github.com/NetheriteBowl), [Yoojun Zhou](https://github.com/NotYoojun), [ZongziTEK](https://github.com/STBBRD), and [ChangSakura](https://github.com/ChangSakura) for contributing to the original project.

Gratitude also to [InkCanvas/InkCanvasForClass](https://github.com/InkCanvas/InkCanvasForClass) for their contributions to the project’s development!

## ✅ License

GPL-3.0 license

---
---
---
---
---
---

Let the past be the past.

ICC is now history. 

**The old "doubx690i" is no more.**

> `dubi906w` is the developer of this software and is unrelated to the user/developer known as "逗比的九百有六大不留" or "Doubx690i" (`kriastans`). The shared name is purely intentional.