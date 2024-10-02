EXTERNAL setQuestStatus(questName, questStatus)
EXTERNAL getQuestStatus(questName)
EXTERNAL npcAction(name)

VAR questStatus = "incomplete"
-> main

=== main ===
~ getQuestStatus("questShaolin")
{questStatus == "incomplete":
    Are you him?
    *[I am Namgung Jin]
        I have received an order to let you through young master.
        Amitabha
        ~ npcAction("leave")
        ->END
}
{questStatus== "unobtained":
Sorry no one can go into the bamboo forest currently.
->END
}