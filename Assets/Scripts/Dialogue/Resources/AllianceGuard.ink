EXTERNAL getMainQuestStatus()
EXTERNAL npcAction(name)
EXTERNAL setEnding()

VAR mainQuestStatus = ""
VAR mainQuestAmount = 0
-> main

=== main ===
~getMainQuestStatus()
{mainQuestStatus == "complete":
Master Jin, you are back.
Carry on straight down to the headquarters...
~ setEnding()
    ~npcAction("leaveX")
-> END
}
{mainQuestStatus == "incomplete":
Master Jin I don't believe you have visited all the leaders.
-> END
}