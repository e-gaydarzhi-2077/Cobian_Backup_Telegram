# CobianBackupTelegram

This is a console program for Windows, that is used to send notifications to telegrams about unsuccessful backups.
The code is written entirely in c# .Net Framework 4.6
The program does not affect the Cobian Backup service, does not change the files of the current (active) log.

[Release For Windows](https://github.com/e-gaydarzhi-2077/Cobian_Backup_Telegram/releases/tag/v1.0)

Example:
![image](https://user-images.githubusercontent.com/107859162/190390771-3025e227-589b-4b5d-853a-3af6be55adce.png)


After you have downloaded and extracted the files, you need to modify the ```CobianBackupTG.cfg``` file
Line 0 is the bot's api token
Line 1 is the group id

(Note the group id is written with a - sign)
(Id of personal correspondence is written without this sig)

Then add a finishing action (To the last task)
Run ```CobianBackupTG.exe```

![image](https://user-images.githubusercontent.com/107859162/190393595-72d7baab-d8b2-4c62-98dd-44d180870008.png)
