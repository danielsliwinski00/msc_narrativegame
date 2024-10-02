EXTERNAL setQuestStatus(questName, questStatus)
EXTERNAL getQuestStatus(questName)
EXTERNAL npcAction(name)
EXTERNAL debug(name)

VAR questStatus = "incomplete"
VAR questStatusTang = ""
-> main

=== main ===
{questStatus == "complete":
~ npcAction("leave")
    ->END
}
->END